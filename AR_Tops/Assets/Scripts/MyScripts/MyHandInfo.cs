using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyHandInfo : MonoBehaviour
{
	GizmoManager GizmoManager;

	GestureInfo GestureInfo;
	TrackingInfo TrackingInfo;

    void Start()
    {
		GizmoManager = GetComponent<GizmoManager>();
		Session session = ManomotionManager.Instance.Manomotion_Session;
		session.smoothing_controller = 0.7f;
		ManomotionManager.Instance.Manomotion_Session = session;
	}

    void Update()
    {
		/*
		Debug.Log("smoothing factor is " + ManomotionManager.Instance.Manomotion_Session.smoothing_controller);

		GestureInfo = ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info;
		TrackingInfo = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info;

		Debug.Log("POI " + TrackingInfo.poi);
		Debug.Log("Palm center " + TrackingInfo.palm_center);
		Debug.Log("Mano class Grab Gesture" + (GestureInfo.mano_class == ManoClass.GRAB_GESTURE_FAMILY));
		*/
	}
}
