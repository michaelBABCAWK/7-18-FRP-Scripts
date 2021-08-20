using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RepairSection_TerminalDisplay : MonoBehaviour { }
/*
{



    /// <summary>
    /// split up lives tracking and level loading 
    /// </summary>
    GameObject MainMenuUI;

    GameObject livesTracking;

    GameManager_LivesTracker drones;

    int livesFromLives;

    //public GameObject Instructions;

    //Canvas livesTimerCanvas;

    [SerializeField] public float cannonRepair = 25f;
    [SerializeField] public float shieldStength = 25f;
    [SerializeField] public float enginePower = 25f;

    //Delayed Load time.
    public float delayTime = 10.0f;

    //enum,is establishing a new type called Screen. Which can now create variables of the new type. Type created = Screen
    enum Screen { MainMenu, PasswordForRepair, SectionToRepairr, FailedAccess, prepareLaunch, forcedFlight };

    //Variable below will be used to store Gamestate.ff
    Screen currentScreen;

    string menuHint = "Type menu to return to menu.";

    // Level loaded from player input in Main Menu.
    public static int SceneLoaded;

    //Level script will look for to load for repair sections.
    public static string levelLoaded;

    // will be set to one word on one of the arrays depending on player input.
    string currentPassword;

    // Condition of ship systems.
    string condition;
    //string dronesLeft;

    float livesFromLives;


    // Start is called before the first frame update
    void Start()
    {
        livesTracking_ = GameObject.Find("Game Manager");

        MainMenuUI_ = GameObject.Find("Pause Menu");

        drones_ = livesTracking_.GetComponent<GameManager_LivesTracker>();

        livesFromLives = drones_.dronesLeft_;

        //OptionsMenu();
    }

    void StatusReport()
    {
        Terminal.ClearScreen();

        Terminal.WriteLine("Progress: Cannon Status: " + cannonRepair * GameManager_CannonObstacleActivation.completionFactor + " / 100");

        Terminal.WriteLine("Progress: " + enginePower * GameManager_BoostersObstacleActivation.completionFactor + " / 100");

        Terminal.WriteLine("Progress: " + shieldStength * GameManager_ShieldObstacleActivation.completionFactor + " / 100");

        Invoke("OptionsMenu", delayTime);
    }

    void OptionsMenu()
    {
        currentScreen = Screen.MainMenu;

        Terminal.ClearScreen();
        /*
        Terminal.WriteLine("Type option then hit enter.");

        */
/*
        Terminal.WriteLine("Unstable levels." +
            " If attempting repairs:" +
            "Watch for venting");
        
        Terminal.WriteLine("");

        Terminal.WriteLine("Choose Ship Section for Maintenence");
        Terminal.WriteLine("1 Cannons Status:" + cannonRepair * GameManager_CannonObstacleActivation.completionFactor + "/ 100% easy");

        Terminal.WriteLine("2 Booster Power:" + enginePower * GameManager_BoostersObstacleActivation.completionFactor + " / 100% med");

        Terminal.WriteLine("3 Shield Strength:" + shieldStength * GameManager_ShieldObstacleActivation.completionFactor + " / 100% hard");

        Terminal.WriteLine("4 to Launch Ship (Kinda Finished)");
        Terminal.WriteLine("Drones left: " + livesFromLives.ToString() + " / 32");


        Terminal.WriteLine("Type Option");
    }


    private void LaunchThisBitch(string input)
    {
        if (input == "y" || input == "Y")
        {
            SceneManager.LoadScene("Final Flight");
            Destroy(livesTracking);
        }
        else if (input == "n" || input == "N")
        {
            OptionsMenu();
        }
    }
}

*/