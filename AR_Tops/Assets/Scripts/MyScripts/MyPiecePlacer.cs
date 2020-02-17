using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MyPiecePlacer : MonoBehaviourPun
{
	private float Radius;

	// The Slots
	private GameObject[] Slots;

	// The Bounds of all the slots
	private Bounds[] SlotsBounds;

	// The Slot it is colliding with
	public GameObject Colliding_slot;

	// Speed that the object should move at
	private float Speed = 20.0f;

	// Zero Vector for disabling speed
	private Vector3 ZeroSpeed = new Vector3(0, 0, 0);

	// The controller
	private MyPieceController PieceController;

	// Used to preserve rotation
	// This assumes no rotation of the board which is incorrect
	private Quaternion Rotation = Quaternion.Euler(90, 0, 0);

	// This objects bounds
	private Collider This_collider;

	// Gameplay Manager
	private MyGameplayManager GameplayManager;

	private Rigidbody Rb;

	// Colour of the piece
	public string Colour;

	// Used to determine whether to take control away from the user
	public bool IsColliding;

	// Used to determine whether to follow the input of the user
	public bool IsSelected;

	// Used for placing in the board
	public bool IsPlaced;

	// Used for determining whtether it is now placed into a position on the board
	public bool IsInPosition;

	// Used for determining can I move this piece
	public bool IsOwned;

	void Start()
	{
		if (photonView.IsMine)
		{
			IsOwned = true;
		}
		else
		{
			IsOwned = false;
		}
		InitialisePiecePlacer();
	}

	void FixedUpdate()
	{
		// Move toward mouse if selected and not colliding with a slot and not in the board
		if (IsSelected && !IsColliding && !IsPlaced)
		{
			MoveTowardCursor();
		}
		if(!IsSelected && !IsPlaced)
		{
			Fall();
		}

		// Assume no collision to begin
		IsColliding = false;

		for (int i = 0; i < Slots.Length; i++)
		{
			float slotToPiece = Vector3.Distance(Slots[i].transform.position, PieceController.mousePosition);

			bool cursorOutsideSlot = slotToPiece > Radius;

			if (This_collider.bounds.Intersects(SlotsBounds[i]) && !cursorOutsideSlot)
			{
				IsColliding = true;

				// If they are still holding the object, keep it there, otherwise let it fall
				if (IsSelected)
				{
					Colliding_slot = Slots[i];
					Magnetise(Colliding_slot);
				}
			}
		}

		// Piece Placed
		if(!IsSelected && IsColliding)
		{
			Fall();
			IsPlaced = true;
			GameplayManager.PieceJustPlaced = gameObject;
			GameplayManager.PieceJustPlacedColour = Colour;
		}
	}

	public void Magnetise(GameObject slot)
	{
		GetComponent<Rigidbody>().position = slot.transform.position;
		GetComponent<Rigidbody>().rotation = slot.transform.rotation;
	}

	void MoveTowardCursor()
	{
		Rb.velocity = ZeroSpeed;

		// Need to fix the rotation variable
		transform.rotation = Rotation;
		Rb.position = Vector3.MoveTowards(transform.position, PieceController.mousePosition, Speed * Time.deltaTime);
		
		Rb.useGravity = false;
		Rb.isKinematic = true;
	}

	void Fall()
	{
		Rb.useGravity = true;
		Rb.isKinematic = false;
	}

	// Sets variables
	void InitialisePiecePlacer()
	{
		Slots = new GameObject[7];
		SlotsBounds = new Bounds[7];
	
		Rb = GetComponent<Rigidbody>();

		GameObject g = GameObject.Find("Connect_4_Board_Slots");

		GameplayManager = GameObject.Find("GameplayManager").GetComponent<MyGameplayManager>();

		PieceController = GameObject.Find("PieceController").GetComponent<MyPieceController>();

		Radius = gameObject.transform.localScale.x / 2;

		This_collider = GetComponent<CapsuleCollider>();

		for (int i = 0; i < g.transform.childCount; i++)
		{
			Slots[i] = g.transform.GetChild(i).gameObject;
			SlotsBounds[i] = Slots[i].GetComponent<SphereCollider>().bounds;
		}
	}
}
