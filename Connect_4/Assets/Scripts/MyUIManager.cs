using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class MyUIManager : MonoBehaviour
{
	public GameObject InteractionDropdown;

	public GameObject UI_Inform_Panel;

	public GameObject MarkerButton;

	public GameObject MarkerLessButton;

	public GameObject StartGameButton;

	public GameObject DoneButton;

	public GameObject Change_Rotation_Buttons;

	public GameObject Change_Scale_Buttons;

	public GameObject Adjust;

	public GameObject Place;

	public GameObject Rotate;

	public GameObject Scale;

	public GameObject BackToLobby;

	public TextMeshProUGUI Room_Text;

	public TextMeshProUGUI UI_InformPanel_Text;

	public GameObject Pointer;

	public bool IsInGame = false;

	public int[] states = new int[] { 0, 1, 2, 3 };

	private string Marker = "Would you like to play using a marker or without a marker";

	private string NotPlaced = "Please place board to search for games";

	private string Placed = "Board has been placed, you can now adjust the board or search for a game";

	public void Start()
	{
		BeforePlaced();
	}


	public void BeforePlaced()
	{
		UI_InformPanel_Text.text = Marker;
		DeactivateEverything();
		MarkerButton.SetActive(true);
		MarkerLessButton.SetActive(true);
	}

	// Board not placed
	public void StateZero()
	{
		UI_InformPanel_Text.text = NotPlaced;
		DeactivateEverything();
		Place.SetActive(true);
		Scale.SetActive(true);
	}
	
	// Board placed
	public void StateOne()
	{
		DeactivateEverything();
		Scale.SetActive(true);
		Rotate.SetActive(true);
		Adjust.SetActive(true);
		Place.SetActive(true);
		if(!IsInGame) StartGameButton.SetActive(true);
		if (!IsInGame) UI_InformPanel_Text.text = Placed;
	}

	// Scale
	public void StateTwo()
	{
		DeactivateEverything();
		Change_Scale_Buttons.SetActive(true);
		DoneButton.SetActive(true);
		if (!IsInGame) UI_InformPanel_Text.text = NotPlaced;
	}

	// Rotation
	public void StateThree()
	{
		DeactivateEverything();
		Change_Rotation_Buttons.SetActive(true);
		DoneButton.SetActive(true);
		if (!IsInGame) UI_InformPanel_Text.text = NotPlaced;
	}

	// In Game
	public void StateInGame()
	{
		DeactivateEverythingInGame();
	}

	public void StateGameOver()
	{
		DeactivateEverything();
		BackToLobby.SetActive(true);
	}

	public void DeactivateEverything()
	{
		MarkerButton.SetActive(false);
		MarkerLessButton.SetActive(false);
		Adjust.SetActive(false);
		Place.SetActive(false);
		DoneButton.SetActive(false);
		StartGameButton.SetActive(false);
		Scale.SetActive(false);
		Rotate.SetActive(false);
		InteractionDropdown.SetActive(false);
		Change_Rotation_Buttons.SetActive(false);
		Change_Scale_Buttons.SetActive(false);
		BackToLobby.SetActive(false);
	}

	public void DeactivateEverythingInGame()
	{
		DeactivateEverything();
		Pointer.SetActive(false);
		UI_Inform_Panel.SetActive(false);
		InteractionDropdown.SetActive(true);
	}
}
