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
	public GameObject FPS_UI;

	public TextMeshProUGUI Room_Text;
	public TextMeshProUGUI UIInformPanelText;

	public int frameRange = 60;
	public int[] FPS_Buffer;
	public int fpsBufferIndex;
	public int AverageFPS;

	public bool IsInGame = false;

	private string Marker = "Would you like to play using a marker or without a marker";
	private string NotPlaced = "Please place board to search for games";
	private string Placed = "Board has been placed, you can now adjust the board or search for a game";

	public void Start()
	{
		BeforePlaced();
	}

	void InitializeBuffer()
	{
		if (frameRange <= 0)
		{
			frameRange = 1;
		}
		FPS_Buffer = new int[frameRange];
		fpsBufferIndex = 0;
	}

	void UpdateBuffer()
	{
		int frameRate = (int)(1f / Time.unscaledDeltaTime);
		Debug.Log(frameRate);

		FPS_Buffer[fpsBufferIndex++] = (int)(1f / Time.unscaledDeltaTime);
		if (fpsBufferIndex >= frameRange)
		{
			fpsBufferIndex = 0;
		}
	}

	void CalculateFPS () {
		int sum = 0;
		for (int i = 0; i < frameRange; i++)
		{
			sum += FPS_Buffer[i];
		}
		AverageFPS = sum / frameRange;
	}

	public void Update()
	{
		if (FPS_Buffer == null || FPS_Buffer.Length != frameRange)
		{
			InitializeBuffer();
		}
		UpdateBuffer();
		CalculateFPS();
		FPS_UI.GetComponent<TextMeshProUGUI>().text = AverageFPS.ToString();
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
