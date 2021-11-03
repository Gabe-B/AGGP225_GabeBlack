using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PhotonManager : MonoBehaviourPunCallbacks
{
	public static PhotonManager instance { get; private set; }

	public string username;
	public GameObject playerPrefab;
	public GameObject connectedText;
	public TMP_InputField red;
	public TMP_InputField green;
	public TMP_InputField blue;
	public Image color;

	public float RED, GREEN, BLUE;
	public int MAX_PLAYERS = 4;

	RoomOptions roomOptions = new RoomOptions();

	string gameVersion = "1";
	string gameLevel = "Chat Room";

	void Awake()
	{
		if (instance)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			gameObject.AddComponent<PhotonView>();
			gameObject.GetPhotonView().ViewID = 999;
			DontDestroyOnLoad(this);
		}

		SceneManager.sceneLoaded += OnSceneLoaded;

		PhotonNetwork.AutomaticallySyncScene = true;
		roomOptions.MaxPlayers = (byte)MAX_PLAYERS;
	}

	void Start()
	{
		Connect();
	}

    /// <summary>
    /// Connects user to master server
    /// </summary>
    public void Connect()
	{
		if (!PhotonNetwork.IsConnected)
		{
			PhotonNetwork.ConnectUsingSettings();
			PhotonNetwork.GameVersion = gameVersion;
			connectedText.SetActive(true);
		}
		else
        {
			connectedText.SetActive(false);
        }
	}

	public void CreateRoom()
	{
		Debug.Log("[PhotonManager][CreateRoom][Trying to create room]");

		PhotonNetwork.NickName = MenuButtons.instance.inputField.text;

		PhotonNetwork.CreateRoom("Test Room", roomOptions);
	}

	public void JoinRandomRoom()
	{
		Debug.Log("[PhotonManager][JoinRandomRoom][Trying to join random room]");

		PhotonNetwork.NickName = MenuButtons.instance.inputField.text;

		PhotonNetwork.JoinRandomRoom();
	}

	public void JoinChatroom()
	{
		Debug.Log("[PhotonManager][JoinChatroom][Trying to join random room]");

		PhotonNetwork.NickName = MenuButtons.instance.inputField.text;

		PhotonNetwork.JoinRandomRoom();
	}

	public void ChangeColor()
	{
		float redVal, greenVal, blueVal;

		if (!string.IsNullOrEmpty(red.ToString()))
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

		if (!string.IsNullOrEmpty(green.ToString()))
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

	#region Photon Callbacks
	public override void OnConnectedToMaster()
	{
		Debug.Log("[PhotonManager][Connected to Master]");
	}

	public override void OnCreatedRoom()
	{
		Debug.Log("[PhotonManager][OnCreatedRoom]");
	}

	public override void OnJoinedRoom()
	{
		Debug.Log("[PhotonManager][OnJoinedRoom]");

		if(PhotonNetwork.IsMasterClient)
        {
			PhotonNetwork.LoadLevel(gameLevel);
		}
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		Debug.Log("[PhotonManager][OnDisconnected] " + cause);
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		Debug.Log("[PhotonManager][OnCreateRoomFailed] " + message);
		JoinRandomRoom();
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		Debug.Log("[PhotonManager][OnJoinRandomFailed] " + message);
		CreateRoom();
	}

	public override void OnLeftRoom()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		SceneManager.LoadScene("Main Menu");
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (instance != this)
		{
			//PhotonNetwork.Destroy(gameObject);
		}
	}
    #endregion

    #region RPC's
    [PunRPC]
	void UsernameRPC(string _username, string _chat)
	{
		Username.instance.field.text += _username + ":	" + _chat + "\n";
	}

	/*[PunRPC]
	void ChatRPC(string _username, string _chat)
	{
		string message = _username + ": " + _chat + "\n";
		FPSChat.instance.SendMessageToChat(message);
	}*/

	[PunRPC]
	void FPSUsernameRPC(string  _username, string nameField)
	{
		nameField = _username;
	}

	[PunRPC]
	void UpdateGameTimer(float t)
	{
        foreach (FPSGameManager gm in FindObjectsOfType<FPSGameManager>())
        {
			//gm.matchTime -= Time.deltaTime;
			gm.timer.text = Mathf.Round(t).ToString();
		}

		/*FPSGameManager.instance.timer.text = Mathf.Round(FPSGameManager.instance.matchTime).ToString();
		FPSGameManager.instance.matchTime -= Time.deltaTime;*/
	}

	[PunRPC]
	void UpdateLobbyTimer(float t)
    {
        foreach (Username u in FindObjectsOfType<Username>())
        {
			u.timer.text = Mathf.Round(t).ToString();
        }
    }

	[PunRPC]
	void AllLeave()
    {
		foreach (FPSPlayerManager pm in FindObjectsOfType<FPSPlayerManager>())
		{
			//gm.matchTime -= Time.deltaTime;
			PhotonNetwork.LeaveRoom();
		}
	}

	[PunRPC]
    void LoadLobby()
    {
		if(PhotonNetwork.IsMasterClient)
        {
			PhotonNetwork.LoadLevel("Chat Room");
        }

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	[PunRPC]
	void ShowWinner(string _username, int kills)
    {
		Username.instance.winnerText.text = _username + " won the last round with " + kills.ToString() + " kills!";
    }
    #endregion
}