﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

public class MyARPlacementManager : MonoBehaviour
{
	ARRaycastManager RayManager;

	float speed = 1.0f;
	static List<ARRaycastHit> RayHits = new List<ARRaycastHit>();
	public Camera ARCamera;

	public GameObject Connect4Board;

	public GameObject DebugObject;
	public bool DebugEnabled;

	private void Awake()
	{
		RayManager = GetComponent<ARRaycastManager>();
	}
    void Start()
    {
        
    }
    void FixedUpdate()
    {
		Vector3 centerOfScreen = new Vector3(Screen.width / 2, Screen.height / 2);
		Ray ray = ARCamera.ScreenPointToRay(centerOfScreen); 
		
		if(RayManager.Raycast(ray, RayHits, TrackableType.PlaneWithinPolygon))
		{ 
			// First intersection
			Pose hitPose = RayHits[0].pose;
			Vector3 positionToBePlaced = hitPose.position;
			Connect4Board.transform.position = positionToBePlaced;

			if (DebugEnabled)
			{
				ShowDebug();
			}
		}
    }

	// ARCore session origin for this
	public void Rotate(bool increase)
	{
		float angle = increase ? speed * 20 : - (speed * 20);
		gameObject.transform.Rotate(Vector3.up, angle);
	}

	public void Scale(bool increase)
	{
		float multiplier = 1f;
		float magnitude = !increase ? speed * multiplier : - (speed * multiplier);
		
		Vector3 boardScaleFactor = new Vector3(magnitude, magnitude, magnitude);

		gameObject.transform.localScale += boardScaleFactor;
	}

	void ShowDebug()
	{
		DebugObject.GetComponent<TextMeshProUGUI>().text =
		"X: " + Connect4Board.transform.position.x +
		"Y: " + Connect4Board.transform.position.y +
		"Z: " + Connect4Board.transform.position.z;
	}
}
