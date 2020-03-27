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
}
