using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class FPSChat : MonoBehaviour
{
    public static FPSChat instance { get; set;  }

    /*
    public TMP_InputField input;
    public TMP_Text playerNameField;
    //public FPSPlayerManager player;
    */

    public RectTransform TextHolder;
    public GameObject textPrefab;
    public int maxMessages = 10;

    [SerializeField]
    public List<Message> messageList = new List<Message>();

    /*
    public static FPSChat instance { get; set; }

    void Awake()
    {
        instance = this;

        playerNameField.text = PhotonNetwork.NickName;
        input.interactable = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            input.interactable = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            //player.canMove = false;
        }

        if (Input.GetKeyDown(KeyCode.Return) && (input.interactable = true))
        {
            PhotonManager.instance.gameObject.GetPhotonView().RPC("ChatRPC", RpcTarget.AllBuffered, playerNameField.text, input.text);
            input.text = "";
            input.interactable = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            //player.canMove = true;
        }
    }
    */

    void Awake()
    {
        instance = this;
    }

    public void SendMessageToChat(string text)
    {
        if (messageList.Count >= maxMessages)
        {
            Destroy(messageList[0].textObj.gameObject);
            messageList.Remove(messageList[0]);
        }

        Message newMessage = new Message();

        newMessage.text = text;

        GameObject newTextObj = Instantiate(textPrefab, TextHolder);

        newMessage.textObj = newTextObj.GetComponent<TMP_Text>();

        newMessage.textObj.text = newMessage.text;

        messageList.Add(newMessage);
    }
}


[System.Serializable]
public class Message
{
    public string text;
    public TMP_Text textObj;

    public override string ToString()
    {
        return text;
    }
}
