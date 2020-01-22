using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPiecePlacer : MonoBehaviour
{
	/* Game Board */
	// Number of Slots
	private int size = 7;

	private float radius;

	// The Slots
	private GameObject[] slots;

	// The Slot it is colliding with
	private GameObject colliding_slot;

	// Speed that the object should move at
	private float Speed = 20.0f;

	// Zero Vector for disabling speed
	private Vector3 ZeroSpeed = new Vector3(0, 0, 0);

	// The controller
	private MyPieceController pieceController;

	// Used to preserve rotation
	private Quaternion rotation = Quaternion.Euler(90, 0, 0);
	
	// This objects bounds
	private Collider this_collider;

	// Used to determine whether to take control away from the user
	public bool isColliding;

	// Used to determine whether to follow the input of the user
	public bool isSelected;

	void Start()
	{
		InitialisePiecePlacer();
	}

	void FixedUpdate()
	{
		// Move toward mouse if selected
		if (isSelected && !isColliding)
		{
			MoveTowardCursor();
		}
		// Otherwise, re-enable gravity
		else
		{
			GetComponent<Rigidbody>().useGravity = true;
		}

		// Assume no collision to begin
		isColliding = false;

		this_collider = GetComponent<CapsuleCollider>();
		foreach (GameObject slot in slots)
		{
			Bounds slotBounds = slot.GetComponent<SphereCollider>().bounds;

			float slotToPiece = Vector3.Distance(slot.transform.position, pieceController.mousePosition);

			bool cursorOutsideSlot = slotToPiece > radius;

			if (this_collider.bounds.Intersects(slotBounds) && Input.GetMouseButton(0) && !cursorOutsideSlot)
			{
				isColliding = true;
				colliding_slot = slot;
				if (colliding_slot.GetComponent<MyMagnetismScript>().WillMagnetise)
				{
					Vector3 slotPosition = colliding_slot.transform.position;
					transform.SetPositionAndRotation(colliding_slot.transform.position, rotation);
				}
			}
		}
	}

	public void InBoard()
	{
		if(this_collider)
	}

	// Sets variables
	void InitialisePiecePlacer()
	{
		slots = new GameObject[size];

		pieceController = GameObject.Find("PieceController").GetComponent<MyPieceController>();
		GameObject g = GameObject.Find("Connect_4_Board_Slots");

		radius = gameObject.transform.localScale.x / 2;

		transform.rotation = rotation;

		GetComponent<Rigidbody>().freezeRotation = true;

		this_collider = GetComponent<CapsuleCollider>();
		
		for (int i = 0; i < g.transform.childCount; i++)
		{
			slots[i] = g.transform.GetChild(i).gameObject;
		}
	}

	void MoveTowardCursor()
	{
		GetComponent<Rigidbody>().useGravity = false;
		GetComponent<Rigidbody>().velocity = ZeroSpeed;
		transform.position = 
			Vector3.MoveTowards(transform.position, pieceController.mousePosition, Speed * Time.deltaTime);
	}
}
