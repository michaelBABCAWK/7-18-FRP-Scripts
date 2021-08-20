using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RepairDrone_RestartPort : MonoBehaviour
{
    string sceneLoaded;

    GameObject livesTracking;
    GameObject finish;

    // Start is called before the first frame update
    void Start()
    {
        SetVariables();

        SetFinish();

    }

    private void SetVariables()
    {
        livesTracking = GameObject.Find("Game Manager");

        sceneLoaded = GameManager_Terminal_Buttons.levelLoaded;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.name == "Restart Port")
        {
            RestartRoutine();
        }
    }


    private void SetFinish()
    {

        if (GameManager_Terminal_Buttons.levelLoaded == "Cannons")
        {
            finish = GameObject.Find("Finish Cannons");
        }
        if (GameManager_Terminal_Buttons.levelLoaded == "Boosters")
        {
            finish = GameObject.Find("Finish Boosters");
        }
        if (GameManager_Terminal_Buttons.levelLoaded == "Shields")
        {
            finish = GameObject.Find("Finish Shields");
        }
    }

    private void RestartRoutine()//completing repair cycles
    {
        SceneManager.LoadScene(sceneLoaded, LoadSceneMode.Single);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
