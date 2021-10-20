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
	public TMP_Text names;
	public static Username instance;

	private void Awake()
	{
		instance = this;
	}

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
			Submit();
        }
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