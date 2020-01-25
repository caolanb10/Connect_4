using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MyPieceSynchronisation : MonoBehaviour, IPunObservable
{
	Rigidbody Rb;
	PhotonView PhotonView;

	MyPiecePlacer Placer;

	Vector3 NetworkPosition;
	Quaternion NetworkRotation;

	private float Angle;
	private float Distance;

	private bool SendRigidBody = true;

	public bool SynchronizeVelocity = true;

	public bool IsTeleportEnabled = true;
	public float TeleportIfDistanceIsGreater = 1.0f;



	public void Awake()
	{
		Rb = GetComponent<Rigidbody>();
		PhotonView = GetComponent<PhotonView>();
		Placer = GetComponent<MyPiecePlacer>();
	}

    void Update()
    {
        SendRigidBody = !Placer.isInPosition;
    }

	void FixedUpdate()
	{
		// I have no control over these pieces so retrieve network position and rotation
		if (!PhotonView.IsMine)
		{
			Rb.position = Vector3.MoveTowards(Rb.position, NetworkPosition, Distance * (1.0f / PhotonNetwork.SerializationRate));
			Rb.rotation = Quaternion.RotateTowards(Rb.rotation, NetworkRotation, Angle * (1.0f / PhotonNetwork.SerializationRate));
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		// I have control over the pieces, so I can move them and then send pos + rot over network 
		if (stream.IsWriting)
		{
			// If the piece isn't already placed into the board
			if (SendRigidBody)
			{
				stream.SendNext(Rb.position);
				stream.SendNext(Rb.rotation);

				// Send Velocity data as well
				if (SynchronizeVelocity)
				{
					stream.SendNext(Rb.velocity);
				}
			}
		}

		// No control over the object, so I update this class with the values read from the network
		// if (stream.IsReading)
		else
		{
			// If the piece isn't already placed into the board
			if (SendRigidBody)
			{ 
				NetworkPosition = (Vector3)stream.ReceiveNext();
				NetworkRotation = (Quaternion)stream.ReceiveNext();

				if (IsTeleportEnabled)
				{
					if (Vector3.Distance(Rb.position, NetworkPosition) > TeleportIfDistanceIsGreater)
					{
						Rb.position = NetworkPosition;
					}
				}
				if (SynchronizeVelocity)
				{
					float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));

					if (SynchronizeVelocity)
					{
						Rb.velocity = (Vector3)stream.ReceiveNext();

						NetworkPosition += Rb.velocity * lag;

						Distance = Vector3.Distance(Rb.position, NetworkPosition);
					}
				}
			}
		}
	}
}
