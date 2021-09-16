using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class PlayerManager : MonoBehaviour
{
	public Color color;

	void Start()
	{
		color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
	}

	void Update()
	{
		//Changes color over network for users to see
		if (Input.GetKeyDown(KeyCode.Space))
		{
			gameObject.GetPhotonView().RPC("ChangeColor", RpcTarget.AllBuffered, color.r, color.g, color.b);
		}

		//Changes color for local user to see
		if (Input.GetKeyDown(KeyCode.C))
		{
			color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
		}
	}

	[PunRPC]
	void ChangeColor(float r, float g, float b)
	{
		Color c = new Color(r, g, b);
		Camera.main.backgroundColor = c;
	}
}