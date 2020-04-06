using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyManoSetup : MonoBehaviour
{
    void Start()
    {
		Session session = ManomotionManager.Instance.Manomotion_Session;
		session.smoothing_controller = 1.0f;
		session.gesture_smoothing_controller = 0.5f;
		ManomotionManager.Instance.Manomotion_Session = session;
	}
}
