using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectionLevel_TerminalOnlyFlight : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject MainMenuUI;

    GameObject livesTracking;


    // Start is called before the first frame update
    void Start()
    {
        livesTracking = GameObject.Find("LivesTracker");

        MainMenuUI = GameObject.Find("Pause Menu");
    }



    public void LaunchThisBitch()
    {
        Destroy(livesTracking);

        SceneManager.LoadScene("Final Flight");
    }

    public void Restart()//not reseting the section percentages
    {
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }
}
