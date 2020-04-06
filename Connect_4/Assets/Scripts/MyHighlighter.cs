using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyHighlighter : MonoBehaviour
{
	Collider ThisCollider;
	public GameObject HighlightedPiece;

    void Start()
    {
		ThisCollider = GetComponent<Collider>();
    }

	private void OnTriggerEnter(Collider collider)
	{
		if (IsGameObjectPieceAndOwned(collider.gameObject) && HighlightedPiece == null)
		{
			HighlightedPiece = collider.gameObject;
			HighlightedPiece.GetComponent<MyChangeShader>().ChangeShaderSelected();
		}
	}

	private void OnTriggerExit(Collider collider)
	{
		if (collider.gameObject == HighlightedPiece)
		{
			HighlightedPiece.GetComponent<MyChangeShader>().ChangeShaderNormal();
			HighlightedPiece = null;
		}
	}

	private bool IsGameObjectPieceAndOwned(GameObject obj)
	{
		return obj.tag == "Piece" && (obj.GetComponent<MyPiecePlacer>().IsOwned == true);
	}
}
