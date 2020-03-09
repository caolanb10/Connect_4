using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MyPieceControllerMultiTouch : MonoBehaviour
{
	[Header("Input")]
	// Players camera object
	public Camera Camera;

	// List of touch objects for discerning unique finger id
	List<Touch> Touches = new List<Touch>();

	// Vector that stores the mouse position in world space to be used by the active piece
	public Vector3 WorldPosition;

	[Header("Game Object Detection and Movement")]
	// Distance away from camera that piece should move to
	private float Distance = 0.4f;

	// Game object list for pieces that we touch
	GameObject[] HighlightedObjects = new GameObject[10];

	bool LeftMouseDown;

	void FixedUpdate()
	{
		bool input = Input.touchCount > 0;

		if (HighlightedObjects[0] != null) ClearGameObjects();

		// Touch or left click down
		if (input)
		{
			int touchCount = Input.touchCount;
			Touches = new List<Touch>();
			Ray[] rays = new Ray[touchCount];

			for (int i = 0; i < touchCount; i++)
			{
				Touches.Add(Input.GetTouch(i));

				WorldPosition = Camera.ScreenToWorldPoint(new Vector3(Touches[i].position.x, Touches[i].position.y, Distance));

				Debug.Log("world position" + WorldPosition);

				RaycastHit hit;
				rays[i] = Camera.ScreenPointToRay(Touches[i].position);

				// If it hits a game object AND we havent previously selected one
				if (Physics.Raycast(rays[i], out hit, Mathf.Infinity, 1 << 8))
				{
					HighlightedObjects[i] = hit.transform.gameObject;

					Debug.Log("Hit gameobject " + HighlightedObjects[i].name);

					// If the game object is a movable piece AND we own the object
					if (HighlightedObjects[i].tag == "Piece" && HighlightedObjects[i].GetComponent<MyPiecePlacer>().IsOwned)
					{
						// Select the object
						HighlightedObjects[i].GetComponent<MyPiecePlacer>().IsSelected = true;
						HighlightedObjects[i].GetComponent<MyPiecePlacer>().TouchPosition = WorldPosition;
					}
				}
			}
		}
	}

	public void ClearGameObjects()
	{
		for (int i = 0; i < HighlightedObjects.Length; i++)
		{
			// If we have selected an object previously
			if (HighlightedObjects[i] != null)
			{
				// If the game object is a movable piece
				if (HighlightedObjects[i].tag == "Piece")
				{
					// De-select the object
					HighlightedObjects[i].GetComponent<MyPiecePlacer>().IsSelected = false;
				}
			}
			// Delete reference
			HighlightedObjects[i] = null;
		}
	}
}
