﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyChangeInteraction : MonoBehaviour
{ 
	public MyPieceController ControllerNormal;
	public MyPieceControllerPinch ControllerPinch;
	public MyPieceControllerMultiTouch ControllerMulti;
	public MyPieceControllerGesture ControllerGesture;

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
			ControllerGesture.enabled = true;
		}
	}
	void DisableOthers()
	{
		ControllerNormal.enabled = false;
		ControllerMulti.enabled = false;
		ControllerPinch.enabled = false;
		ControllerGesture.enabled = false;
	}
}
