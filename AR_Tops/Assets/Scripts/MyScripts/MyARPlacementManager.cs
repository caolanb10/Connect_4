using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MyARPlacementManager : MonoBehaviour
{
	ARRaycastManager RayManager;
	static List<ARRaycastHit> RayHits = new List<ARRaycastHit>();
	public Camera ARCamera;
	public GameObject Connect4Board;

	public void Awake()
	{
		RayManager = GetComponent<ARRaycastManager>();
	}
    void Start()
    {
        
    }

    // Update is called once per frame
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
		}
    }
}
