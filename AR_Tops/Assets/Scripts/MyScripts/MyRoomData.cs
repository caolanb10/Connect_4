using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRoomData: MonoBehaviour
{
	public static string RoomName = "null";

	public void SetRoom(string name)
	{
		RoomName = name;
	}

	public string GetRoom()
	{
		return RoomName;
	}
}
