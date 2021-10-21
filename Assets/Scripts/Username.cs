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
	public TMP_InputField red;
	public TMP_InputField green;
	public TMP_InputField blue;
	public Image color;

	public float RED;
	public float GREEN;
	public float BLUE;

	public static Username instance;

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

	public void LoadGame()
    {
		if(PhotonNetwork.IsMasterClient)
        {
			PhotonNetwork.LoadLevel(gameLevel);
        }
    }

	public void ChangeColor()
    {
		float redVal, greenVal, blueVal;

		if(!string.IsNullOrEmpty(red.ToString()))
        {
			float.TryParse(red.text.ToString(), out float resultRed);
			redVal = resultRed;
			RED = redVal;
			red.text = "";
		}
		else
        {
			redVal = 0;
			RED = 0;
        }

		if(!string.IsNullOrEmpty(green.ToString()))
        {
			float.TryParse(green.text.ToString(), out float resultGreen);
			greenVal = resultGreen;
			GREEN = greenVal;
			green.text = "";
		}
		else
        {
			greenVal = 0;
			GREEN = 0;
        }

		if (!string.IsNullOrEmpty(blue.ToString()))
        {
			float.TryParse(blue.text.ToString(), out float resultBlue);
			blueVal = resultBlue;
			BLUE = blueVal;
			blue.text = "";
		}
        else
        {
			blueVal = 0;
			BLUE = 0;
        }

		color.color = new Color32((byte)redVal, (byte)greenVal, (byte)blueVal, (byte)255);
	}

	[PunRPC]
	void UpdateNames(string _username)
	{
		names.text += _username + "\n";
	}
}