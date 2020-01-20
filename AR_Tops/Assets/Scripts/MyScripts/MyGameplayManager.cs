using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MyGameplayManager : MonoBehaviour
{
	public TextMeshProUGUI UI_Inform_Text;

	// Game board slots
	public GameObject Positions;
	public GameObject[,] BoardPositions;

	// Top slots to place pieces
	public GameObject Slots;
	public GameObject[] TopSlots;

	public bool[,] IsOccupied;

	public bool ItemJustPlaced = false;
	
	// Board Dimensions
	public int height = 6;
	public int width = 7;

	public string SlotsName = "Connect_4_Board_Slots";
	public string PositionsName = "Positions";

	public string GameOver = "GameOver";

	void Start()
	{
		// Initialize board
		BoardPositions = new GameObject[height, width];
		IsOccupied = new bool[height, width];
		TopSlots = new GameObject[width];

		// Get parent game object for the board
		Positions = GameObject.Find(PositionsName);

		// Get Top Slots Parent
		Slots = GameObject.Find(SlotsName);

		// Initialize the positions of the board with their game objects and set the positions to free (true)
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				BoardPositions[i, j] = Positions.transform.GetChild(i).gameObject.transform.GetChild(j).gameObject;
				IsOccupied[i, j] = true;

				// Initialize the slots that will collide with the pieces (bottom)
				if (i == 0)
				{
					SetMagnetism(BoardPositions[i, j], true);
				}
				SetMagnetism(BoardPositions[i, j], false);
			}
		}

		// Top Slots will always magnetise
		InitialiseTopSlots();
    }

	public void SetMagnetism(GameObject position, bool val)
	{
		position.GetComponent<MyMagnetismScript>().WillMagnetise = val;
	}

	public void InitialiseTopSlots()
	{
		for(int i = 0; i<TopSlots.Length; i++)
		{
			TopSlots[i] = Slots.transform.GetChild(i).gameObject;
			SetMagnetism(TopSlots[i], true);
		}
	}

	public bool IsGameOver()
	{
		// For each row check horizontal
		for (int i = 0; i < height; i++)
		{
			for(int j = 0; j <= width - 4; j++)
			{
				if(IsOccupied[i,j] && IsOccupied[i, j + 1] && IsOccupied[i, j + 2] && IsOccupied[i, j + 3])
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

		// TODO: Implement non linear checks
		return false;
	}

	void SetPositionOccupied(int i, int j)
	{
		IsOccupied[i, j] = false;
	}

	void Update()
	{
		if (ItemJustPlaced)
		{
			if (IsGameOver())
			{
				UI_Inform_Text.text = GameOver;
			}
		}
	}
}
