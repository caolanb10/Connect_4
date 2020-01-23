using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MySpawnManager : MonoBehaviourPunCallbacks
{

	public GameObject Player1Camera;
	public GameObject Player2Camera;

	public int NumberOfPieces;

	public bool IsRoomEmpty = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public override void OnJoinedRoom()
	{
		if (PhotonNetwork.IsConnectedAndReady)
		{
			if (IsRoomEmpty)
			{

				// Place yellow pieces into the game first

				PhotonNetwork.Instantiate();
			}
		}
	}
}
