﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MyPiecePlacer : MonoBehaviourPun
{
	/* Game Board */
	// Number of Slots
	private int size = 7;

	private float radius;

	// The Slots
	private GameObject[] slots;

	// The Slot it is colliding with
	public GameObject colliding_slot;

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

	// Gameplay Manager
	private MyGameplayManager gameplayManager;

	// Color of the piece
	public string Colour;

	// Used to determine whether to take control away from the user
	public bool isColliding;

	// Used to determine whether to follow the input of the user
	public bool isSelected;

	// Used for placing in the board
	public bool isPlaced;

	// Used for determining whtether it is now placed into a position on the board
	public bool isInPosition;

	// Used for determining can I move this piece
	public bool isOwned;

	void Start()
	{
		if (photonView.IsMine)
		{
			isOwned = true;
		}
		else
		{
			isOwned = false;
		}
		InitialisePiecePlacer();
	}

	void FixedUpdate()
	{
		// Move toward mouse if selected
		if (isSelected && !isColliding && !isPlaced)
		{
			MoveTowardCursor();
		}
		// Otherwise, re-enable gravity
		else
		{
			if (GetComponent<Rigidbody>() != null)
			{
				GetComponent<Rigidbody>().useGravity = true;
			}
		}

		// Assume no collision to begin
		isColliding = false;

		this_collider = GetComponent<CapsuleCollider>();
		foreach (GameObject slot in slots)
		{
			Bounds slotBounds = slot.GetComponent<SphereCollider>().bounds;

			float slotToPiece = Vector3.Distance(slot.transform.position, pieceController.mousePosition);

			bool cursorOutsideSlot = slotToPiece > radius;

			if (this_collider.bounds.Intersects(slotBounds) && !cursorOutsideSlot && isOwned)
			{
				isColliding = true;
				if (Input.GetMouseButton(0))
				{
					colliding_slot = slot;
					Magnetise(null);
				}
			}
		}

		// Piece Placed
		if(!isSelected && isColliding)
		{
			isPlaced = true;
			gameplayManager.PieceJustPlaced = gameObject;
		}
	}

	public void Magnetise(GameObject slot)
	{
		if (slot == null) slot = colliding_slot;
		if (slot.GetComponent<MyMagnetismScript>().WillMagnetise)
		{
			Vector3 slotPosition = slot.transform.position;
			transform.SetPositionAndRotation(slot.transform.position, rotation);
		}
	}

	// Sets variables
	void InitialisePiecePlacer()
	{
		slots = new GameObject[size];

		gameplayManager = GameObject.Find("GameplayManager").GetComponent<MyGameplayManager>();

		pieceController = GameObject.Find("PieceController").GetComponent<MyPieceController>();

		GameObject g = GameObject.Find("Connect_4_Board_Slots");
		Debug.Log(g.gameObject.name);

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
