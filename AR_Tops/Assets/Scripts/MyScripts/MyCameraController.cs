using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCameraController : MonoBehaviour
{
	public Camera cam;
	public float speed = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			cam.transform.Translate(0, 0, speed);
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			cam.transform.Translate(0, 0, -speed);
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			cam.transform.Translate(-speed, 0, 0);
		}
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			cam.transform.Translate(speed, 0, 0);
		}
	}
}
