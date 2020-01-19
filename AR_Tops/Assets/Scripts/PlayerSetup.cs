using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSetup : MonoBehaviourPun
{
    public TextMeshProUGUI PlayerName;

    // Start is called before the first frame update
    void Start()
    {
        if(photonView.IsMine)
        {
            // Local Player
            transform.GetComponent<MovementController>().enabled = true;
            transform.GetComponent<MovementController>().joystick.gameObject.SetActive(true);
        }
        else
        {
            // Remote
            transform.GetComponent<MovementController>().enabled = false;
            transform.GetComponent<MovementController>().joystick.gameObject.SetActive(false);
        }
        SetPlayerName();
    }

    void SetPlayerName()
    {
        if (PlayerName != null)
        {
            if (photonView.IsMine)
            {
                PlayerName.text = "YOU";
                PlayerName.color = Color.red;
            }
            else
            {
                PlayerName.text = photonView.Owner.NickName;
            }
        }
    }
}
