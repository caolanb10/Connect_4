using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPiecePlacer : MonoBehaviour
{
	/* Game Board */
	// Number of Slots
	private float size = 7.0f;

	// The Slots
	private GameObject[] slots;

	// The bounds of the slots
	private Collider[] slotsColliders;

	// The Slot it is colliding with
	private GameObject colliding_slot;

	// Speed that the object should move at
	private float Speed = 20.0f;

	// The controller
	private MyPieceController pieceController;

	// Used to preserve rotation
	private Quaternion rotation = Quaternion.Euler(90, 0, 0);
	
	// This objects bounds
	private Collider this_collider;

	// Used to determine whether to take control away from the user
	private bool isColliding;

	// Used to determine whether to follow the input of the user
	public bool isSelected;

	void Start()
	{
		pieceController = GameObject.Find("PieceController").GetComponent<MyPieceController>();

		// Preserve rotation
		transform.rotation = rotation;
		GetComponent<Rigidbody>().freezeRotation = true;

		this_collider = GetComponent<CapsuleCollider>();

		GameObject g = GameObject.Find("Connect_4_Board_Slots");

		Debug.Log(g.name);
		Debug.Log(g.transform.childCount);
		Debug.Log(g.transform.GetChild(0).gameObject.name);

		for(int i = 0; i < g.transform.childCount; i++)
		{
			slots[i] = g.transform.GetChild(i).gameObject;
			slotsColliders[i] = slots[i].GetComponent<SphereCollider>();
		}
    }

    void FixedUpdate()
    {
		// Move toward mouse if selected
		if(isSelected)
		{
			GetComponent<Rigidbody>().useGravity = false;
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

		if(this_collider.bounds.Intersects(slotsColliders[0].bounds) && Input.GetMouseButton(0))
		{
			isColliding = true;
			colliding_slot = slotsColliders[0].gameObject;

			Debug.Log("Colliding with" + slotsColliders[0].gameObject.name);
			Debug.Log("Colliding is" + isColliding);

			bool magnet = colliding_slot.GetComponent<MyMagnetismScript>().WillMagnetise;
			if (magnet)
			{
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
}
