using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPieceController : MonoBehaviour
{
    private Vector3 mousePosition;
	private GameObject highlightedObject;
	private Quaternion rotation;
	public float Speed = 5.0f;
	public Camera cam;
	public float Distance = 20.0f;
	public Vector3 zeroVelocity = new Vector3(0, 0, 0);
	public MyPiecePlacer placer;
	public bool LeftMouseDown = false;
	public Rigidbody highlightedObjectRb;

	void Start()
    {
		// Set absolute rotation
		rotation = Quaternion.Euler(90, 0, 0);
    }

	void FixedUpdate()
	{
		Vector3 mousePos = Input.mousePosition;

		LeftMouseDown = Input.GetMouseButton(0);
  
		Debug.Log(LeftMouseDown);

		// Left Click
		if (LeftMouseDown)
		{
			Ray ray = cam.ScreenPointToRay(mousePos);
			RaycastHit hit;

			// If it hits a game object
			if (Physics.Raycast(ray, out hit))
			{
				Transform selection = hit.transform;
				highlightedObject = selection.gameObject;

				// If the game object is a movable piece
				if (highlightedObject.tag == "Piece")
				{

					placer = highlightedObject.GetComponent<MyPiecePlacer>();
					highlightedObject.GetComponent<Rigidbody>().useGravity = false;

					placer.isSelected = true;

					mousePos.z = Distance;

					// Point in world space
					Vector3 point = cam.ScreenToWorldPoint(mousePos);

					if (placer.isColliding == false)
					{
						// Move object to mouse point
						highlightedObject.transform.position
							= Vector3.MoveTowards(highlightedObject.transform.position, point, Speed * Time.deltaTime);

						// Apply no velocity
						// highlightedObject.GetComponent<Rigidbody>().velocity = zeroVelocity;
					}
				}
			}
		}

		// No left click, clear highlighted object
		else
		{
			if (highlightedObject != null)
			{
				highlightedObject.GetComponent<Rigidbody>().useGravity = true;
			}
			highlightedObject = null;
		}
	}
}