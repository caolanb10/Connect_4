using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MyPieceController : MonoBehaviour
{
	[Header("Input")]
	// Players camera object
	public Camera Camera;

	// Vector that stores the mouse position in world space to be used by the active piece
	public Vector3 mousePosition;

	[Header("Game Object Detection and Movement")]
	// Distance away from camera that piece should move to
	private float Distance = 0.4f;

	// Gameobject that mouse ray has intersected with
	private GameObject highlightedObject;

	bool LeftMouseDown;

	public bool EnableDesktopControls;

	void FixedUpdate()
	{

		bool input = EnableDesktopControls
			? Input.GetMouseButton(0)
			: Input.touchCount > 0;

		// Left Click Down
		if (input)
		{
			Touch touch;
			Vector3 screenPosition;
			if (Input.touchCount > 0)
			{
				touch = Input.GetTouch(0);
				screenPosition = touch.position;
			}
			else
			{
				screenPosition = Input.mousePosition;
			}

			Ray ray = Camera.ScreenPointToRay(screenPosition);
			RaycastHit hit;

			// Point in world space
			mousePosition = Camera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, Distance));

			// If it hits a game object AND we havent previously selected one
			if (Physics.Raycast(ray, out hit) && highlightedObject == null)
			{
				Transform selection = hit.transform;
				highlightedObject = selection.gameObject;
				Debug.Log("Has hit game object " + highlightedObject.name);

				// If the game object is a movable piece AND we own the object
				if (highlightedObject.tag == "Piece" && highlightedObject.GetComponent<MyPiecePlacer>().IsOwned)
				{
					Debug.Log("Touch has hit a piece");
					// Select the object
					highlightedObject.GetComponent<MyPiecePlacer>().IsSelected = true;
				}
			}
		}

		// No touch
		else
		{
			// If we have selected an object previously
			if (highlightedObject != null)
			{
				// If the game object is a movable piece
				if (highlightedObject.tag == "Piece")
				{
					// De-select the object
					highlightedObject.GetComponent<MyPiecePlacer>().IsSelected = false;
				}
			}
			// Delete reference
			highlightedObject = null;
		}
	}
}
