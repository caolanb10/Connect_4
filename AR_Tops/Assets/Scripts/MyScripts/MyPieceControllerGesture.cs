using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPieceControllerGesture : MonoBehaviour
{
	public void PrintInfo(ManoGestureTrigger trigger, TrackingInfo trackingInfo)
	{
		Vector3 poi = trackingInfo.poi;
		Vector3 palm_center = trackingInfo.palm_center;

		Debug.Log("poi" + poi);
		Debug.Log("palm center" + palm_center);

		if(trigger == ManoGestureTrigger.CLICK)
		{
			Debug.Log("Received click gesture");
		}
		if (trigger == ManoGestureTrigger.DROP)
		{
			Debug.Log("Received drop gesture");
		}
		if (trigger == ManoGestureTrigger.GRAB_GESTURE)
		{
			Debug.Log("Received grab gesture");
		}
		if (trigger == ManoGestureTrigger.NO_GESTURE)
		{
			Debug.Log("Received no gesture gesture");
		}
		if (trigger == ManoGestureTrigger.PICK)
		{
			Debug.Log("Received pick gesture");
		}
		if (trigger == ManoGestureTrigger.RELEASE_GESTURE)
		{
			Debug.Log("Received release gesture");
		}
	}
}
