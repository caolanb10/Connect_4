using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableMano : MonoBehaviour
{
	public GameObject Mano;

	public void EnableManoObjects()
	{
		Mano.SetActive(true);
	}
}
