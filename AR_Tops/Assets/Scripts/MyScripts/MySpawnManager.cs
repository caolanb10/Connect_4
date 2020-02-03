using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MySpawnManager : MonoBehaviourPunCallbacks
{
	// Parent of gameplay objects
	public GameObject GameplayObjects;

	public GameObject PieceController;

	public GameObject BoardParent;

	// Piece spawn positions for red and yellow
	public GameObject YellowSpawnPositionsParent;
	public GameObject RedSpawnPositionsParent;

	public int NumberOfPieces;

	public bool IsRoomEmpty = true;

	private string YellowPiecePrefabName = "Connect_4_Piece_Yellow";
	private string RedPiecePrefabName = "Connect_4_Piece_Red";

	public override void OnJoinedRoom()
	{
		if (PhotonNetwork.IsConnectedAndReady)
		{
			MyARPlacementManager placementManager = GameObject.Find("AR Session Origin").GetComponent<MyARPlacementManager>();

			Debug.Log("Rotation of board is " + BoardParent.transform.rotation.ToString());

			// Place yellow pieces into the game first
			if ((int) PhotonNetwork.CurrentRoom.PlayerCount == 1)
			{
				Debug.Log("Spawning Yellow  Pieces");
				for(int i = 0; i < NumberOfPieces; i++)
				{
					Transform YellowSpawnPositionTransform = YellowSpawnPositionsParent.transform.GetChild(i).transform;
					Vector3 spawnPosition = YellowSpawnPositionTransform.position;
					GameObject piece = (GameObject) PhotonNetwork.Instantiate(YellowPiecePrefabName, spawnPosition, Quaternion.identity);
					piece.transform.Rotate(Vector3.up, placementManager.totalAngle);
					Debug.Log(piece.transform.rotation.ToString());
				}

				// Enable controller and gameplay manager
				GameplayObjects.SetActive(true);
			}
			
			// Red pieces are for player 2
			else
			{
				Debug.Log("Spawning Red Pieces Pieces");
				for (int i = 0; i < NumberOfPieces; i++)
				{
					Transform RedSpawnPositionTransform = RedSpawnPositionsParent.transform.GetChild(i).transform;
					Vector3 spawnPosition = RedSpawnPositionTransform.position;
					GameObject piece = PhotonNetwork.Instantiate(RedPiecePrefabName, spawnPosition, Quaternion.identity);
				}

				// Enable controller and gameplay manager
				GameplayObjects.SetActive(true);
			}
		}
	}
}
