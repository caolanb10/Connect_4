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
		YellowCameraTransform = GameObject.Find("Board_Objects/Yellow_Camera_Position").transform;
		RedCameraTransform = GameObject.Find("Board_Objects/Red_Camera_Position").transform;
	}

	void Update()
	{

    }

	public override void OnJoinedRoom()
	{
		if (PhotonNetwork.IsConnectedAndReady)
		{
			// Place yellow pieces into the game first
			if ((int) PhotonNetwork.CurrentRoom.PlayerCount == 1)
			{
				Debug.Log("Spawning Yellow  Pieces");
				for(int i = 0; i < NumberOfPieces; i++)
				{
					Vector3 spawnPosition = YellowSpawnPositionsParent.transform.GetChild(i).transform.position;
					PhotonNetwork.Instantiate(YellowPiecePrefabName, spawnPosition, Quaternion.identity);
				}

				// Move Camera to yellow camera position
				MainCamera.transform.SetPositionAndRotation(YellowCameraTransform.position, YellowCameraTransform.rotation);

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
					PhotonNetwork.Instantiate(RedPiecePrefabName, spawnPosition, Quaternion.identity);
				}

				// Move Camera to red camera position
				MainCamera.transform.SetPositionAndRotation(RedCameraTransform.position, RedCameraTransform.rotation);

				// Enable controller and gameplay manager
				GameplayObjects.SetActive(true);
			}
		}
	}
}
