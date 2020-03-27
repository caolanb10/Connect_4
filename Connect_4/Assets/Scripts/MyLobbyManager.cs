using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class MyLobbyManager : MonoBehaviourPunCallbacks
{
    [Header("Login UI")]
    public InputField playerNameInputField;
    public GameObject UILoginGameObject;

    [Header("Lobby UI")]
    public GameObject UILobby;

    [Header("Connection Status UI")]
    public GameObject UIConnectionStatus;
    public Text connectionStatusText;
    public bool showConnectionStatus = false;

    #region UNITY METHODS
    void Start()
    {
		// Go to Lobby
		if (PhotonNetwork.IsConnected)
        {
			UILobby.SetActive(true);
			UIConnectionStatus.SetActive(false);
			UILoginGameObject.SetActive(false);
        }
		// Go to Login
		else
		{
			UILobby.SetActive(false);
			UIConnectionStatus.SetActive(false);
			UILoginGameObject.SetActive(true);
        }
    }

    void Update()
    {
        if (showConnectionStatus)
        {
            connectionStatusText.text = "Connection Status: " + PhotonNetwork.NetworkClientState;
        }
    }
    #endregion

    #region UI Callback Methods
    public void OnEnterGameButtonClicked()
    {
        string playerName = playerNameInputField.text;

        if (!string.IsNullOrEmpty(playerName))
        {
			UILobby.SetActive(false);
			UIConnectionStatus.SetActive(true);
            showConnectionStatus = true;
			UILoginGameObject.SetActive(false);
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.LocalPlayer.NickName = playerName;
                PhotonNetwork.ConnectUsingSettings();
            }
        }
        else
        {
            Debug.Log("Player name is invalid or empty");
        }
    }

    public void OnQuickMatchButtonClicked()
    {
        SceneLoader.Instance.LoadMyScene("Scene_Gameplay");
    }

	#endregion

	#region PHOTON Callback Methods
	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		foreach(RoomInfo room in roomList)
		{
			Debug.Log(room.Name);
		}
	}

	public override void OnConnected()
    {
        Debug.Log("We connected to the internet");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " is connected to Photon");
		PhotonNetwork.JoinLobby();
		UILobby.SetActive(true);
		UIConnectionStatus.SetActive(false);
		UILoginGameObject.SetActive(false);
    }
    #endregion
}
