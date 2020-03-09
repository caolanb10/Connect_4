using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCameraController : MonoBehaviour
{
	private float speed = 0.01f;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			gameObject.transform.Translate(0, 0, speed);
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			gameObject.transform.Translate(0, 0, -speed);
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			gameObject.transform.Translate(-speed, 0, 0);
		}
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			gameObject.transform.Translate(speed, 0, 0);
		}
	}
}
