using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class MenuButtons : MonoBehaviour
{

	public TMP_InputField inputField;

	public static MenuButtons instance;

	private void Awake()
	{
		instance = this;
	}

	public void OnCreateRoomClick()
	{
		PhotonManager.instance.CreateRoom();
	}

	public void OnJoinRandomRoomClick()
	{
		PhotonManager.instance.JoinRandomRoom();
	}

	public void OnJoinChatroomClick()
	{
		if (!string.IsNullOrEmpty(inputField.text))
		{
			PhotonManager.instance.username = inputField.text;

			if (PhotonManager.instance != null)
			{
				PhotonManager.instance.JoinChatroom();
			}
		}
		else
		{
			Debug.LogError("[MenuButtons][OnJoinChatroomClick][Unable to join random room. PhotonManager unable to be found]");
		}
	}

	public void Quit()
	{
		Application.Quit();
	}
}