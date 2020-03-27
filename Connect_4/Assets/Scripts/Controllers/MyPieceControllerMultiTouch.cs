using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MyPieceControllerMultiTouch : MyPieceController
{
	List<Touch> Touches = new List<Touch>();

	GameObject[] HighlightedObjects = new GameObject[4];

	protected override void FixedUpdate()
	{
		if (HighlightedObjects[0] != null) ClearGameObjects();

		if (Input.touchCount > 0)
		{
			int touchCount = Input.touchCount;

			Touches = new List<Touch>();

			Ray[] rays = new Ray[touchCount];

			for (int i = 0; i < touchCount; i++)
			{
				Touches.Add(Input.GetTouch(i));

				WorldPosition = Camera.ScreenToWorldPoint(new Vector3(Touches[i].position.x, Touches[i].position.y, Distance));

				RaycastHit hit;
				rays[i] = Camera.ScreenPointToRay(Touches[i].position);

				if (Physics.Raycast(rays[i], out hit, Mathf.Infinity, 1 << 8))
				{
					HighlightedObjects[i] = hit.transform.gameObject;

					if (HighlightedObjects[i].GetComponent<MyPiecePlacer>().IsOwned)
					{
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
			if (HighlightedObjects[i] != null)
			{
				HighlightedObjects[i].GetComponent<MyPiecePlacer>().IsSelected = false;
			}
			HighlightedObjects[i] = null;
		}
	}
}
