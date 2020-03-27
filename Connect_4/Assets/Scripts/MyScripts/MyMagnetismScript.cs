using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMagnetismScript : MonoBehaviour
{
	public bool IsBoardPiece = false;
	public int PositionH;
	public int PositionW;

	public GameObject CollidingPiece;

	public void Initialise(bool isBoardPiece, int posH, int posW)
	{
		IsBoardPiece = isBoardPiece;
		PositionH = posH;
		PositionW = posW;
	}
}
