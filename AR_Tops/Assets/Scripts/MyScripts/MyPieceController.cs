using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MyPieceController : MonoBehaviour
{
	[Header("Input")]
	// Players camera object
	public Camera AR_Camera;

	// Vector that stores the mouse position in world space to be used by the active piece
	public Vector3 mousePosition;

	[Header("Game Object Detection and Movement")]
	// Distance away from camera that piece should move to
	private float Distance = 0.4f;

	// Gameobject that mouse ray has intersected with
	private GameObject highlightedObject;

	ARRaycastManager RaycastManager;
	static List<ARRaycastHit> Rayhits = new List<ARRaycastHit>();

	void FixedUpdate()
	{
		// Vector3 mousePos = Input.mousePosition;
		// Left Click Down
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);

			Vector3 touchPos = touch.position;

			Ray ray = AR_Camera.ScreenPointToRay(touchPos);
			
			RaycastHit hit;

			// Point in world space
			mousePosition = AR_Camera.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, Distance));

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
