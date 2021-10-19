using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class FPSWeaponSounds : MonoBehaviour
{
    public AudioSource weaponSound;
    public AudioClip oui;

    // Start is called before the first frame update
    void Start()
    {
        weaponSound.clip = oui;
        weaponSound.spatialBlend = 1;
        weaponSound.minDistance = 1;
        weaponSound.maxDistance = 90;
        weaponSound.pitch = Random.Range(1f, 2f);
        weaponSound.volume = 0.1f;
        weaponSound.rolloffMode = AudioRolloffMode.Linear;
        weaponSound.dopplerLevel = 0;
        weaponSound.PlayOneShot(weaponSound.clip);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
