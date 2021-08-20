using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Player_MissleDetectSystem : MonoBehaviour
{
    //event subscribe here
    public GameObject[] bossMisslePorts_;
    public GameObject missleAlert_;

    Boss_MissleDefenseSystem EventsLockon;

    private void Start()
    {
        SetEventVariables();
    }


    private void SetEventVariables()
    {
        for(int i = 0; i < bossMisslePorts_.Length; i++)
        {
            EventsLockon = bossMisslePorts_[i].GetComponent<Boss_MissleDefenseSystem>();
            EventsLockon.EventDetectingPlayer_ += turnOnWarning;
            EventsLockon.EventLostPlayer_ += turnOffWarning;
        }

    }

    private void turnOnWarning(object Sender, EventArgs e)
    {
        missleAlert_.SetActive(true);
    }

    private void turnOffWarning(object Sender, EventArgs e)
    {
        missleAlert_.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
