using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyFlippedCoordinates
{
	public static Vector3 PositionRelativeToBoard(Vector3 position, GameObject Board)
	{
		return position - Board.transform.position;
	}

	public static Vector3 FlipPerspectiveOfBoardPiece(Vector3 position, GameObject Board)
	{
		Vector3 flippedXAndZ = new Vector3(-position.x, position.y, -position.z);
		return flippedXAndZ + Board.transform.position;
	}
}
