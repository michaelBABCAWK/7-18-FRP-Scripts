using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_RepairHazards : MonoBehaviour
{

    /// <summary>
    /// //event system here
    /// </summary>
    /// 

    public GameObject[] hazardInLevel;
    GameObject currentHazard_;
    GameObject newHazard_;//cannot == currenthazard
    GameObject lastHazard_;

    [Header("Hazard Values")]

    public float timeBetweenHazards_;

    // Start is called before the first frame update

    private void Start()
    {
        InvokeHazard();
    }

    public void SelectHazard()//will be an enumerator
    {
        //hazard select process
        newHazard_ = hazardInLevel[(Random.Range(0, hazardInLevel.Length))];



        if(newHazard_ != lastHazard_)
        {
            lastHazard_ = newHazard_;
            RepairHazard_HazardActivate newHazardActivate_ = newHazard_.GetComponent<RepairHazard_HazardActivate>();
            newHazardActivate_.HasBeenChosen();
        }
        else if (lastHazard_ == newHazard_)
        {
            SelectHazard();
        }


    }

        //activate the hazard
    public void InvokeHazard()
    {
        Invoke("SelectHazard", timeBetweenHazards_);
    }
}
