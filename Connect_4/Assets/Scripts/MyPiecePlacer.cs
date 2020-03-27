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
	public GameObject CollidingSlot;

	// Speed that the object should move at
	private float Speed = 20.0f;

	// Zero Vector for disabling speed
	private Vector3 ZeroSpeed = new Vector3(0, 0, 0);	

	// Used to preserve rotation
	// This assumes no rotation of the board which is incorrect
	private Quaternion Rotation = Quaternion.Euler(90, 0, 0);

	// This objects bounds
	private Collider ThisCollider;

	public Vector3 TouchPosition;

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
			InitialisePiecePlacer();
		}
		else
		{
			IsOwned = false;
		}
	}

	void FixedUpdate()
	{
		if (IsOwned)
		{
			// Move toward mouse if selected and not colliding with a slot and not in the board
			if(IsSelected && !IsColliding && !IsPlaced)
			{
				MoveTowardCursor();
			}
			// Piece Placed
			if (!IsSelected && IsColliding)
			{
				Fall();
				IsPlaced = true;
				GameplayManager.PieceJustPlaced = gameObject;
				GameplayManager.PieceJustPlacedColour = Colour;
			}
			// Assume no collision to begin
			IsColliding = false;
			for (int i = 0; i < Slots.Length; i++)
			{
				float slotToPiece = Vector3.Distance(Slots[i].transform.position, TouchPosition);

				bool cursorOutsideSlot = slotToPiece > Radius;

				if (ThisCollider.bounds.Intersects(SlotsBounds[i]) && !cursorOutsideSlot)
				{
					if (IsSelected)
					{
						IsColliding = true;
						CollidingSlot = Slots[i];
						Magnetise(CollidingSlot);
					}
				}
			}
			if (!IsSelected && !IsPlaced)
			{
				Fall();
			}
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
		Rb.useGravity = false;
		Rb.isKinematic = true;

		// Need to fix the rotation variable
		transform.rotation = Rotation;
		Rb.position = Vector3.MoveTowards(transform.position, TouchPosition, Speed * Time.deltaTime);
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

		Radius = gameObject.transform.localScale.x / 2;

		ThisCollider = GetComponent<CapsuleCollider>();

		for (int i = 0; i < g.transform.childCount; i++)
		{
			Slots[i] = g.transform.GetChild(i).gameObject;
			SlotsBounds[i] = Slots[i].GetComponent<SphereCollider>().bounds;
		}
	}
}
