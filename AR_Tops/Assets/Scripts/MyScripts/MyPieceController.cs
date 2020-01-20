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

    void Start()
    {
		// Set absolute rotation
		rotation = Quaternion.Euler(90, 0, 0);
    }

	void FixedUpdate()
	{
		Vector3 mousePos = Input.mousePosition;

		// Left Click
		if (Input.GetMouseButton(0))
		{
			Ray ray = cam.ScreenPointToRay(mousePos);
			RaycastHit hit;

			// If it hits a game object
			if (Physics.Raycast(ray, out hit))
			{
				Transform selection = hit.transform;
				highlightedObject = selection.gameObject;

				// If the game object is a movable piece
				if(highlightedObject.tag == "Piece")
				{
					mousePos.z = Distance;
					Vector3 point = cam.ScreenToWorldPoint(mousePos);

					//Move object to mouse point
					highlightedObject.transform.SetPositionAndRotation(point, rotation);
				}
			}
		}
	}
}