using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles_LevelHazard_LightTelegraph : MonoBehaviour
{
    public GameObject light_;


    public float telegraphTime_;
    float origTeleTime_;
    public float minTime_;
    public float maxTime_;
    float timer_;


    bool selected_;

    private void Awake()
    {
        origTeleTime_ = telegraphTime_;//ref to return tele-time
        selected_ = false;//do not start the telegraph times
        flashTimerSet();//initial call for flash timer within telegraph timer
    }

    public void On()//telegrpah on//CALLED BY THE repair hazard manager if selected to enable countdown
    {
        selected_ = true;
    }

    public void Off()//telegraph off
    {

        selected_ = false;
        telegraphTime_ = origTeleTime_;//resets tele-time
        flashTimerSet();//new flash timer
    }

    public void flashTimerSet()
    {
        timer_ = Random.Range(minTime_, maxTime_);//called once before Off is called
    }

    public void hazardTelegraph()
    {

        if (telegraphTime_ > 0)//counts down while time left
        {
            FlickerControl();

            telegraphTime_ -= .02f;//when 0 the hazard triggers

        }
        else
        {
            HazardAfterTelegraph();//triggers when telegraphTime_ is zero 
            Off();
        }
    }

    private void FlickerControl()
    {
        if (timer_ > 0)
        {
            light_.SetActive(false);
        }
        if (timer_ <= 0)

        {
            light_.SetActive(true);

            flashTimerSet();
        }

        timer_ -= .05f;//counterdown for light to flicker on and off
    }

    private void HazardAfterTelegraph()
    {
        Obstacles_LevelHazard_ControlHazard thisHazardTrigger_ = gameObject.GetComponent<Obstacles_LevelHazard_ControlHazard>();



        thisHazardTrigger_.fireHazard(true);
    }

    private void Update()
    {
        if(selected_ == true)
        {
            hazardTelegraph();//counting down while selected is true

        }
        else
        {

        }
    }
}
