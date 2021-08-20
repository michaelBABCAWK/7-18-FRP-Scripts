using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager_ShieldObstacleActivation : MonoBehaviour
{
    public Material[] currentColor;

    Renderer finishColor;

    public GameObject Obstacle1;
    Renderer Obstacle1Render;
    public GameObject Obstacle2;


    public GameObject Obstacle3;
    Renderer Obstacle3Render;
    public GameObject Obstacle4;


    public GameObject Obstacle5;
    Renderer Obstacle5Render;
    public GameObject Obstacle6;


    public GameObject Obstacle7;
    Renderer Obstacle7Render;
    public GameObject Obstacle8;



    [SerializeField] GameObject CompleteImage;


    [SerializeField] GameObject finish;

    bool primerReady;

    [HideInInspector] public static int objWins;

    [HideInInspector] public static int completionFactor = 1;

    private void CheckForDuplicates()
    {
        int numberOfFinishes = FindObjectsOfType<GameManager_ShieldObstacleActivation>().Length;

        if (numberOfFinishes > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        CheckForDuplicates();

        SetFinishComponents();

        CompletionMessage();

        trackingCylindersActivation();

        ActivatingSparks();

        completionFactorUpdate();
    }

    private void SetFinishComponents()
    {
        finishColor = GetComponent<Renderer>();

        //Variable starts at zero and updates as completions from the source change.
        //varaiable is tracked in theAllSectionTrackerScript as well.
        objWins = GameManager_AllSectionCompletionTrackers.ShieldCompeltions;
    }

    private void CompletionMessage()
    {
        if (completionFactor == 4)
        {
            CompleteImage.SetActive(true);

            Destroy(CompleteImage, 10f);
        }
    }

    private void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                increaseObjWins();
                callManageTimer();
                break;
            default:
                break;
        }
    }

    private static void callManageTimer()
    {
        GameObject primerObject;
        Objective_Primer manageTimer;

        primerObject = GameObject.Find("Primer");
        manageTimer = primerObject.GetComponent<Objective_Primer>();

        manageTimer.ManageTimer();
    }

    void increaseObjWins()
    {
        GameObject livesTracker;
        livesTracker = GameObject.Find("Game Manager");
        GameManager_AllSectionCompletionTrackers updateTracker;
        updateTracker = livesTracker.GetComponent<GameManager_AllSectionCompletionTrackers>();
        objWins += 1;
        updateTracker.SetObjWins();
    }


    private void CheckingForPrimed()
    {
        primerReady = Objective_Primer.primed;
        if (primerReady == true)
        {
            finishColor.sharedMaterial = currentColor[1];
            gameObject.tag = "Finish";
        }
        if (primerReady == false)
        {
            finishColor.sharedMaterial = currentColor[0];
            gameObject.tag = "Untagged";
        }
    }

    private void ActivatingSparks()
    {
        if (objWins > 1)
        {
            Obstacle2.SetActive(true);
        }
        if (objWins > 3)
        {
            Obstacle4.SetActive(true);
        }
        if (objWins > 5)
        {
            Obstacle6.SetActive(true);

        }
        if (objWins > 7)
        {
            Obstacle8.SetActive(true);
        }
    }



    private void completionFactorUpdate()//Increases completion Factor
    {
        if (objWins >= 4)
        {
            completionFactor = 2;
        }
        if (objWins >= 6)
        {
            completionFactor = 3;
        }
        if (objWins >= 8)
        {
            completionFactor = 4;
        }
    }

    private void trackingCylindersActivation()
    {
        Obstacles_Oscillate toMove;


        if (objWins > 0)
        {
            Obstacle1Render = Obstacle1.GetComponent<Renderer>();
            Obstacle1Render.sharedMaterial = currentColor[2];


            //Enable oscilate script here.
            toMove = Obstacle1.GetComponent<Obstacles_Oscillate>();
            toMove.activateOscillate();
            Obstacle1.tag = "Deadly";
        }
        if (objWins > 2)
        {
            Obstacle3Render = Obstacle3.GetComponent<Renderer>();
            Obstacle3Render.sharedMaterial = currentColor[2];

            Obstacle3.GetComponent<Obstacles_Oscillate>().activateOscillate();
            Obstacle3.tag = "Deadly";
        }

        if (objWins > 4)
        {
            Obstacle5Render = Obstacle5.GetComponent<Renderer>();
            Obstacle5Render.sharedMaterial = currentColor[2];

            Obstacle5.GetComponent<Obstacles_Oscillate>().activateOscillate();
            Obstacle5.tag = "Deadly";
        }

        if (objWins > 6)
        {
            Obstacle7Render = Obstacle7.GetComponent<Renderer>();
            Obstacle7Render.sharedMaterial = currentColor[2];

            Obstacle7.GetComponent<Obstacles_Oscillate>().activateOscillate();
            Obstacle7.tag = "Deadly";
        }

    }
}
