using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class FPSGameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject playerInstace;
    public List<GameObject> spawnPoints = new List<GameObject>();

    public FPSGameManager instance { get; set; }

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(!PhotonNetwork.IsConnected)
		{
            SceneManager.LoadScene("Main Menu");

            return;
		}
        else
        {
            if(instance == this)
            {
                if (playerPrefab)
                {
                    GameObject spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count - 1)];
                    GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.transform.position, Quaternion.identity);
                    player.GetComponent<FPSPlayerManager>().playerCam.gameObject.GetComponent<Camera>().enabled = true;
                    PhotonManager.instance.gameObject.GetPhotonView().RPC("FPSUsernameRPC", RpcTarget.AllBuffered, PhotonManager.instance.username.ToString());
                }
                else
                {
                    Debug.Log("[FPSGameManager] There is no player prefab attached");
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
