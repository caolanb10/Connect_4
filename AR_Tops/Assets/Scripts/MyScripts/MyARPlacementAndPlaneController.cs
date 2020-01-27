using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class MyARPlacementAndPlaneController : MonoBehaviour
{

	ARPlaneManager ARPlaneManager;
	MyARPlacementManager MyARPlacementManager;

    // Start is called before the first frame update
    void Awake()
    {
		ARPlaneManager = GetComponent<ARPlaneManager>();
		MyARPlacementManager = GetComponent<MyARPlacementManager>();
	}

    // Update is called once per frame
    void Update()
    {
        
    }
	public void DisableARPlacementAndPlaneDetection()
	{
		ARPlaneManager.enabled = false;
		MyARPlacementManager.enabled = false;

		SetAllPlanesActiveOrDeactive(false);
	}


	public void EnableARPlacementAndPlaneDetection()
	{
		ARPlaneManager.enabled = true;
		MyARPlacementManager.enabled = true;

		SetAllPlanesActiveOrDeactive(true);
	}
	private void SetAllPlanesActiveOrDeactive(bool value)
	{
		foreach(var plane in ARPlaneManager.trackables)
		{
			plane.gameObject.SetActive(value);
		}
	}
}
