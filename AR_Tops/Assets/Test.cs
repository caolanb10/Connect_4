using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Test : MonoBehaviour
{
	ARSessionOrigin Origin;
    
    void Awake()
    {
		Origin = GetComponent<ARSessionOrigin>();
    }

    void Update()
    {
		Debug.Log(Origin.camera.transform.position);
    }

	// Doesn't work
	public void MoveCamera()
	{
		Origin.camera.transform.position += new Vector3(0, 0, 0.5f);
	}
}
