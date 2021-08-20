using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_activatePrimer : MonoBehaviour
{
    public Material[] currentColor;

    Renderer finishColor;

    //public GameObject obstacle;

    GameManager_tutorialManager winsFromDrone;

    Tutorial_Primer changeToGreen;

    RepairDrone_Movement didDroneActivate;

    public int trackingCycles = 1;

    bool activateObstacle;

    bool primerReady;

    //public bool obstacleOn = false;

    // Start is called before the first frame update
    void Start()
    {
        setVariables();
        //CheckForDuplicates();
    }

    private void setVariables()
    {
        //obstacle = GameObject.Find("Finish Tutorial");
        changeToGreen = FindObjectOfType<Tutorial_Primer>();

        winsFromDrone = FindObjectOfType<GameManager_tutorialManager>();

        didDroneActivate = FindObjectOfType<RepairDrone_Movement>();

        //trackingCycles = winsFromDrone.wins;

        activateObstacle = RepairDrone_Movement.TutorialFinishStatus;

        //obstacle = GameObject.Find("Finish Tutorial");

        finishColor = GetComponent<Renderer>();
    }


    // Update is called once per frame
    void Update()
    {
        //checkingForActivation();
        CheckingForPrimed();
    }

    private void CheckForDuplicates()
    {
        int numberOfFinishes = FindObjectsOfType<Tutorial_activatePrimer>().Length;

        if (numberOfFinishes > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }


    private void CheckingForPrimed()
    {
        primerReady = Tutorial_Primer.primed;

        //print(primerReady);

        if (primerReady == true)
        {
            finishColor.sharedMaterial = currentColor[1];
            gameObject.tag = "Finish Tutorial";
        }
        if (primerReady == false)
        {
            finishColor.sharedMaterial = currentColor[0];
            gameObject.tag = "Untagged";
        }

    }

    /*
    void checkingForActivation()
    {
        //trackingCycles = winsFromDrone.wins;

        if (trackingCycles >= 1)
        {
            //Enable oscilate script here.
            //activateObstacle no longer determined by drone status.
            //activateObstacle = true;
            obstacle.GetComponent<Oscillate>().enabled = true;
            obstacle.tag = "Deadly Tutorial";
        }
    }
    */
}
