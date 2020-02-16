﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyFlippedCoordinates
{
	static float xAdjust = 0.009f;

	public static Vector3 PositionRelativeToBoard(Vector3 position, GameObject Board)
	{
		return position - Board.transform.position;
	}

	public static Vector3 FlipPerspectiveOfBoardPiece(Vector3 position, GameObject Board)
	{
		Vector3 flippedXAndZ = new Vector3(-(position.x + xAdjust), position.y, -position.z);
		return flippedXAndZ + Board.transform.position;
	}
}
