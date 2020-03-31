using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyChangeInteraction : MonoBehaviour
{
	public GameObject VisualOrb;
	public GameObject Raycast;
	public GameObject ManoMotion;

	public MyPieceControllerSingleTouch ControllerNormal;
	public MyPieceControllerGesture ControllerGesture;
	public MyPieceControllerPinch ControllerPinch;
	public MyPieceControllerMultiTouch ControllerMulti;
	public MyPieceControllerRaycast ControllerRaycast;

	public void ChangeInteraction(int index)
	{
		if (index == 5)
		{
			DestroyAll();
			return;
		}
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
			VisualOrb.SetActive(true);
			ManoMotion.SetActive(true);
			ControllerGesture.enabled = true;
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
		VisualOrb.SetActive(false);
	}

	public void DestroyAll()
	{
		DisableIfFound("Canvas");
		DisableIfFound("Board_Objects");
		DisableIfFound("NetworkObjects");
		DisableIfFound("GamePlayObjects");

		GameObject[] objs = GameObject.FindGameObjectsWithTag("Piece");
		foreach(GameObject g in objs)
		{
			Destroy(g);
		}
	}

	void DisableIfFound(string gameobject)
	{
		GameObject foundObject = GameObject.Find(gameobject);
		Destroy(foundObject);
		if (foundObject != null)
		{
			Debug.Log("Found" + foundObject.name);
			Destroy(foundObject);
		}
		else
		{
			Debug.Log("Not found" + gameobject);
		}
	}
}
