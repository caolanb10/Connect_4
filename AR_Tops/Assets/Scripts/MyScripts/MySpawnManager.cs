using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MySpawnManager : MonoBehaviourPunCallbacks
{
	// Parent of gameplay objects
	public GameObject GameplayObjects;

	public GameObject BoardParent;

	// Piece spawn positions for red and yellow
	public GameObject SpawnPositionsParent;

	public int NumberOfPieces;

	bool IsFirstPlayer;

	public override void OnJoinedRoom()
	{
		IsFirstPlayer = (int) PhotonNetwork.CurrentRoom.PlayerCount == 1;

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

		Vector3 spawnPosition = SpawnPositionsParent.transform.GetChild(i).transform.position;

		GameObject piece = IsFirstPlayer
			? (GameObject)PhotonNetwork.Instantiate("Connect_4_Piece_Yellow", spawnPosition, Quaternion.identity)
			: (GameObject)PhotonNetwork.Instantiate("Connect_4_Piece_Red", spawnPosition, Quaternion.identity);
	}
	#endregion
}
