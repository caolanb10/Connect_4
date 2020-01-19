﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPieceController : MonoBehaviour
{
    private Vector3 mousePosition;
	private GameObject highlightedObject;
	public float Speed = 5.0f;
	public Camera cam;
	public float Distance = 20.0f;

    void Start()
    {
        
    }

	void Update()
	{
		Vector3 mousePos = Input.mousePosition;
		if (Input.GetMouseButton(0))
		{
			Ray ray = cam.ScreenPointToRay(mousePos);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				Transform selection = hit.transform;
				highlightedObject = selection.gameObject;
				if(highlightedObject.tag == "Piece")
				{
					mousePos.z = Distance;
					Vector3 point = cam.ScreenToWorldPoint(mousePos);
					Quaternion currentRotation = highlightedObject.transform.rotation;
					highlightedObject.transform.SetPositionAndRotation(point, currentRotation);
				}
			}
		}
	}
}