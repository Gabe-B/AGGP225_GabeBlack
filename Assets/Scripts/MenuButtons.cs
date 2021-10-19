using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class MenuButtons : MonoBehaviour
{

	public TMP_InputField inputField;
	public GameObject pleaseInputText;

	public static MenuButtons instance;

	private void Awake()
	{
		instance = this;
	}

	public void OnCreateRoomClick()
	{
		if (!string.IsNullOrEmpty(inputField.text))
		{
			PhotonManager.instance.username = inputField.text;

			if (PhotonManager.instance != null)
			{
				PhotonManager.instance.CreateRoom();
			}
		}
		else
		{
			pleaseInputText.SetActive(true);
		}
	}

	public void OnJoinRandomRoomClick()
	{
		if (!string.IsNullOrEmpty(inputField.text))
		{
			PhotonManager.instance.username = inputField.text;

			if (PhotonManager.instance != null)
			{
				PhotonManager.instance.JoinRandomRoom();
			}
		}
		else
		{
			pleaseInputText.SetActive(true);
		}
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
			pleaseInputText.SetActive(true);
		}
	}

	public void Quit()
	{
		Application.Quit();
	}
}