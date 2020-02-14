﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MyPieceSynchronisation : MonoBehaviour, IPunObservable
{
	Rigidbody Rb;

	PhotonView PhotonView;

	MyPiecePlacer Placer;

	GameObject Board;

	// Network variables
	Vector3 NetworkPositionRb;

	bool NetworkIsKinematic;

	Quaternion NetworkRotation;

	private float Distance;

	public bool SynchronizeVelocity = true;

	public bool IsTeleportEnabled = true;

	public float TeleportIfDistanceIsGreater = 1.0f;

	public void Awake()
	{
		Board = GameObject.Find("Board_Objects");
		Rb = GetComponent<Rigidbody>();
		PhotonView = GetComponent<PhotonView>();
		Placer = GetComponent<MyPiecePlacer>();
	}

	void FixedUpdate()
	{
		// I have no control over these pieces so retrieve network position and rotation
		// Need to add offset for other side of the board
		if (!PhotonView.IsMine)
		{
			transform.position = Vector3.MoveTowards(Rb.position, NetworkPositionRb, Distance * (1.0f / PhotonNetwork.SerializationRate));
			Rb.isKinematic = NetworkIsKinematic;
			Rb.rotation = NetworkRotation;
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		// I have control over the pieces, so I can move them and then send pos + rot over network
		if (stream.IsWriting)
		{
			Vector3 positionRelativeToBoard = Rb.position - Board.transform.position;

			// Debug.Log("Yellow" + positionRelativeToBoard);

			stream.SendNext(positionRelativeToBoard);
			stream.SendNext(Rb.isKinematic);
			stream.SendNext(Rb.rotation);

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
			Vector3 pos = (Vector3)stream.ReceiveNext();
			// Debug.Log("Red" + pos);
			// Need to add offset
			NetworkPositionRb = pos + Board.transform.position;

			NetworkIsKinematic = (bool)stream.ReceiveNext();
			NetworkRotation = (Quaternion)stream.ReceiveNext();

			if (IsTeleportEnabled)
			{
				if (Vector3.Distance(Rb.position, NetworkPositionRb) > TeleportIfDistanceIsGreater)
				{
					Rb.position = NetworkPositionRb + Board.transform.position;
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
