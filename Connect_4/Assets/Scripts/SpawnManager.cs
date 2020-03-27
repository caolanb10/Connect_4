using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnManager : MonoBehaviourPunCallbacks
{

    public GameObject[] Spinners;
    public Transform[] SpawnPositions;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Photon Callback Methods
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            object playerSelection;
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerSpinnerTopGame.PLAYER_SELECTION_NUMBER, out playerSelection))
            {
                Debug.Log("Player selection is " + (int) playerSelection);

                // Pick random start point for new player to spawn at
                int randomStartPoint = Random.Range(0, SpawnPositions.Length - 1);
                
                // Get position from the array of spawn points
                Vector3 instantiatePosition = SpawnPositions[randomStartPoint].position;

                // Instantiate them into the game.
                PhotonNetwork.Instantiate(Spinners[(int)playerSelection].name, instantiatePosition, Quaternion.identity);

            }
        }
    }
    #endregion
}
