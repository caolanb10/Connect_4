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
	public Vector3 WorldPosition;

	[Header("Game Object Detection and Movement")]
	// Distance away from camera that piece should move to
	private float Distance = 0.4f;

	// Gameobject that mouse ray has intersected with
	private GameObject HighlightedObject;

	bool LeftMouseDown;

	public bool EnableDesktopControls;

	void FixedUpdate()
	{

		bool input = EnableDesktopControls
			? Input.GetMouseButton(0)
			: Input.touchCount > 0;

		// Touch or left click down
		if (input)
		{
			Vector3 screenPosition;
			if (EnableDesktopControls)
			{
				screenPosition = Input.mousePosition;
			}
			else
			{
				screenPosition = Input.GetTouch(0).position;
			}

			GrabPiece(screenPosition);
		}

		// No touch
		else
		{
			ClearPiece();
		}
	}

	public void GrabPiece(Vector3 screenPosition)
	{
		Ray ray = Camera.ScreenPointToRay(screenPosition);
		RaycastHit hit;

		// Point in world space
		WorldPosition = Camera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, Distance));

		if (HighlightedObject != null)
			HighlightedObject.GetComponent<MyPiecePlacer>().TouchPosition = WorldPosition;

		// If it hits a game object AND we havent previously selected one
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 8))
		{
			HighlightedObject = hit.transform.gameObject;
			Debug.Log("Has hit game object " + HighlightedObject.name);

			// If the game object is a movable piece AND we own the object
			if (HighlightedObject.tag == "Piece" && HighlightedObject.GetComponent<MyPiecePlacer>().IsOwned)
			{
				Debug.Log("Touch has hit a piece");
				HighlightedObject.GetComponent<MyPiecePlacer>().IsSelected = true;
			}
		}
	}

	public void ClearPiece()
	{
		// If we have selected an object previously
		if (HighlightedObject != null)
		{
			// If the game object is a movable piece
			if (HighlightedObject.tag == "Piece")
			{
				// De-select the object
				HighlightedObject.GetComponent<MyPiecePlacer>().IsSelected = false;
			}
		}
		// Delete reference
		HighlightedObject = null;
	}
}
