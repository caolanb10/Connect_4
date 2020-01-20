using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MyGameplayManager : MonoBehaviour
{
	public TextMeshProUGUI UI_Inform_Text;
	public GameObject[,] BoardSlots;
	public bool[,] IsFree;
	public bool[,] WillCollide;

	public int height = 6;
	public int width = 7;
	public string PositionsName = "Positions";
	public GameObject Positions;

	public string GameOver = "GameOver";

	void Start()
	{
		// Initialize board
		BoardSlots = new GameObject[height, width];
		IsFree = new bool[height, width];
		WillCollide = new bool[height, width];

		// Get parent game object for the board
		Positions = GameObject.Find(PositionsName);

		// Initialize the slots of the board with their game objects and set the positions to free (true)
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				BoardSlots[i, j] = Positions.transform.GetChild(i).gameObject.transform.GetChild(j).gameObject;
				IsFree[i, j] = true;

				// Initialize the slots that will collide with the pieces
				if (i == 0)
				{
					WillCollide[i, j] = true;
				}
				WillCollide[i, j] = false;
			}
		}
    }

	bool IsGameOver()
	{
		// For each row check horizontal
		for (int i = 0; i < height; i++)
		{
			for(int j = 0; j <= width - 4; j++)
			{
				if(IsFree[i,j] == false && IsFree[i, j + 1] == false && IsFree[i, j + 2] == false && IsFree[i, j + 3] == false)
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
				if (IsFree[i1, j1] == false && IsFree[i1 + 1, j1] == false && IsFree[i1 + 2, j1] == false && IsFree[i1 + 3, j1] == false)
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
		IsFree[i, j] = false;
	}

    void Update()
    {
		if(IsGameOver())
		{
			UI_Inform_Text.text = GameOver;
		}
    }
}
