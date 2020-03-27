using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [Header("Login UI")]
    public InputField playerNameInputField;
    public GameObject uI_LoginGameObject;

    [Header("Lobby UI")]
    public GameObject uI_Lobby;
    public GameObject uI_3Dobj;

    [Header("Connection Status UI")]
    public GameObject uI_ConnectionStatus;
    public Text connectionStatusText;
    public bool showConnectionStatus = false;


    #region UNITY methods
    // Start is called before the first frame update
    void Start()
    {

        if (PhotonNetwork.IsConnected)
        {
            // Go to Lobby
            uI_Lobby.SetActive(true);
            uI_3Dobj.SetActive(true);
            uI_ConnectionStatus.SetActive(false);
            uI_LoginGameObject.SetActive(false);
        }
        else
        {
            // Go to Login
            uI_Lobby.SetActive(false);
            uI_3Dobj.SetActive(false);
            uI_ConnectionStatus.SetActive(false);
            uI_LoginGameObject.SetActive(true);
        }
    }

    // Update is called once per frame
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
            uI_Lobby.SetActive(false);
            uI_3Dobj.SetActive(false);
            uI_ConnectionStatus.SetActive(true);
            showConnectionStatus = true;
            uI_LoginGameObject.SetActive(false);
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

    public void onQuickMatchButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Scene_PLayerSelection");
    }

    #endregion

    #region PHOTON Callback Methods

    public override void OnConnected()
    {
        Debug.Log("We connected to the internet");
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + "is connected to Photon");
        uI_Lobby.SetActive(true);
        uI_3Dobj.SetActive(true);
        uI_ConnectionStatus.SetActive(false);

        uI_LoginGameObject.SetActive(false);
    }

    #endregion

}
