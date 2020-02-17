using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class MySpawnManager : MonoBehaviourPunCallbacks
{
	public MyGameplayManager GameplayManager;

	public MyARPlacementManager PlacementManager;

	public MyUIManager UI_Manager;

	// Parent of gameplay objects
	public GameObject GameplayObjects;

	public GameObject[] Positions;

	public GameObject Board;

	public GameObject YellowPlayer;

	public GameObject RedPlayer;

	public int NumberOfPieces;
	
	public bool IsFirstPlayer;

	Quaternion PieceRotation;

	Vector3 DefaultPieceRotation = new Vector3(90.0f, 0.0f, 0.0f);

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

	public void SpawnPieces()
	{
		Vector3 rotationVector = DefaultPieceRotation
			+ new Vector3(0.0f, PlacementManager.totalAngle, 0.0f);

		PieceRotation = Quaternion.Euler(rotationVector);
		if (PhotonNetwork.IsConnectedAndReady)
		{
			for(int i = 0; i < NumberOfPieces; i++)
			{
				SpawnPlayer(i);
			}
			GameplayObjects.SetActive(true);
			GameplayManager.MyColour = IsFirstPlayer
				? MyPlayerColour.Yellow
				: MyPlayerColour.Red;
		}
		UI_Manager.IsInGame = true;
		UI_Manager.StateInGame();
	}

	#region Private Methods
	void SpawnPlayer(int i)
	{
		Debug.Log("Are the first player: " + IsFirstPlayer);

		GameObject piece = IsFirstPlayer
			? Instantiate(YellowPlayer, Positions[i].transform.position, PieceRotation)
			: Instantiate(RedPlayer, Positions[i].transform.position, PieceRotation);

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

	void OnEvent(EventData photonEvent)
	{
		if (photonEvent.Code == (byte)RaiseEventCodes.PlayerSpawnEventCode)
		{
			object[] data = (object[])photonEvent.CustomData;

			Vector3 receivedPosition = (Vector3)data[0];
			Quaternion receivedRotation = (Quaternion)data[1];
			int receivedViewID = (int)data[2];
			bool receievedFirstPlayer = (bool)data[3];

			GameObject player = receievedFirstPlayer
				? Instantiate(YellowPlayer,
				MyFlippedCoordinates.FlipPerspectiveOfBoardPiece(receivedPosition, Board), receivedRotation)
				: Instantiate(RedPlayer,
				MyFlippedCoordinates.FlipPerspectiveOfBoardPiece(receivedPosition, Board), receivedRotation);

			PhotonView view = player.GetComponent<PhotonView>();
			view.ViewID = receivedViewID;
		}
	}
	#endregion
}