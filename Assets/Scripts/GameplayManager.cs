using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
	public GameObject playerprefab;

	void Start()
	{
		if (!PhotonNetwork.IsConnected)
		{
			SceneManager.LoadScene("Main Menu");

			return;
		}

		if (playerprefab)
		{
			PhotonNetwork.Instantiate(playerprefab.name, Vector3.zero, Quaternion.identity);
		}
		else
		{
			Debug.Log("[GameplayManager] there is no player prefab");
		}
	}

}