using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MyPieceSynchronisation : MonoBehaviour, IPunObservable
{
	Rigidbody Rb;

	PhotonView PhotonView;

	MyPiecePlacer Placer;

	// Both the same
	Vector3 NetworkPositionRb;

	Vector3 NetworkPositionT;

	bool NetworkIsKinematic;

	private float Distance;

	public bool SynchronizeVelocity = true;

	public bool IsTeleportEnabled = true;

	public float TeleportIfDistanceIsGreater = 1.0f;

	public void Awake()
	{
		Rb = GetComponent<Rigidbody>();
		PhotonView = GetComponent<PhotonView>();
		Placer = GetComponent<MyPiecePlacer>();
		Debug.Log("name: " + gameObject.name);
	}

	void FixedUpdate()
	{
		// I have no control over these pieces so retrieve network position and rotation
		if (!PhotonView.IsMine)
		{

			Debug.Log("Network position rb : " + NetworkPositionRb);
			Debug.Log("Network is kinematic : " + NetworkIsKinematic);

			transform.position = Vector3.MoveTowards(Rb.position, NetworkPositionRb, Distance * (1.0f / PhotonNetwork.SerializationRate));
			Rb.isKinematic = NetworkIsKinematic;
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		// I have control over the pieces, so I can move them and then send pos + rot over network
		if (stream.IsWriting)
		{
			stream.SendNext(Rb.position);
			stream.SendNext(Rb.isKinematic);
			stream.SendNext(transform.position);

			if (Placer.IsSelected)
			{
				Debug.Log(Rb.position);
				Debug.Log(Rb.isKinematic);
				Debug.Log(transform.position);
			}

			// Send Velocity data as well
			if (SynchronizeVelocity)
			{
				stream.SendNext(Rb.velocity);
			}
		}

		// No control over the object, so I update this class with the values read from the network
		// if (stream.IsReading)
		else
		{
			NetworkPositionRb = (Vector3)stream.ReceiveNext();
			NetworkIsKinematic = (bool)stream.ReceiveNext();
			NetworkPositionT = (Vector3)stream.ReceiveNext();

			if (IsTeleportEnabled)
			{
				if (Vector3.Distance(Rb.position, NetworkPositionRb) > TeleportIfDistanceIsGreater)
				{
					Rb.position = NetworkPositionRb;
				}
			}
			if (SynchronizeVelocity)
			{
				float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));

				if (SynchronizeVelocity)
				{
					Rb.velocity = (Vector3)stream.ReceiveNext();

					NetworkPositionRb += Rb.velocity * lag;

					Distance = Vector3.Distance(Rb.position, NetworkPositionRb);
				}
			}
		}
	}
}
