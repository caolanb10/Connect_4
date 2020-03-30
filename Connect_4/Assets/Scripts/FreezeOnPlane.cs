using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeOnPlane : MonoBehaviour
{
	private string PlaneName = "Plane";

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.name == PlaneName)
		{
			Rigidbody rb = gameObject.GetComponent<Rigidbody>();
			rb.angularVelocity = new Vector3(0, 0, 0);
			rb.velocity = new Vector3(0, 0, 0);
		}
	}
}
