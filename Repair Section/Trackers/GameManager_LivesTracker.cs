using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager_LivesTracker : MonoBehaviour
{
    [SerializeField] public int dronesLeft_ = 32;

    private void Start()
    {


        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {

    }




    public void ReduceDrones()//Seperate methods that can be called depending on circumstance of cycle complete or drone death
    {
        dronesLeft_ -= 1;


        if (dronesLeft_ == 0)
        {


            GameManager_AllSectionCompletionTrackers resetButton = GetComponent<GameManager_AllSectionCompletionTrackers>();

            resetButton.resetAllStats();


            UI_ForcedFlightMenu forcedFlightMenu = GetComponent<UI_ForcedFlightMenu>();

            forcedFlightMenu.TurnOnMenu();





            // Set ui that has button to go to flight only terminal


        }
    }


}
