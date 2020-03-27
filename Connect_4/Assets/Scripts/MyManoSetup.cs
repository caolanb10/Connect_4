using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyManoSetup : MonoBehaviour
{
	GizmoManager GizmoManager;
    void Start()
    {
		GizmoManager = GetComponent<GizmoManager>();
		Session session = ManomotionManager.Instance.Manomotion_Session;
		session.smoothing_controller = 0.7f;
		ManomotionManager.Instance.Manomotion_Session = session;
	}
}
