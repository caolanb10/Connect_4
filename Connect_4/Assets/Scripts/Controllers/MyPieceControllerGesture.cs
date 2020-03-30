using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPieceControllerGesture : MyPieceController
{
	GestureInfo GestureInformation;
	TrackingInfo TrackingInformation;

	protected override void FixedUpdate()
	{
		GestureInformation = ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info;
		TrackingInformation = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info;
		ScreenPosition = TrackingInformation.poi;

		Distance = TrackingInformation.depth_estimation * 2;

		if(IsGrabPieceGesture(GestureInformation.mano_gesture_trigger))
		{
			GestureGrab();
		}
		
		if(IsReleaseGesture(GestureInformation.mano_gesture_trigger))
		{
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

	public void AttemptGrab()
	{
		Vector3 orbPos = Camera.ViewportToWorldPoint(new Vector3(ScreenPosition.x, ScreenPosition.y, 1.0f));
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
