using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPiecePlacer : MonoBehaviour
{
	// Number of Slots
	public int size = 7;

	// The Slots
	public GameObject[] slots;

	// The bounds of the slots
	public Collider[] slotsColliders;	
	
	// The parent game object
	private GameObject slotsParent;
	
	// The game object name for the parent
	private string parent_name = "Connect_4_Board_Slots";
	
	// This objects bounds
	public Collider this_collider;

	// Used to determine whether to take control away from the user
	public bool isColliding;

	// Used to determine whether to follow the input of the user
	public bool isSelected;

	// The Slot it is colliding with
	public GameObject colliding_slot;

	void Start()
	{
		GetComponent<Rigidbody>().freezeRotation = true;

		slotsParent = GameObject.Find(parent_name);

		this_collider = GetComponent<CapsuleCollider>();

		int i = 0;
		foreach(Transform t in slotsParent.transform)
		{
			slots[i] = t.gameObject;
			slotsColliders[i] = t.gameObject.GetComponent<SphereCollider>();
			i += 1;
		}
    }

    void FixedUpdate()
    {
		// Assume no collision to begin
		isColliding = false;

		this_collider = GetComponent<CapsuleCollider>();

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
    }
}
