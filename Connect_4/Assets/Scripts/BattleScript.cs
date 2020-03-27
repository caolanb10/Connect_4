using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleScript : MonoBehaviour
{
    public Spinner SpinnerScript;
    private float StartSpinSpeed;
    private float CurrentSpinSpeed;
    public Image SpinSpeedBar_Image;



    public void Awake()
    {
        StartSpinSpeed = SpinnerScript.spinSpeed;
        CurrentSpinSpeed = SpinnerScript.spinSpeed;
        SpinSpeedBar_Image.fillAmount = CurrentSpinSpeed / StartSpinSpeed;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            float mySpeed = gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            float otherPlayerSpeed = collision.collider.gameObject.GetComponent<Rigidbody>().velocity.magnitude;
            Debug.Log("My Speed " + mySpeed + " ----other player speed: " + otherPlayerSpeed);
            string message = (mySpeed > otherPlayerSpeed)
                ? "You damage the other player"
                : "You were damaged by the other player";
            Debug.Log(message);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
