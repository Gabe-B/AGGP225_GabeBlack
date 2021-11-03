using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class Username : MonoBehaviour
{
	public TMP_InputField input;
	public TMP_Text field;
	public TMP_Text names;
	public GameObject startButton;
	public TMP_Text timer;
	public TMP_Text winnerText;

	public float time = 20f;
	bool isTimerRunning = true;

	public static Username instance { get; set; }

	string gameLevel = "FPS";

	private void Awake()
	{
		instance = this;

		gameObject.GetPhotonView().RPC("UpdateNames", RpcTarget.AllBuffered, PhotonManager.instance.username.ToString());

		if(PhotonNetwork.IsMasterClient)
        {
			startButton.SetActive(true);
        }
		else
        {
			startButton.SetActive(false);
        }
	}

    void Start()
    {
		PhotonManager.instance.gameObject.GetPhotonView().RPC("ShowWinner", RpcTarget.All, FPSGameManager.instance.temp.nameText.text, FPSGameManager.instance.temp.kills);
	}

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
			Submit();
        }

		if(PhotonNetwork.CurrentRoom.PlayerCount == 4)
        {
			if(isTimerRunning)
            {
				if (time > 0)
				{
					time -= Time.deltaTime;

					PhotonManager.instance.gameObject.GetPhotonView().RPC("UpdateLobbyTimer", RpcTarget.AllBuffered, time);
				}
				else
				{
					time = 0;
					isTimerRunning = false;

					LoadGame();
				}
			}
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

	public void LoadGame()
    {
		if(PhotonNetwork.IsMasterClient)
        {
			PhotonNetwork.LoadLevel(gameLevel);
        }
    }

	[PunRPC]
	void UpdateNames(string _username)
	{
		names.text += _username + "\n";
	}
}