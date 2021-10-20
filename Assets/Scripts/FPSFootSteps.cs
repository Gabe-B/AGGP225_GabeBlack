using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class FPSFootSteps : MonoBehaviour
{
    public AudioSource AS;
    public FPSPlayerManager player;

    public void PlayAS()
    {
        gameObject.GetPhotonView().RPC("NetworkPlayAS", RpcTarget.All);
    }

    [PunRPC]
    void NetworkPlayAS()
    {
        AS.PlayOneShot(AS.clip);
    }
}
