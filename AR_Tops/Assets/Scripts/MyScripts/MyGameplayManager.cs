using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class MyGameplayManager : MonoBehaviour//, PunObservable
{
	PhotonView PhotonView;

	public Canvas UI;
	public TextMeshProUGUI UI_Inform_Text;

	// Game board Positions
	public GameObject[,] BoardPositions;
	public bool[,] IsOccupiedYellow;
	public bool[,] IsOccupiedRed;

	// Top slots to place pieces
	private GameObject[] TopSlots;

	public GameObject PieceJustPlaced = null;
	public string PieceJustPlacedColour = null;

	public int SlotPlaced;

	// Board Dimensions
	private int height = 6;
	private int width = 7;

	private Quaternion Rotation = Quaternion.Euler(90, 0, 0);

	private string PrefabParentName = "Board_Objects";
	private string BoardPiecesSlots = "Board_Pieces_Slots";
	private string SlotsName = "Connect_4_Board_Slots";
	private string PositionsName = "Connect_4_Board_Positions";

	private string Yellow = "Yellow";
	private string Red = "Red";


	void Awake()
	{
		PhotonView = GetComponent<PhotonView>();
	}

	void Start()
	{
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

		if (position.GetComponent<MyMagnetismScript>().WillMagnetise)
		{
			// Collision on board
			if (piecePlaced.Intersects(positionBounds))
			{
				PlacePiece(position);
			}
		}
	}

	public void PlacePiece(GameObject boardPosition)
	{
		Rigidbody rb = PieceJustPlaced.GetComponent<Rigidbody>();

		// Not sure whether to use RB or Transform atm
		rb.transform.position = boardPosition.transform.position;
		rb.transform.rotation = Rotation;

		int positionH = boardPosition.GetComponent<MyMagnetismScript>().PositionH;
		int positionW = boardPosition.GetComponent<MyMagnetismScript>().PositionW;

		// No Gravity
		rb.useGravity = false;

		// Freeze Piece
		rb.isKinematic = true;

		// Placed in board
		PieceJustPlaced.GetComponent<MyPiecePlacer>().IsInPosition = true;

		// Update board state and state of board positions objects
		if (PieceJustPlacedColour == Yellow) IsOccupiedYellow[positionH, positionW] = true;
		else IsOccupiedRed[positionH, positionW] = true;

		// Disable this slot
		BoardPositions[positionH, positionW].GetComponent<MyMagnetismScript>().WillMagnetise = false;

		// Enable the slot above it
		BoardPositions[positionH + 1, positionW].GetComponent<MyMagnetismScript>().WillMagnetise = true;
		
		// Piece has been placed, delete reference
		PieceJustPlaced = null;
		PieceJustPlacedColour = null;
		
		if(IsGameOver(Yellow) || IsGameOver(Red))
		{

			string winner = IsGameOver(Yellow) ? Yellow : Red;
			string winningText = "Game over " + winner + " won the game";

			UI_Inform_Text.text = winningText;
			UI.enabled = true;
		}
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

	public void SetMagnetism(GameObject position, bool val)
	{
		position.GetComponent<MyMagnetismScript>().WillMagnetise = val;
	}

	public bool IsGameOver(string colour)
	{
		bool[,] IsOccupied = colour == Yellow ? IsOccupiedYellow : IsOccupiedRed;
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

		// Get parent game object for the board
		GameObject Positions = GameObject.Find(PrefabParentName + "/" + BoardPiecesSlots + "/" + PositionsName);
		// Initialize the positions of the board with their game objects and set the positions to free (true)
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				BoardPositions[i, j] = Positions.transform.GetChild(i).gameObject.transform.GetChild(j).gameObject;
				BoardPositions[i, j].GetComponent<MyMagnetismScript>().IsBoardPiece = true;
				BoardPositions[i, j].GetComponent<MyMagnetismScript>().PositionH = i;
				BoardPositions[i, j].GetComponent<MyMagnetismScript>().PositionW = j;

				IsOccupiedRed[i, j] = false;
				IsOccupiedYellow[i, j] = false;

				// Initialize the slots that will collide with the pieces (bottom)
				if (i == 0)
				{
					SetMagnetism(BoardPositions[i, j], true);
				}
				else
				{
					SetMagnetism(BoardPositions[i, j], false);
				}
			}
		}
	}
	public void InitialiseTopSlots()
	{
		TopSlots = new GameObject[width];
		GameObject Slots = Slots = GameObject.Find(PrefabParentName + "/" + BoardPiecesSlots + "/" + SlotsName);
		for (int i = 0; i < TopSlots.Length; i++)
		{
			TopSlots[i] = Slots.transform.GetChild(i).gameObject;
			SetMagnetism(TopSlots[i], true);
		}
	}
}
