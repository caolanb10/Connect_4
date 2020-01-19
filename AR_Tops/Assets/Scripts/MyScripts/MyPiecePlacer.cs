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
	
	// The Slot it is colliding with
	public GameObject colliding_slot;

	void Start()
	{
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

    void Update()
    {
		this_collider = GetComponent<CapsuleCollider>();

		foreach (Collider c in slotsColliders)
		{
			if (this_collider.bounds.Intersects(c.bounds))
			{
				isColliding = true;
				colliding_slot = c.gameObject;
			}
			else
			{
				isColliding = false;
				colliding_slot = null;
			}
		}
    }
}
