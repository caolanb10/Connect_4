using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MyGameplaySynchronisation : MonoBehaviour, IPunObservable
{
	GameObject[,] BoardPositions;
	bool[,] IsOccupied;
	PhotonView PhotonView;

	GameObject[,] NetworkBoardPositions;
	bool[,] NetworkIsOccupied;

	void Start()
    {
		BoardPositions = GetComponent<MyGameplayManager>().BoardPositions;
		IsOccupied = GetComponent<MyGameplayManager>().IsOccupied;
		PhotonView = GetComponent<PhotonView>();
    }

    void FixedUpdate()
    {
        if(!PhotonView.IsMine)
		{
			BoardPositions = NetworkBoardPositions;
			IsOccupied = NetworkIsOccupied;
		}
    }

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{

		// Owned, writing to stream
		if(stream.IsWriting)
		{
			stream.SendNext(BoardPositions);
			stream.SendNext(IsOccupied);
		}

		// Not owned, reading from opponents stream
		else
		{
			NetworkBoardPositions = (GameObject[,])stream.ReceiveNext();
			NetworkIsOccupied = (bool[,])stream.ReceiveNext();
		}
	}
}
