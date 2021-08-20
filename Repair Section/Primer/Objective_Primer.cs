using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Objective_Primer : MonoBehaviour
{
    public event EventHandler OnPrimerIgnited_;
    public event EventHandler OnPrimerOff_;


    public Text currentTime;

    public float _timeOnPrime;
    float chargeCountdown;
    float _timeToUse;

    [SerializeField]
    float primerScale;
    float multiplier;

    public Material[] materialColor;

    Renderer primerColor;

    int SceneLoaded;

    public static bool primed;

    private void Start()
    {
        SetScene();//Before IncreaseTimer so multiplier gets the right scene.

        ManageTimer();//Before Timer so timer gets the right multiplier

        SetGlobalVariables();

        SetEventVariables();

    }

    private void SetEventVariables()
    {
        OnPrimerOff_ += PrimerDeactive;
        OnPrimerIgnited_ += ChangePrimer;
    }

    private void PrimerDeactive(object Sender, EventArgs e)
    {

        chargeCountdown = _timeToUse;//reset primer timer
        primerColor.sharedMaterial = materialColor[0];

        primed = false;

    }

    private void SetScene()
    {
        SceneLoaded = GameManager_Terminal_Buttons.SceneLoaded;
    }

    public void SetGlobalVariables()
    {
        primerColor = GetComponent<Renderer>();


        primed = false;
    }

    private void OnParticleCollision(GameObject player)
    {
        if (player.gameObject.tag == "Player" && !primed)
        {
            OnPrimerIgnited_?.Invoke(this, EventArgs.Empty);//Triggers primer on Event
        }
    }

    private void ChangePrimer(object Sender, EventArgs e)
    {
        primerColor.sharedMaterial = materialColor[1];

        primed = true;
    }

    void Update()
    {
        if (primed == true)
            TrackingTime();
    }

    public void ManageTimer()
    {


        if (SceneLoaded == 1)
        {
            //multiplier = CannonObstacleActivation.completionFactor;
            if (GameManager_CannonObstacleActivation.completionFactor > 1)
            {
                multiplier = GameManager_CannonObstacleActivation.completionFactor * primerScale;

                chargeCountdown = _timeOnPrime * multiplier;
            }
            else
            {
                chargeCountdown = _timeOnPrime;
            }
        }
        else if (SceneLoaded == 2)
        {
            multiplier = GameManager_BoostersObstacleActivation.completionFactor * primerScale;
            if (GameManager_BoostersObstacleActivation.completionFactor > 1)
            {
                multiplier = GameManager_BoostersObstacleActivation.completionFactor * primerScale;
                chargeCountdown = _timeOnPrime * multiplier;
            }
            else
            {
                chargeCountdown = _timeOnPrime;
            }
        }
        else if (SceneLoaded == 3)
        {
            if (GameManager_ShieldObstacleActivation.completionFactor > 1)
            {
                multiplier = GameManager_ShieldObstacleActivation.completionFactor * primerScale;
                chargeCountdown = _timeOnPrime * multiplier;
            }
            else
            {
                chargeCountdown = _timeOnPrime;
            }
        }

        _timeToUse = chargeCountdown;
    }

    private void TrackingTime()
    {
        string seconds = (chargeCountdown % 60).ToString("f0");

        currentTime.text = "00:" + seconds;

        chargeCountdown -= 1 * Time.deltaTime; 

        if (chargeCountdown <= 0)
        {
            OnPrimerOff_?.Invoke(this, EventArgs.Empty);//triggers primer off event


        }
    }
}
