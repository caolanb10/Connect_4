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
	public GameObject YellowSpawnPositionsParent;
	public GameObject RedSpawnPositionsParent;

	public int NumberOfPieces;

	public override void OnJoinedRoom()
	{
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
		// Spawn yellow pieces for first player, red pieces for second player
		bool isFirstPlayer = (int)PhotonNetwork.CurrentRoom.PlayerCount == 1;

		Debug.Log("Are the first player: " + isFirstPlayer);

		Vector3 spawnPosition = isFirstPlayer
			? YellowSpawnPositionsParent.transform.GetChild(i).transform.position
			: RedSpawnPositionsParent.transform.GetChild(i).transform.position;

		GameObject piece = isFirstPlayer
			? (GameObject)PhotonNetwork.Instantiate("Connect_4_Piece_Yellow", spawnPosition, Quaternion.identity)
			: (GameObject)PhotonNetwork.Instantiate("Connect_4_Piece_Red", spawnPosition, Quaternion.identity);
	}

	#endregion
}
