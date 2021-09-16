using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class LeaveButton : MonoBehaviour
{
	public void Leave()
	{
		PhotonNetwork.LeaveRoom();
	}
}
