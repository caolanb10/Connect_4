using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMagnetismScript : MonoBehaviour
{
	public bool WillMagnetise;

	public GameObject CollidingPiece;

    void Start()
    {
		GetComponent<SphereCollider>().enabled = false;
    }

    void Update()
    {
        
    }
}
