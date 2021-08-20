using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_Primer : MonoBehaviour
{
    GameObject chargerObject;
    public GameObject player;

    public Text currentTime;

    public float primerTimer = 15;

    //float countDown;

    float chargeCountdown;

    float multiplier = 2;

    public Material[] materialColor;

    Renderer primerColor;

    public static bool primed;

    private void Start()
    {
        SetGlobals();
    }

    private void SetGlobals()
    {
        primerColor = GetComponent<Renderer>();

        primed = false;

        chargeCountdown = primerTimer * multiplier;
    }

    private void OnParticleCollision(GameObject chargerObject)
    {
        if (chargerObject.gameObject.tag == "Player" && !primed)
        {
            primed = true;
            primerColor.sharedMaterial = materialColor[1];
            player.name = "Player";
        }
    }

    void primeLoss()
    {
        primed = false;
        chargeCountdown = primerTimer * multiplier;
        primerColor.sharedMaterial = materialColor[0];
    }

    void Update()
    {
        TrackingTime();
        //print(primed);
    }

    private void TrackingTime()
    {
        if (primed == true)
        {
            chargeCountdown -= 1 * Time.deltaTime; ;

            string seconds = (chargeCountdown % 60).ToString("f0");

            currentTime.text = "00:" + seconds;
        }

        if (chargeCountdown <= 0)
        {
            primeLoss();
        }
    }
}
