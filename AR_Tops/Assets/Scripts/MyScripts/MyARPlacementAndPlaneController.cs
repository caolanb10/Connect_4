using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class MyARPlacementAndPlaneController : MonoBehaviour
{
	ARPlaneManager PlaneManager;
	MyARPlacementManager MyARPlacementManager;

	public TextMeshProUGUI UI_Inform;
	public GameObject UI_SearchForGames;

	string NotPlaced = "Connect 4 Board not placed, please place the board before searching for a game";
	string IsPlaced = "Connect 4 Board has been placed, you can now search for a game or adjust the board";

    // Start is called before the first frame update
    private void Awake()
    {
		PlaneManager = GetComponent<ARPlaneManager>();
		MyARPlacementManager = GetComponent<MyARPlacementManager>();
	}
	public void Start()
	{
		UI_Inform.text = NotPlaced;
	}
	public void DisableARPlacementAndPlaneDetection()
	{
		PlaneManager.enabled = false;
		MyARPlacementManager.enabled = false;
		SetAllPlanesActiveOrDeactive(false);
		UI_SearchForGames.gameObject.SetActive(true);
		UI_Inform.text = IsPlaced;
	}
	public void EnableARPlacementAndPlaneDetection()
	{
		PlaneManager.enabled = true;
		MyARPlacementManager.enabled = true;
		SetAllPlanesActiveOrDeactive(true);
		UI_SearchForGames.gameObject.SetActive(false);
		UI_Inform.text = NotPlaced;
	}
	private void SetAllPlanesActiveOrDeactive(bool value)
	{
		foreach(var plane in PlaneManager.trackables)
		{
			plane.gameObject.SetActive(value);
		}
	}
}
