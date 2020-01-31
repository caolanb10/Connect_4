using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MySpawnManager : MonoBehaviourPunCallbacks
{
	// Parent of gameplay objects
	public GameObject GameplayObjects;

	public GameObject PieceController;

	// GameObjects for their camera's
	public GameObject YellowCamera;
	public GameObject RedCamera;

	// Piece spawn positions for red and yellow
	public GameObject YellowSpawnPositionsParent;
	public GameObject RedSpawnPositionsParent;

	public int NumberOfPieces;

	public bool IsRoomEmpty = true;

	private string YellowPiecePrefabName = "Connect_4_Piece_Yellow";
	private string RedPiecePrefabName = "Connect_4_Piece_Red";

	private GameObject MainCamera;

	private Transform YellowCameraTransform;
	private Transform RedCameraTransform;


	void Start()
    {
		MainCamera = GameObject.Find("MainCamera");
		YellowCameraTransform = GameObject.Find("Board_Objects/Camera_Player_1_Position").transform;
		RedCameraTransform = GameObject.Find("Board_Objects/Camera_Player_2_Position").transform;
	}

	void Update()
	{

    }

	public override void OnJoinedRoom()
	{
		if (PhotonNetwork.IsConnectedAndReady)
		{
			MyARPlacementManager placementManager = GameObject.Find("AR Session Origin").GetComponent<MyARPlacementManager>();

			// Place yellow pieces into the game first
			if ((int) PhotonNetwork.CurrentRoom.PlayerCount == 1)
			{
				Debug.Log("Spawning Yellow  Pieces");
				for(int i = 0; i < NumberOfPieces; i++)
				{
					Vector3 spawnPosition = YellowSpawnPositionsParent.transform.GetChild(i).transform.position;
					GameObject piece = (GameObject) PhotonNetwork.Instantiate(YellowPiecePrefabName, spawnPosition, Quaternion.identity);
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
					Vector3 spawnPosition = RedSpawnPositionsParent.transform.GetChild(i).transform.position;
					GameObject piece = PhotonNetwork.Instantiate(RedPiecePrefabName, spawnPosition, Quaternion.identity);
				}

				// Enable controller and gameplay manager
				GameplayObjects.SetActive(true);
			}
		}
	}
}
