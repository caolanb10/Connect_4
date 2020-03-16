using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPieceControllerSingleTouch : MyPieceController
{
	public bool EnableDesktopControls;

	protected override void FixedUpdate()
	{
		bool input = EnableDesktopControls
			? Input.GetMouseButton(0)
			: Input.touchCount > 0;

		if (input)
		{
			if (EnableDesktopControls)
			{
				ScreenPosition = Input.mousePosition;
			}
			else
			{
				ScreenPosition = Input.GetTouch(0).position;
			}

			Grab();

			base.FixedUpdate();
		}

		// No touch
		else
		{
			Release();
		}
	}
}
