using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class MyGameplayManager : MonoBehaviour
{
	MyGameplaySynchronisation gameplaySynchronisation;

	public GameObject UI_Inform_Panel;
	public TextMeshProUGUI UI_Inform_Text;
	public MyUIManager UIManager;

	// Game board Positions
	public GameObject[,] BoardPositions;
	public bool[,] IsOccupiedYellow;
	public bool[,] IsOccupiedRed;

	// Top slots to place pieces
	private GameObject[] TopSlots;

	public GameObject PieceJustPlaced = null;
	public string PieceJustPlacedColour = null;

	public int SlotPlaced;

	public GameObject Slots;
	public GameObject Positions;

	// Board Dimensions
	private int height = 6;
	private int width = 7;

	public string MyColour;

	void Start()
	{
		gameplaySynchronisation = GetComponent<MyGameplaySynchronisation>();
		InitializeBoard();
		InitialiseTopSlots();
	}

	void FixedUpdate()
	{
		if (PieceJustPlaced != null)
		{
			CheckForCollision();
		}
	}

	public void CheckForCollision()
	{
		Bounds piecePlaced = PieceJustPlaced.GetComponent<CapsuleCollider>().bounds;

		// Find the index of the slot that it was dropped in
		string slotName = PieceJustPlaced.GetComponent<MyPiecePlacer>().Colliding_slot.gameObject.name;
		SlotPlaced = Int32.Parse(slotName.Substring(slotName.Length - 1, 1));

		GameObject position = GetAvailablePosition(SlotPlaced);

		Debug.Log("Checking for collision with " + position.transform.parent.gameObject.name + position.gameObject.name);

		Bounds positionBounds = position.GetComponent<SphereCollider>().bounds;

		// Collision on board
		if (piecePlaced.Intersects(positionBounds))
		{
			PlacePiece(position);
		}
	}

	public void PlacePiece(GameObject boardPosition)
	{
		Rigidbody rb = PieceJustPlaced.GetComponent<Rigidbody>();

		rb.transform.position = boardPosition.transform.position;
		rb.transform.rotation = boardPosition.transform.rotation;

		rb.useGravity = false;
		rb.isKinematic = true;

		int positionH = boardPosition.GetComponent<MyMagnetismScript>().PositionH;
		int positionW = boardPosition.GetComponent<MyMagnetismScript>().PositionW;

		// Placed in board
		PieceJustPlaced.GetComponent<MyPiecePlacer>().IsInPosition = true;

		// Update board state and state of board positions objects
		if (PieceJustPlacedColour == MyPlayerColour.Yellow) IsOccupiedYellow[positionH, positionW] = true;
		else IsOccupiedRed[positionH, positionW] = true;

		// Piece has been placed, delete reference
		PieceJustPlaced = null;
		PieceJustPlacedColour = null;

		// Send board data over the network to be synchronised
		gameplaySynchronisation.SendPositionData(positionH, positionW);
		HandleGameOver(MyColour);
	}

	public GameObject GetAvailablePosition(int index)
	{
		for (int i = 0; i < height; i++)
		{
			if (!IsOccupiedYellow[i, index] && !IsOccupiedRed[i, index])
				return (BoardPositions[i, index]);
		}
		return null;
	}

	public void HandleGameOver(string colour)
	{
		if(IsGameOver(colour))
		{
			UI_Inform_Text.text = "Game over " + colour + " won the game";
			UI_Inform_Panel.SetActive(true);
			UIManager.StateGameOver();
		}
	}

	public void HandleBackToLobby()
	{
		SceneLoader.Instance.LoadMyScene("My_Scene_Lobby");
	}

	public bool IsGameOver(string colour)
	{
		bool[,] IsOccupied = colour == MyPlayerColour.Yellow ? IsOccupiedYellow : IsOccupiedRed;
		// For each row check horizontal
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j <= width - 4; j++)
			{
				if (IsOccupied[i, j] && IsOccupied[i, j + 1] && IsOccupied[i, j + 2] && IsOccupied[i, j + 3])
				{
					return true;
				}
			}
		}

		// For each row check vertically
		for (int j1 = 0; j1 < width; j1++)
		{
			for (int i1 = 0; i1 <= height - 4; i1++)
			{
				if (IsOccupied[i1, j1] && IsOccupied[i1 + 1, j1] && IsOccupied[i1 + 2, j1] && IsOccupied[i1 + 3, j1])
				{
					return true;
				}
			}
		}

		// Check direction: /
		for (int j2 = 0; j2 < width - 4; j2++)
		{
			for (int i2 = 0; i2 < height - 4; i2++)
			{
				if (IsOccupied[i2, j2] && IsOccupied[i2 + 1, j2 + 1] && IsOccupied[i2 + 2, j2 + 2] && IsOccupied[i2 + 3, j2 + 3])
				{
					return true;
				}
			}
		}

		// Check direction: \
		for (int j3 = 0; j3 < width - 4; j3++)
		{
			for(int i3 = height - 1; i3 > height - 4; i3--)
			{
				if (IsOccupied[i3, j3] && IsOccupied[i3 - 1, j3 - 1] && IsOccupied[i3 - 2, j3 - 2] && IsOccupied[i3 - 3, j3 - 3])
				{
					return true;
				}
			}
		}

		Debug.Log("Game Not Over Yet !!!" + colour + " hasn't won the game");
		return false;
	}

	public void InitializeBoard()
	{
		BoardPositions = new GameObject[height, width];
		IsOccupiedRed = new bool[height, width];
		IsOccupiedYellow = new bool[height, width];

		// Initialize the positions of the board with their game objects and set the positions to free (true)
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				BoardPositions[i, j] = Positions.transform.GetChild(i).gameObject.transform.GetChild(j).gameObject;
				BoardPositions[i, j].GetComponent<MyMagnetismScript>().Initialise(true, i, j);

				IsOccupiedRed[i, j] = false;
				IsOccupiedYellow[i, j] = false;
			}
		}
	}
	public void InitialiseTopSlots()
	{
		TopSlots = new GameObject[width];
		for (int i = 0; i < TopSlots.Length; i++)
		{
			TopSlots[i] = Slots.transform.GetChild(i).gameObject;
		}
	}
}
