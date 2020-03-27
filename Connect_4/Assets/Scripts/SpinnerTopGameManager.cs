using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;


public class SpinnerTopGameManager : MonoBehaviourPunCallbacks
{
    [Header("Room Setup")]
    public byte maxPlayers;

    [Header("UI Inform")]
    public GameObject UI_Inform_Panel;
    public TextMeshProUGUI UI_Inform_Text;
    public GameObject SearchForGamesButton;


    [Header("String Constants")]
    private string notSearching = "Search for Games to Battle";
    private string searching = "Searching for random room ....";

    // Start is called before the first frame update
    void Start()
    {
        UI_Inform_Panel.SetActive(true);
        UI_Inform_Text.text = notSearching;
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region UI Callbacks
    public void JoinRandomRoom()
    {
        UI_Inform_Text.text = searching;
        PhotonNetwork.JoinRandomRoom();
        SearchForGamesButton.SetActive(false);
    }
    #endregion

    #region Photon Callback Methods
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        UI_Inform_Text.text = message;
        CreateAndJoinRoom();
    }

    // Called for local player joining a room when they join a room
    public override void OnJoinedRoom()
    {
        string UI_String_Game_Not_Full= "Joined to " + PhotonNetwork.CurrentRoom.Name + " .Waiting for other players... ";
        string UI_String_Game_Full= "Joined to " + PhotonNetwork.CurrentRoom.Name + " Game is now full";

        if (PhotonNetwork.CurrentRoom.PlayerCount < maxPlayers)
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

    // Called for local player when another player joins their room
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name + " Player count " + PhotonNetwork.CurrentRoom.PlayerCount);
        if(PhotonNetwork.CurrentRoom.PlayerCount >= maxPlayers)
        {
            UI_Inform_Text.text = "New player called " + newPlayer + " has joined the room. Game starting";
            StartCoroutine(DeactivateAfterSeconds(UI_Inform_Panel, 2.0f));
        }
        else
        {
            UI_Inform_Text.text = "New player called " + newPlayer + " has joined the room. Waiting for more players";
        }
    }
    #endregion

    #region Private Methods
    // Called when joining a random room fails
    private void CreateAndJoinRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayers;

        string randomRoomName = "Room" + Random.Range(0, 10000);
        PhotonNetwork.CreateRoom(randomRoomName, roomOptions);
    }

    // Co routine for deactivating UI after a set number of seconds
    IEnumerator DeactivateAfterSeconds(GameObject gameObj, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObj.SetActive(false);
    }
    #endregion
}
