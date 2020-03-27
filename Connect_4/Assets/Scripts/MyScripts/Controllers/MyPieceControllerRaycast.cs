using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPieceControllerRaycast : MyPieceController
{
	protected override void Start()
	{
		base.Start();
		ScreenPosition = new Vector3(Screen.width / 2, Screen.height / 2);
	}
}
