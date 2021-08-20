using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles_LevelHazard_ControlHazard : MonoBehaviour
{
    public GameObject hazardToTrigger_;
    public GameObject hazardManger_;



    public float hazardTime_;
    float origHazardTimer_;
    float newHazardTime_;

    private void Awake()
    {
        origHazardTimer_ = hazardTime_;
    }

    void StopHazard()//called too many times
    {
        fireHazard(false);
        hazardTime_ = origHazardTimer_;//resets hazard time on

        newHazardTime_ = Random.Range(9, 13);//new amount of time it takes to call next hazard

        print("stopped hazard");


        Invoke("triggerNewHazardCall", newHazardTime_);//calls for new hazard
    }

    private void triggerNewHazardCall()//called too many times
    {
        print("new call");

        GameManager_RepairHazards selectNewHazard = hazardManger_.GetComponent<GameManager_RepairHazards>();

        selectNewHazard.SelectHazard();//calls new hazard
    }


    public void fireHazard(bool activateCharger)//called too many times
    {
        ParticleSystem thisHazard_ = hazardToTrigger_.GetComponent<ParticleSystem>();
        var chargerBeam = thisHazard_.emission;


        chargerBeam.enabled = activateCharger;

        Invoke("StopHazard", hazardTime_);//stops hazard
    }

}
