using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyChangeInteraction : MonoBehaviour
{
	public GameObject Raycast;
	public GameObject ManoMotion;
	public MyPieceControllerSingleTouch ControllerNormal;
	public MyPieceControllerPinch ControllerPinch;
	public MyPieceControllerRaycast ControllerRaycast;
	public MyPieceControllerMultiTouch ControllerMulti;

	public void ChangeInteraction(int index)
	{
		DisableOthers();
		if(index == 0)
		{
			ControllerNormal.enabled = true;
		}
		if (index == 1)
		{
			ControllerPinch.enabled = true;
		}
		if (index == 2)
		{
			ControllerMulti.enabled = true;
		}
		if(index == 3)
		{
			ManoMotion.SetActive(true);
		}
		if(index == 4)
		{
			ControllerRaycast.enabled = true;
			Raycast.SetActive(true);
		}
	}
	void DisableOthers()
	{
		ControllerNormal.enabled = false;
		ControllerMulti.enabled = false;
		ControllerPinch.enabled = false;
		ControllerRaycast.enabled = false;
		Raycast.SetActive(false);
		ManoMotion.SetActive(false);
	}

	void DisableIfFound(string gameobject)
	{
		GameObject foundObject = GameObject.Find(gameobject);
		if (foundObject != null)
		{
			Debug.Log("Found");
			foundObject.SetActive(false);
		}
		else
		{
			Debug.Log("Not found");
		}
	}
}
