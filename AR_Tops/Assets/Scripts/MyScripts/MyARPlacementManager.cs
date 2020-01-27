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
    void Update()
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
				DebugObject.GetComponent<TextMeshProUGUI>().text =
					"X: " + Connect4Board.transform.position.x +
					"Y: " + Connect4Board.transform.position.y +
					"Z: " + Connect4Board.transform.position.z;
			}
		}
    }
}
