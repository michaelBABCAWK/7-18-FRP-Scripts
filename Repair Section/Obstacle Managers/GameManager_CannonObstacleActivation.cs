using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager_CannonObstacleActivation : MonoBehaviour
{
    public Material[] currentColor;

    Renderer finishColor;
    Renderer Obstacle1Render_;

    [Header("Obstacle GameObjects")]
    public GameObject Obstacle1_;
    public GameObject ObstacleRod1_;
    public GameObject LaserRack1_;
    //Renderer Obstacle1RodRender_;
    public GameObject LaserObstacle1;

    public GameObject Obstacle2_;
    Renderer Obstacle3Render;
    public GameObject ObstacleRod2_;
    public GameObject LaserRack2_;
    Renderer Obstacle2RodRender_;
    public GameObject LaserObstacle2;

    public GameObject Obstacle3;
    Renderer Obstacle5Render;
    public GameObject ObstacleRod3_;
    public GameObject LaserRack3_;
    Renderer Obstacle3RodRender_;
    public GameObject LaserObstacle3;

    public GameObject Obstacle4_;
    Renderer Obstacle7Render;
    public GameObject ObstacleRod4_;
    public GameObject LaserRack4_;
    Renderer Obstacle4RodRender_;
    public GameObject LaserObstacle4;

    [SerializeField] GameObject CompleteImage;

    [SerializeField] GameObject finish;



    [HideInInspector] public static int objWins;

    [HideInInspector] public static int completionFactor = 1;

    private void CheckForDuplicates()
    {
        int numberOfFinishes = FindObjectsOfType<GameManager_CannonObstacleActivation>().Length;

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
        //primerReady = Objective_Primer.primed;

        //Variable starts at zero and updates as completions from the source change.
        //varaiable is tracked in theAllSectionTrackerScript as well.
        objWins = GameManager_AllSectionCompletionTrackers.CannonsCompletions;
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
        GameObject gameManager_;
        gameManager_ = GameObject.Find("Game Manager");

        GameManager_AllSectionCompletionTrackers updateTracker;
        updateTracker = gameManager_.GetComponent<GameManager_AllSectionCompletionTrackers>();

        objWins += 1;
        updateTracker.SetObjWins();
    }

    public void PrimerOn()
    {
        finishColor.sharedMaterial = currentColor[1];
        gameObject.tag = "Finish";
    }

    public void PrimerOff()
    {
        finishColor.sharedMaterial = currentColor[0];
        gameObject.tag = "Untagged";
    }

    private void ActivatingSparks()
    {
        if (objWins > 1)
        {
            LaserObstacle1.SetActive(true);
        }
        if (objWins > 3)
        {
            LaserObstacle2.SetActive(true);
        }
        if (objWins > 5)
        {
            LaserObstacle3.SetActive(true);
        }
        if (objWins > 7)
        {
            LaserObstacle4.SetActive(true);
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

    public GameObject ObstacleColorChange(GameObject _obstacleToChange) 
    {
        Obstacle1Render_ = _obstacleToChange.GetComponent<Renderer>();
        Obstacle1Render_.sharedMaterial = currentColor[2];

        return _obstacleToChange;
    } 
    
    public GameObject RodColorChange(GameObject _rodToChange) 
    {
        Obstacle1Render_ = _rodToChange.GetComponent<Renderer>();
        Obstacle1Render_.sharedMaterial = currentColor[2];

        return _rodToChange;
    }

    public GameObject LaserRackColorChange(GameObject _laserToChange)
    {
        Obstacle1Render_ = _laserToChange.GetComponent<Renderer>();
        Obstacle1Render_.sharedMaterial = currentColor[2];

        return _laserToChange;

    }


    private void trackingCylindersActivation()
    {

        if (objWins > 0)
        {
            ObstacleColorChange(Obstacle1_);
            RodColorChange(ObstacleRod1_);
            LaserRackColorChange(LaserRack1_);

            //Enable oscilate script here.
            Obstacle1_.GetComponent<Obstacles_Oscillate>().activateOscillate();
            Obstacle1_.tag = "Deadly";
        }
        if (objWins > 2)
        {
            ObstacleColorChange(Obstacle2_);
            RodColorChange(ObstacleRod2_);
            LaserRackColorChange(LaserRack2_);

            Obstacle2_.GetComponent<Obstacles_Oscillate>().activateOscillate();
            Obstacle2_.tag = "Deadly";
        }
     
        if (objWins > 4)
        {
            ObstacleColorChange(Obstacle3);
            RodColorChange(ObstacleRod3_);
            LaserRackColorChange(LaserRack3_);

            Obstacle3.GetComponent<Obstacles_Oscillate>().activateOscillate();
            Obstacle3.tag = "Deadly";
        }

        if (objWins > 6)
        {
            ObstacleColorChange(Obstacle4_);
            RodColorChange(ObstacleRod4_);
            LaserRackColorChange(LaserRack4_);

            Obstacle4_.GetComponent<Obstacles_Oscillate>().activateOscillate();
            Obstacle4_.tag = "Deadly";
        }

    }
}
