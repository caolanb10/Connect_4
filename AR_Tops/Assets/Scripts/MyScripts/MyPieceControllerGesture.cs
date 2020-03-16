using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPieceControllerGesture : MonoBehaviour
{
	float Distance = 0.4f;

	public void PrintInfo(GestureInfo info, TrackingInfo trackingInfo)
	{
		ManoGestureTrigger trigger = info.mano_gesture_trigger;
		ManoGestureContinuous continuous = info.mano_gesture_continuous;

		Vector3 poiRectTransform = Camera.main.ViewportToWorldPoint(new Vector3(trackingInfo.poi.x, trackingInfo.poi.y, Distance));

		// Ray ray = Camera.ScreenPointToRay(screenPosition);
		RaycastHit hit;


		Vector3 poi = trackingInfo.poi;
		Vector3 palm_center = trackingInfo.palm_center;

		Debug.Log("poi" + poi);
		Debug.Log("palm center" + palm_center);
		Debug.Log("continuous " + continuous);

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
