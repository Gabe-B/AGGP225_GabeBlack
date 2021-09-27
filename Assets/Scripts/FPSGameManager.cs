using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class FPSGameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public List<GameObject> spawnPoints = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        if(!PhotonNetwork.IsConnected)
		{
            SceneManager.LoadScene("Main Menu");

            return;
		}

        if(playerPrefab)
		{
            GameObject spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count - 1)];
            playerPrefab = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.transform.position, Quaternion.identity);
		}
		else
		{
            Debug.Log("[FPSGameManager] There is no player prefab attached");
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
