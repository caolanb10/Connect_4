using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

public class MyARPlacementManager : MonoBehaviour
{
	ARRaycastManager RayManager;
	ARTrackedImageManager ImageManager;
	ARPlaneManager PlaneManager;

	float Speed = 1.0f;
	public float TotalAngle = 0f;

	static List<ARRaycastHit> RayHits = new List<ARRaycastHit>();
	public Camera ARCamera;

	public GameObject Connect4Board;

	private GameObject[] MarkerObject;

	public bool MarkerBased;

	private void Awake()
	{
		MarkerObject = new GameObject[1];
		PlaneManager = GetComponent<ARPlaneManager>();
		RayManager = GetComponent<ARRaycastManager>();
		ImageManager = GetComponent<ARTrackedImageManager>();
	}

    void FixedUpdate()
    {
		// Markerless
		if (!MarkerBased)
		{
			Vector3 centerOfScreen = new Vector3(Screen.width / 2, Screen.height / 2);
			Ray ray = ARCamera.ScreenPointToRay(centerOfScreen);

			if (RayManager.Raycast(ray, RayHits, TrackableType.PlaneWithinPolygon))
			{
				// First intersection
				Pose hitPose = RayHits[0].pose;
				Vector3 positionToBePlaced = hitPose.position;
				Connect4Board.transform.position = positionToBePlaced;
			}
		}
		// Marker based
		// ImageManager.enabled == true
		else
		{
			FindMarkerObject();
			if (MarkerObject.Length > 0)
			{
				Debug.Log("Marker Object found");
				Connect4Board.transform.position = MarkerObject[0].transform.position;
			}
			else
			{
				Debug.Log("Marker Object not found");
			}
		}
    }

	public void FindMarkerObject()
	{
		MarkerObject = GameObject.FindGameObjectsWithTag("ARImage");
	}

	public void Rotate(bool increase)
	{
		float angle = increase ? Speed * 20 : - (Speed * 20);
		Connect4Board.transform.Rotate(Vector3.up, angle);
		TotalAngle += angle;
	}

	// AR Session Origin for this
	public void Scale(bool increase)
	{
		float multiplier = 0.1f;
		float magnitude = !increase ? Speed * multiplier : - (Speed * multiplier);
		
		Vector3 boardScaleFactor = new Vector3(magnitude, magnitude, magnitude);

		gameObject.transform.localScale += boardScaleFactor;
	}

	public void SetMarkerBased(bool x) => MarkerBased = x;
}
