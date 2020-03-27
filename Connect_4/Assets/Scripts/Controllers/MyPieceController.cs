using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MyPieceController : MonoBehaviour
{
	// Players camera object
	public Camera Camera;

	public Vector2 ScreenPosition;

	// Vector that stores the mouse position in world space to be used by the active piece
	public Vector3 WorldPosition;

	// Distance away from camera that piece should move to
	public static float Distance = 0.4f;

	// Gameobject that mouse ray has intersected with
	public GameObject SelectedPiece;
	public MyPiecePlacer SelectedPlacer;

	protected virtual void Start()
	{
		InitialiseGameObjects();
	}

	protected virtual void FixedUpdate()
	{
		if (SelectedPiece == null)
			return;
		else
		{
			UpdatePositionForPiece();
			SelectedPlacer.TouchPosition = WorldPosition;
		}
	}

	protected virtual void GrabScreenPoint()
	{
		// Already have a piece
		if (SelectedPiece != null)
			return;
		else
		{
			Debug.Log("Called Grab at " + ScreenPosition);
			Ray ray = Camera.ScreenPointToRay(ScreenPosition);
			Grab(ray);
		}
	}

	protected virtual void GrabViewportPoint()
	{
		// Already have a piece
		if (SelectedPiece != null)
			return;
		else
		{
			Debug.Log("Called Grab at " + ScreenPosition);
			Ray ray = Camera.ViewportPointToRay(ScreenPosition);
			Grab(ray);
		}
	}

	protected virtual void Grab(Ray ray)
	{
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << 8))
		{
			SelectedPiece = hit.transform.gameObject;
			SelectedPlacer = SelectedPiece.GetComponent<MyPiecePlacer>();

			Debug.Log("Has hit game object " + SelectedPiece.name);

			if (SelectedPlacer.IsOwned)
			{
				Debug.Log("Touch has hit an owned piece");
				SelectedPlacer.IsSelected = true;
			}
		}
		else
		{
			Debug.Log("Hit nothing");
		}
	}

	public void UpdatePositionForPiece()
	{
		WorldPosition = Camera.ScreenToWorldPoint(new Vector3(ScreenPosition.x, ScreenPosition.y, Distance));
	}

	public void Release()
	{
		// Not grabbing a piece
		if (SelectedPiece == null) return;

		Debug.Log("Called Release");

		SelectedPlacer.IsSelected = false;
		SelectedPlacer = null;
		SelectedPiece = null;
	}

	public void InitialiseGameObjects()
	{
		Camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
	}
}
