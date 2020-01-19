using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class PlayerSelectionManager : MonoBehaviour
{
    public Button NextButton;
    public Button PrevButton;
    public Transform playerTransform;

    public int PlayerSelectionNumber;

    public GameObject[] spinnerTopModels;

    [Header("UI")]
    public TextMeshProUGUI PlayerModelType;
    public GameObject UI_Selection;
    public GameObject UI_AfterSelection;

    // Start is called before the first frame update
    void Start()
    {
        UI_Selection.SetActive(true);
        UI_AfterSelection.SetActive(false);
        PlayerSelectionNumber = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region UI Callback Region
    public void NextPlayer()
    {
        PlayerSelectionNumber += 1;
        if(PlayerSelectionNumber >= spinnerTopModels.Length)
        {
            PlayerSelectionNumber = 0;
        }

        Debug.Log(PlayerSelectionNumber);
        NextButton.enabled = false;
        PrevButton.enabled = false;
        Debug.Log("Here");
        StartCoroutine(Rotate(Vector3.up, playerTransform, 90, 1.0f));

        if( PlayerSelectionNumber == 0 || PlayerSelectionNumber == 1)
        {
            PlayerModelType.text = "Attack";
        }
        else
        {
            PlayerModelType.text = "Defend";
        }
    }
    public void PreviousPlayer()
    {
        PlayerSelectionNumber -= 1;
        if(PlayerSelectionNumber < 0)
        {
            PlayerSelectionNumber = spinnerTopModels.Length - 1;
        }
        Debug.Log(PlayerSelectionNumber);
        NextButton.enabled = false;
        PrevButton.enabled = false;
        Debug.Log("Here");
        StartCoroutine(Rotate(Vector3.up, playerTransform, -90, 1.0f));

        if (PlayerSelectionNumber == 0 || PlayerSelectionNumber == 1)
        {
            PlayerModelType.text = "Attack";
        }
        else
        {
            PlayerModelType.text = "Defend";
        }
    }

    public void OnSelectButtonClicked()
    {
        UI_Selection.SetActive(false);
        UI_AfterSelection.SetActive(true);
        ExitGames.Client.Photon.Hashtable playerSelectionProp = new ExitGames.Client.Photon.Hashtable
        { 
            { 
                MultiplayerSpinnerTopGame.PLAYER_SELECTION_NUMBER, PlayerSelectionNumber  
            } 
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerSelectionProp);
    }

    public void OnReselectButtonClicked()
    {
        UI_Selection.SetActive(true);
        UI_AfterSelection.SetActive(false);
    }

    public void OnBattleButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Scene_Gameplay");
    }

    public void OnBackButtonClicked()
    {
        SceneLoader.Instance.LoadScene("Scene_Lobby");
    }
    #endregion

    #region Private Methods
    IEnumerator Rotate(Vector3 axis, Transform transformToRotate, float angle, float duration = 1.0f)
    {
        Debug.Log("Started co routine");
        Quaternion originalRotation = transformToRotate.rotation;
        Quaternion finalRotation = transformToRotate.rotation * Quaternion.Euler(axis * angle);
        float elapsedTime = 0.0f;
        while(elapsedTime < duration)
        {
            transformToRotate.rotation = Quaternion.Slerp(originalRotation, finalRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;

        }
        transformToRotate.rotation = finalRotation;
        NextButton.enabled = true;
        PrevButton.enabled = true;
    }
    #endregion
}