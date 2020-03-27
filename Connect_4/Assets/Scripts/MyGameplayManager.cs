using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class MyGameplayManager : MonoBehaviour
{
	MyGameplaySynchronisation GameplaySynchronisation;

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
	private int Height = 6;
	private int Width = 7;

	public string MyColour;

	void Start()
	{
		GameplaySynchronisation = GetComponent<MyGameplaySynchronisation>();
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
		string slotName = PieceJustPlaced.GetComponent<MyPiecePlacer>().CollidingSlot.gameObject.name;
		SlotPlaced = Int32.Parse(slotName.Substring(slotName.Length - 1, 1));

		GameObject position = GetAvailablePosition(SlotPlaced);

		Bounds positionBounds = position.GetComponent<SphereCollider>().bounds;

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
		GameplaySynchronisation.SendPositionData(positionH, positionW);
		HandleGameOver(MyColour);
	}

	public GameObject GetAvailablePosition(int index)
	{
		for (int i = 0; i < Height; i++)
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
		SceneLoader.Instance.LoadMyScene("Scene_Lobby");
	}

	public bool IsGameOver(string colour)
	{
		bool[,] IsOccupied = colour == MyPlayerColour.Yellow ? IsOccupiedYellow : IsOccupiedRed;
		for (int i = 0; i < Height; i++)
		{
			for (int j = 0; j <= Width - 4; j++)
			{
				if (IsOccupied[i, j] && IsOccupied[i, j + 1] && IsOccupied[i, j + 2] && IsOccupied[i, j + 3])
				{
					return true;
				}
			}
		}

		for (int j1 = 0; j1 < Width; j1++)
		{
			for (int i1 = 0; i1 <= Height - 4; i1++)
			{
				if (IsOccupied[i1, j1] && IsOccupied[i1 + 1, j1] && IsOccupied[i1 + 2, j1] && IsOccupied[i1 + 3, j1])
				{
					return true;
				}
			}
		}

		for (int j2 = 0; j2 < Width - 4; j2++)
		{
			for (int i2 = 0; i2 < Height - 4; i2++)
			{
				if (IsOccupied[i2, j2] && IsOccupied[i2 + 1, j2 + 1] && IsOccupied[i2 + 2, j2 + 2] && IsOccupied[i2 + 3, j2 + 3])
				{
					return true;
				}
			}
		}

		for (int j3 = 0; j3 < Width - 4; j3++)
		{
			for(int i3 = Height - 1; i3 > Height - 4; i3--)
			{
				if (IsOccupied[i3, j3] && IsOccupied[i3 - 1, j3 - 1] && IsOccupied[i3 - 2, j3 - 2] && IsOccupied[i3 - 3, j3 - 3])
				{
					return true;
				}
			}
		}
		return false;
	}

	public void InitializeBoard()
	{
		BoardPositions = new GameObject[Height, Width];
		IsOccupiedRed = new bool[Height, Width];
		IsOccupiedYellow = new bool[Height, Width];

		// Initialize the positions of the board with their game objects and set the positions to free (true)
		for (int i = 0; i < Height; i++)
		{
			for (int j = 0; j < Width; j++)
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
		TopSlots = new GameObject[Width];
		for (int i = 0; i < TopSlots.Length; i++)
		{
			TopSlots[i] = Slots.transform.GetChild(i).gameObject;
		}
	}
}
