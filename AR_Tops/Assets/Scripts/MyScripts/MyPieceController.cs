using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPieceController : MonoBehaviour
{
	[Header("Input")]
	// Players camera object
	public Camera cam;

	// Vector that stores the mouse position in world space to be used by the active piece
	public Vector3 mousePosition;

	[Header("Game Object Detection and Movement")]
	// Distance away from camera that piece should move to
	public float Distance = 20.0f;

	// Left click state
	private bool LeftMouseDown = false;

	// Gameobject that mouse ray has intersected with
	private GameObject highlightedObject;

	void Start()
    {

    }

	void FixedUpdate()
	{
		Vector3 mousePos = Input.mousePosition;

		LeftMouseDown = Input.GetMouseButton(0);

		// Left Click Down
		if (LeftMouseDown)
		{
			Ray ray = cam.ScreenPointToRay(mousePos);
			RaycastHit hit;

			// Point in world space
			mousePosition = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Distance));

			// If it hits a game object AND we havent previously selected one
			if (Physics.Raycast(ray, out hit) && highlightedObject == null)
			{
				Transform selection = hit.transform;
				highlightedObject = selection.gameObject;

				// If the game object is a movable piece AND we own the object
				if (highlightedObject.tag == "Piece" && highlightedObject.GetComponent<MyPiecePlacer>().isOwned)
				{
					// Select the object
					highlightedObject.GetComponent<MyPiecePlacer>().isSelected = true;
				}
			}
		}

		// Left Click Up
		else
		{
			// If we have selected an object previously
			if (highlightedObject != null)
			{
				// If the game object is a movable piece
				if (highlightedObject.tag == "Piece")
				{
					// De-select the object
					highlightedObject.GetComponent<MyPiecePlacer>().isSelected = false;
				}
			}
			// Delete reference
			highlightedObject = null;
		}
	}
}
