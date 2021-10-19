using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Projectile : MonoBehaviour
{
    public Rigidbody rb;

    public float moveSpeed = 25;
    public int damage = 10;

    bool hasDealtDamage = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!rb)
        {
            Debug.Log("There's no RigidBody attached");
        }

        rb = gameObject.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.velocity = gameObject.transform.forward * moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        FPSPlayerManager p = collision.gameObject.GetComponent<FPSPlayerManager>();
        //GameObject p = collision.gameObject;

        if(p && !hasDealtDamage)
        {
            Destroy(gameObject);
            hasDealtDamage = true;
            p.gameObject.GetPhotonView().RPC("TakeDamage", RpcTarget.AllBuffered, damage);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
