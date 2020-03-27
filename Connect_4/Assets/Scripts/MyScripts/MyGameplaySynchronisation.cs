﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class MyGameplaySynchronisation : MonoBehaviourPunCallbacks
{
	MyGameplayManager GameplayManager;

	enum RaiseEventCodes
	{
		UpdatePositions = 1
	}

	void Start()
    {
		GameplayManager = GetComponent<MyGameplayManager>();
		PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
	}

	private void OnDestroy()
	{
		PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
	}

	public void SendPositionData(int posH, int posW)
	{
		object[] data = new object[]
		{
			posH,
			posW,
		};

		RaiseEventOptions raiseEventOptions = new RaiseEventOptions
		{
			Receivers = ReceiverGroup.Others,
			CachingOption = EventCaching.AddToRoomCache,
		};

		SendOptions sendOptions = new SendOptions
		{ Reliability = true };

		PhotonNetwork.RaiseEvent(
			(byte)RaiseEventCodes.UpdatePositions, 
			data,
			raiseEventOptions, 
			sendOptions);
	}

	void OnEvent(EventData photonEvent)
	{
		if(photonEvent.Code == (byte) RaiseEventCodes.UpdatePositions)
		{
			object[] data = (object[])photonEvent.CustomData;

			int posH = (int)data[0];

			// Compensate for mirroring
			int posW = Mathf.Abs((int)data[1] - 6);

			if (GameplayManager.MyColour == MyPlayerColour.Yellow)
			{
				GameplayManager.IsOccupiedRed[posH, posW] = true;
				GameplayManager.HandleGameOver(MyPlayerColour.Red);
			}
			else
			{
				GameplayManager.IsOccupiedYellow[posH, posW] = true;
				GameplayManager.HandleGameOver(MyPlayerColour.Yellow);
			}
		}
	}
}
