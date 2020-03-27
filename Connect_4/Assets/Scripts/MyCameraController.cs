using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCameraController : MonoBehaviour
{
	private float Speed = 0.01f;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			gameObject.transform.Translate(0, 0, Speed);
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			gameObject.transform.Translate(0, 0, -Speed);
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			gameObject.transform.Translate(-Speed, 0, 0);
		}
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			gameObject.transform.Translate(Speed, 0, 0);
		}
	}
}
