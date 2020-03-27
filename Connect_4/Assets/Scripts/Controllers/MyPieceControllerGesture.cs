using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPieceControllerGesture : MyPieceController
{
	GestureInfo GestureInformation;
	TrackingInfo TrackingInformation;

	// Continuous gestures
	// Trigger
	// Poi
	// Palm Centre
	// Mano Class

	protected override void FixedUpdate()
	{
		GestureInformation = ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info;
		TrackingInformation = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info;
		ScreenPosition = TrackingInformation.poi;

		Debug.Log(TrackingInformation.depth_estimation);

		Distance = TrackingInformation.depth_estimation;

		if(IsGrabPieceGesture(GestureInformation.mano_gesture_trigger))
		{
			Debug.Log("Grab at: " + ScreenPosition);

			GrabViewportPoint();
		}
		
		if(IsReleaseGesture(GestureInformation.mano_gesture_trigger))
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
