using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class MyUIManager : MonoBehaviour
{
	public GameObject InteractionDropdown;
	public GameObject UIInformPanel;
	public GameObject MarkerButton;
	public GameObject MarkerLessButton;
	public GameObject StartGameButton;
	public GameObject DoneButton;
	public GameObject ChangeRotationButtons;
	public GameObject ChangeScaleButtons;
	public GameObject Adjust;
	public GameObject Place;
	public GameObject Rotate;
	public GameObject Scale;
	public GameObject BackToLobby;
	public GameObject RaycastCentre;

	public TextMeshProUGUI Room_Text;
	public TextMeshProUGUI UIInformPanelText;

	public bool IsInGame = false;

	private string Marker = "Would you like to play using a marker or without a marker";
	private string NotPlaced = "Please place board to search for games";
	private string Placed = "Board has been placed, you can now adjust the board or search for a game";

	public void Start()
	{
		BeforePlaced();
	}

	public void BeforePlaced()
	{
		UIInformPanelText.text = Marker;
		DeactivateEverything();
		MarkerButton.SetActive(true);
		MarkerLessButton.SetActive(true);
	}

	// Board not placed
	public void StateZero()
	{
		UIInformPanelText.text = NotPlaced;
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
		if (!IsInGame) UIInformPanelText.text = Placed;
	}

	// Scale
	public void StateTwo()
	{
		DeactivateEverything();
		ChangeScaleButtons.SetActive(true);
		DoneButton.SetActive(true);
		if (!IsInGame) UIInformPanelText.text = NotPlaced;
	}

	// Rotation
	public void StateThree()
	{
		DeactivateEverything();
		ChangeRotationButtons.SetActive(true);
		DoneButton.SetActive(true);
		if (!IsInGame) UIInformPanelText.text = NotPlaced;
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
		ChangeRotationButtons.SetActive(false);
		ChangeScaleButtons.SetActive(false);
		BackToLobby.SetActive(false);
	}

	public void DeactivateEverythingInGame()
	{
		DeactivateEverything();
		RaycastCentre.SetActive(false);
		UIInformPanel.SetActive(false);
		InteractionDropdown.SetActive(true);
	}
}
