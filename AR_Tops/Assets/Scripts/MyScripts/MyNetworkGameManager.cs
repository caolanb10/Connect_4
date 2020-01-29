using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class MyNetworkGameManager : MonoBehaviourPunCallbacks
{
	[Header("Room Setup")]
	public byte MaxPlayers;

	[Header("UI Inform")]
	public GameObject UI_Inform_Panel;
	public TextMeshProUGUI UI_Inform_Text;
	public GameObject SearchForGamesButton;

	[Header("String Constants")]

	private string searching = "Searching for room ....";

	void Start()
	{
		UI_Inform_Panel.SetActive(true);
	}

	private void CreateAndJoinRoom()
	{
		// Create new options object and set max players to be equal to 2
		RoomOptions roomOptions = new RoomOptions();
		roomOptions.MaxPlayers = MaxPlayers;

		// Create room with random integer ID
		string randomRoomName = "Room " + Random.Range(0, 1000);
		PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
	}

	#region UI Callbacks
	public void JoinRandomRoom()
	{
		UI_Inform_Text.text = searching;
		PhotonNetwork.JoinRandomRoom();
		SearchForGamesButton.SetActive(false);
	}
	#endregion

	#region Photon Callback Mehtods

	// No game found, create and join a room
	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		UI_Inform_Text.text = message;
		CreateAndJoinRoom();
	}

	// Called for a local player when they are joining a room
	public override void OnJoinedRoom()
	{
		string UI_String_Game_Not_Full = "Joined to " + PhotonNetwork.CurrentRoom.Name
			+ ". Waiting for other players to join";
		string UI_String_Game_Full = "Joined to " + PhotonNetwork.CurrentRoom.Name
			+ " Game is now full.";

		if (PhotonNetwork.CurrentRoom.PlayerCount < MaxPlayers)
		{
			UI_Inform_Text.text = UI_String_Game_Not_Full;
			Debug.Log(UI_String_Game_Not_Full);
		}
		else
		{
			UI_Inform_Text.text = UI_String_Game_Full;
			Debug.Log(UI_String_Game_Full);
			StartCoroutine(DeactivateAfterSeconds(UI_Inform_Panel, 2.0f));
		}
	}

	// Called for a local player when another player joins a room
	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		Debug.Log(newPlayer.NickName + " has just joined to " + PhotonNetwork.CurrentRoom.Name + " Player Count is " + PhotonNetwork.CurrentRoom.PlayerCount);
		if (PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayers)
		{
			UI_Inform_Text.text = "New Player called " + newPlayer.NickName + " has just joined the room. Game Starting";
			StartCoroutine(DeactivateAfterSeconds(UI_Inform_Panel, 2.0f));
		}
	}
	#endregion

	IEnumerator DeactivateAfterSeconds(GameObject gameObj, float seconds)
	{
		yield return new WaitForSeconds(seconds);
		gameObj.SetActive(false);
	}
}
