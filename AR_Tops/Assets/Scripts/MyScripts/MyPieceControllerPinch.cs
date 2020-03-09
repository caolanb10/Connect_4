using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MyPieceControllerPinch : MonoBehaviour
{
	[Header("Input")]
	// Players camera object
	public Camera Camera;

	// Vector that stores the mouse position in world space to be used by the active piece
	public Vector3 WorldPosition;

	// Vector for the position of the second finger touching the screen, used for the pinching gesture.
	public Vector3 SecondTouchPosition;

	// TODO: Need to find a reasonable value for this.
	private float DistanceBetweenTouches = 0f;

	[Header("Game Object Detection and Movement")]
	// Same as initial value for Distance
	private static float DefaultDistance = 0.4f;

	// Distance away from camera that piece should move to
	private float Distance = DefaultDistance;

	// Gameobject that mouse ray has intersected with
	private GameObject HighlightedObject;

	public float Speed = 0.00025f;

	public bool EnableDesktopControls;

	bool LeftMouseDown;

	void FixedUpdate()
	{
		bool input = EnableDesktopControls
			? Input.GetMouseButton(0)
			: Input.touchCount > 0;

		// Left Click Down or touch
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
				if (Input.touchCount == 2)
				{
					SecondTouchPosition = Input.GetTouch(1).position;

					float previousDistanceBetweenTouches = DistanceBetweenTouches != 0f
						? DistanceBetweenTouches
						: Vector3.Distance(screenPosition, SecondTouchPosition);
					DistanceBetweenTouches = Vector3.Distance(screenPosition, SecondTouchPosition);
					Distance += (DistanceBetweenTouches - previousDistanceBetweenTouches) * 0.1f * Speed;
				}
				else
				{
					DistanceBetweenTouches = 0f;
				}
			}

			Ray ray = Camera.ScreenPointToRay(screenPosition);
			RaycastHit hit;

			// Point in world space
			WorldPosition = Camera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, Distance));

			// If it hits a game object AND we havent previously selected one
			if (Physics.Raycast(ray, out hit) && HighlightedObject == null)
			{
				HighlightedObject = hit.transform.gameObject;
				Debug.Log("Has hit game object " + HighlightedObject.name);

				// If the game object is a movable piece AND we own the object
				if (HighlightedObject.tag == "Piece" && HighlightedObject.GetComponent<MyPiecePlacer>().IsOwned)
				{
					Debug.Log("Touch has hit a piece");
					// Select the object
					HighlightedObject.GetComponent<MyPiecePlacer>().IsSelected = true;
					HighlightedObject.GetComponent<MyPiecePlacer>().TouchPosition = WorldPosition;
				}
			}
		}

		// No touch
		else
		{
			Distance = DefaultDistance;
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
}
