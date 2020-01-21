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

	// The bounds of the slots
	private Bounds[] slotsColliders;

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
		if(isSelected && !isColliding)
		{
			GetComponent<Rigidbody>().useGravity = false;
			GetComponent<Rigidbody>().velocity = ZeroSpeed;
			transform.position = 
				Vector3.MoveTowards(transform.position, pieceController.mousePosition, Speed * Time.deltaTime);
		}
		else
		{
			GetComponent<Rigidbody>().useGravity = true;
		}

		// Assume no collision to begin
		isColliding = false;

		this_collider = GetComponent<CapsuleCollider>();
		Debug.Log(pieceController.mousePosition);


		float distanceFromSlotToMouse = Vector3.Distance(slots[0].transform.position, pieceController.mousePosition);

		bool cursorOutside = distanceFromSlotToMouse > radius;

		Debug.Log("cursor outside" + cursorOutside);
		Debug.Log("distance from slot to cursor" + distanceFromSlotToMouse);

		if (this_collider.bounds.Intersects(slotsColliders[0])
			&& Input.GetMouseButton(0)
			&& !cursorOutside)
		{
			isColliding = true;
			colliding_slot = slots[0].gameObject;
			if (colliding_slot.GetComponent<MyMagnetismScript>().WillMagnetise)
			{ 
				Vector3 slotPosition = colliding_slot.transform.position;
				transform.SetPositionAndRotation(colliding_slot.transform.position, rotation);
			}
		}
		else
		{
			colliding_slot = null;
		}
	}
		/*
		foreach (Collider c in slotsColliders)
		{
			if (this_collider.bounds.Intersects(c.bounds) && Input.GetMouseButton(0))
			{
				isColliding = true;
				colliding_slot = c.gameObject;

				Debug.Log("Colliding with" + c.gameObject.name);
				Debug.Log("Colliding is" + isColliding);

				bool magnet = colliding_slot.GetComponent<MyMagnetismScript>().WillMagnetise;
				if (magnet)
				{
					transform.SetPositionAndRotation(colliding_slot.transform.position, Quaternion.Euler(90, 0, 0));
				}
			}
			else
			{
				colliding_slot = null;
			}
		}
		*/

	// Sets variables
	void InitialisePiecePlacer()
	{
		slots = new GameObject[size];
		slotsColliders = new Bounds[size];

		pieceController = GameObject.Find("PieceController").GetComponent<MyPieceController>();
		GameObject g = GameObject.Find("Connect_4_Board_Slots");

		radius = gameObject.transform.localScale.x / 2;

		transform.rotation = rotation;

		GetComponent<Rigidbody>().freezeRotation = true;

		this_collider = GetComponent<CapsuleCollider>();
		
		for (int i = 0; i < g.transform.childCount; i++)
		{
			slots[i] = g.transform.GetChild(i).gameObject;
			slotsColliders[i] = slots[i].GetComponent<SphereCollider>().bounds;
		}
	}
}
