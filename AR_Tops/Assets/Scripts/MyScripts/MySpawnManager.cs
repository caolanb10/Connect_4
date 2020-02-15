using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class MySpawnManager : MonoBehaviourPunCallbacks
{
	// Parent of gameplay objects
	public GameObject GameplayObjects;

	// Piece spawn positions for red and yellow
	public GameObject SpawnPositionsParent;

	public GameObject[] Positions;

	public GameObject Board;

	public GameObject YellowPlayer;

	public GameObject RedPlayer;

	public int NumberOfPieces;
	
	bool IsFirstPlayer;

	enum RaiseEventCodes
	{
		PlayerSpawnEventCode = 0
	}

	void Start()
	{
		PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
	}

	private void OnDestroy()
	{
		PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
	}

	void OnEvent(EventData photonEvent)
	{
		if(photonEvent.Code == (byte)RaiseEventCodes.PlayerSpawnEventCode)
		{
			object[] data = (object[])photonEvent.CustomData;
			Vector3 receivedPosition = (Vector3)data[0];
			Quaternion receivedRotation = (Quaternion)data[1];
			int receivedViewID = (int)data[2];
			bool receievedFirstPlayer = (bool)data[3];

			Debug.Log(receivedPosition);

			GameObject player = receievedFirstPlayer
				? Instantiate(YellowPlayer, 
				MyFlippedCoordinates.FlipPerspectiveOfBoardPiece(receivedPosition, Board), receivedRotation)
				: Instantiate(RedPlayer, 
				MyFlippedCoordinates.FlipPerspectiveOfBoardPiece(receivedPosition, Board), receivedRotation);

			PhotonView view = player.GetComponent<PhotonView>();
			view.ViewID = receivedViewID;
		}
	}

	public override void OnJoinedRoom()
	{
		IsFirstPlayer = ((int) PhotonNetwork.CurrentRoom.PlayerCount == 1);
	
		if (PhotonNetwork.IsConnectedAndReady)
		{
			for(int i = 0; i < NumberOfPieces; i++)
			{
				SpawnPlayer(i);
			}
			GameplayObjects.SetActive(true);
		}
	}

	#region Private Methods
	private void SpawnPlayer(int i)
	{
		Debug.Log("Are the first player: " + IsFirstPlayer);

		GameObject piece = IsFirstPlayer
			? Instantiate(YellowPlayer, Positions[i].transform.position, Quaternion.identity)
			: Instantiate(RedPlayer, Positions[i].transform.position, Quaternion.identity);

		PhotonView view = piece.GetComponent<PhotonView>();

		if (PhotonNetwork.AllocateViewID(view))
		{
			object[] data = new object[]
			{
				MyFlippedCoordinates.PositionRelativeToBoard(piece.transform.position, Board),
				piece.transform.rotation,
				view.ViewID,
				IsFirstPlayer
			};

			RaiseEventOptions raiseEventOptions = new RaiseEventOptions
			{
				Receivers = ReceiverGroup.Others,
				CachingOption = EventCaching.AddToRoomCache,
			};

			SendOptions sendOptions = new SendOptions
			{
				Reliability = true
			};

			PhotonNetwork.RaiseEvent((byte)RaiseEventCodes.PlayerSpawnEventCode, data, raiseEventOptions, sendOptions);
		}
	}
	#endregion
}
