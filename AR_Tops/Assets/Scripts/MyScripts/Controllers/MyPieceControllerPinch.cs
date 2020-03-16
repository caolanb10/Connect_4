using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MyPieceControllerPinch : MyPieceController
{
	public Vector3 SecondTouchPosition;

	private float DistanceBetweenTouches = 0f;

	private float ChangableDistance = Distance;

	public float Speed = 0.00025f;

	protected override void FixedUpdate()
	{
		if (Input.touchCount > 0)
		{
			Debug.Log("Distance" + Distance);

			ScreenPosition = Input.GetTouch(0).position;
			if (Input.touchCount == 2)
			{
				SecondTouchPosition = Input.GetTouch(1).position;

				float previousDistanceBetweenTouches = DistanceBetweenTouches != 0f
					? DistanceBetweenTouches
					: Vector3.Distance(ScreenPosition, SecondTouchPosition);

				DistanceBetweenTouches = Vector3.Distance(ScreenPosition, SecondTouchPosition);

				Distance += (DistanceBetweenTouches - previousDistanceBetweenTouches) * Speed;
			}
			else
			{
				Distance = 0.4f;
				DistanceBetweenTouches = 0f;
			}
			Grab();
			base.FixedUpdate();
		}
		else
		{
			Release();
		}
	}
}
