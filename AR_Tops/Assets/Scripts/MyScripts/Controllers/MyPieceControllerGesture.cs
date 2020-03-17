﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPieceControllerGesture : MyPieceController
{
	GestureInfo GestureInfo;
	TrackingInfo TrackingInfo;
	
	// Continuous gestures
	// Trigger
	// Poi
	// Palm Centre
	// Mano Class

	protected override void FixedUpdate()
	{
		GestureInfo = ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info;
		TrackingInfo = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info;

		ScreenPosition = TrackingInfo.poi;

		if(IsGrabPieceGesture(GestureInfo.mano_gesture_trigger))
		{
			Debug.Log("Grab at: " + ScreenPosition);

			GrabViewportPoint();
		}
		
		if(IsReleaseGesture(GestureInfo.mano_gesture_trigger))
		{
			Debug.Log("Release at: " + ScreenPosition);

			Release();
		}
		if (SelectedPiece == null)
			return;
		else
		{
			UpdateScreenPosition();
			SelectedPlacer.TouchPosition = WorldPosition;
		}
	}

	public void UpdateScreenPosition()
	{
		WorldPosition = Camera.ViewportToWorldPoint(new Vector3(ScreenPosition.x, ScreenPosition.y, Distance));
	}

	public bool IsGrabPieceGesture(ManoGestureTrigger trigger)
	{
		return ((trigger == ManoGestureTrigger.CLICK)
			|| (trigger == ManoGestureTrigger.GRAB_GESTURE)
			|| (trigger == ManoGestureTrigger.PICK));
	}

	public bool IsReleaseGesture(ManoGestureTrigger trigger)
	{
		return ((trigger == ManoGestureTrigger.DROP)
			|| (trigger == ManoGestureTrigger.RELEASE_GESTURE));
	}
}