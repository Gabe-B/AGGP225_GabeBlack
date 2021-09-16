using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class Username : MonoBehaviour
{

	public TMP_InputField input;
	public TMP_Text field;
	public static Username instance;

	private void Awake()
	{
		instance = this;
	}

	public void Submit()
	{
		if (!string.IsNullOrEmpty(input.text))
		{
			PhotonManager.instance.gameObject.GetPhotonView().RPC("UsernameRPC", RpcTarget.AllBuffered, PhotonManager.instance.username.ToString(), input.text);
			input.text = "";
		}
	}

	public void Leave()
	{
		PhotonNetwork.LeaveRoom();
		//PhotonNetwork.Disconnect();
	}
}