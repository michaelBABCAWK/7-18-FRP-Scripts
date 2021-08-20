using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager_Terminal_Buttons : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("Terminal Menus")]
    //public GameObject MainMenuUI_;
    public GameObject cannonsSummary_;
    public GameObject boosterSummary_;
    public GameObject shieldSummary_;
    public GameObject flightSummary_;

    /// <summary>
    /// These valkues below are holdovers which are used to track what is loaded in various systems from tutorial period
    /// </summary>
    public static string levelLoaded;
    public static int SceneLoaded;

    public void MainMenu()
    {
        cannonsSummary_.SetActive(false);
        boosterSummary_.SetActive(false);
        shieldSummary_.SetActive(false);
        flightSummary_.SetActive(false);

        //MainMenuUI_.SetActive(true);
    }

    public void CannonSummary()
    {
        cannonsSummary_.SetActive(true);

    }


    public void BooseterSummary()
    {
        boosterSummary_.SetActive(true);


    }

    public void ShieldSummary()
    {
        shieldSummary_.SetActive(true);


    }

    public void FlightSummary()
    {
        flightSummary_.SetActive(true);
    }

    public void LoadCannons()
    {
        SceneManager.LoadScene("Cannons", LoadSceneMode.Single);
        levelLoaded = "Cannons";
        SceneLoaded = 1;
    }



    public void LoadBooosters()
    {
        SceneManager.LoadScene("Boosters", LoadSceneMode.Single);
        levelLoaded = "Boosters";
        SceneLoaded = 2;
    }



    public void LoadShields()
    {
        SceneManager.LoadScene("Shields", LoadSceneMode.Single);
        levelLoaded = "Shields";
        SceneLoaded = 3;
    }

    public void StartFlight()
    {
        SceneManager.LoadScene("Final Flight", LoadSceneMode.Single);

        GameObject _destroyManager = GameObject.Find("Game Manager");

        Destroy(_destroyManager);
    }

    // Update is called once per frame
    void Update()
    {

    }
}


