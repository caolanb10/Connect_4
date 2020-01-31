using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class MyARPlacementAndPlaneController : MonoBehaviour
{
	ARPlaneManager PlaneManager;
	MyARPlacementManager MyARPlacementManager;

    // Start is called before the first frame update
    private void Awake()
    {
		PlaneManager = GetComponent<ARPlaneManager>();
		MyARPlacementManager = GetComponent<MyARPlacementManager>();
	}
	public void DisableARPlacementAndPlaneDetection()
	{
		PlaneManager.enabled = false;
		MyARPlacementManager.enabled = false;
		// SetAllPlanesActiveOrDeactive(false);
	}
	public void EnableARPlacementAndPlaneDetection()
	{
		PlaneManager.enabled = true;
		MyARPlacementManager.enabled = true;
		SetAllPlanesActiveOrDeactive(true);
	}
	private void SetAllPlanesActiveOrDeactive(bool value)
	{
		foreach(var plane in PlaneManager.trackables)
		{
			plane.gameObject.SetActive(value);
		}
	}
}
