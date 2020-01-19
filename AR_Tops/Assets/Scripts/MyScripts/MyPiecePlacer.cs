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
	public Bounds[] slotsBounds;	
	
	// The parent game object
	private GameObject slotsParent;
	
	// The game object name for the parent
	private string parent_name = "Connect_4_Board_Slots";
	
	// This objects bounds
	private Mesh this_mesh;

	// This objects bounds
	public Bounds this_bounds;

	// Test Bounds
	public Bounds first_bound;

	// First slot
	public GameObject first_slot;


	void Start()
	{
		slotsParent = GameObject.Find(parent_name);

		this_mesh = GetComponent<MeshFilter>().mesh;

		this_bounds = this_mesh.bounds;

		int i = 0;
		foreach(Transform t in slotsParent.transform)
		{
			slots[i++] = t.gameObject;
			// slotsBounds[i++] = t.gameObject.transform.GetChild(0).gameObject.GetComponent<MeshFilter>().mesh.bounds;
		}
		first_slot = slotsParent.transform.GetChild(0).GetChild(0).gameObject;
		first_bound = first_slot.GetComponent<MeshFilter>().mesh.bounds;
    }

    void Update()
    {
		this_mesh = GetComponent<MeshFilter>().mesh;

		this_bounds = this_mesh.bounds;

		if (this_bounds.Intersects(first_bound))
		{
			Debug.Log("Intersecting");
		}
    }
}
