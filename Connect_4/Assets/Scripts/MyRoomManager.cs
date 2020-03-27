using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class MyRoomManager : MonoBehaviourPunCallbacks
{
	public GameObject[] Rooms;
	public TextMeshProUGUI[] RoomTexts;
	public MyRoomData RoomData;

	private int SelectedIndex = 0;
	private int NumberOfRooms = 5;

	public void OnClickRoom(int index)
	{
		ClearHighlight();
		SelectedIndex = index;
		Highlight(index - 1);
	}

	void Highlight(int index)
	{
		Rooms[index].GetComponent<Image>().fillCenter = true;
	}

	void ClearHighlight()
	{
		for (int i = 0; i < NumberOfRooms; i++)
		{
			Rooms[i].GetComponent<Image>().fillCenter = false;
		}
	}

	public void SetRoomName()
	{
		RoomData.SetRoom(RoomTexts[SelectedIndex - 1].text);
	}

	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		int i = 0;

		foreach (RoomInfo room in roomList)
		{
			Rooms[i].SetActive(true);
			RoomTexts[i].text = room.Name;
			i++;
		}

		// Set empty UI elements to be disabled
		for (int j = i; j < NumberOfRooms; j++)
		{
			Rooms[j].SetActive(false);
		}
	}
}
