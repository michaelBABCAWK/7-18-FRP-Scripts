using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DisplayCompletions : MonoBehaviour
{
    //Number of completions from Finish.
    int currentNumberOfCompletions;

    //Text object used for displaying.
    public Text displayCompletions;

    public GameObject completionMessage;

    public GameObject totalNeeded;

    // Start is called before the first frame update
    void Start()
    {
        SetSCore();
    }

    private void SetSCore()
    {
        if (GameManager_Terminal_Buttons.levelLoaded == "Cannons")
        {
            currentNumberOfCompletions = GameManager_CannonObstacleActivation.objWins;
            if(currentNumberOfCompletions == 8)
            {
                completionMessage.SetActive(true);
                totalNeeded.SetActive(false);
                displayCompletions.enabled = false;
            }

            //Text component of text object.
            displayCompletions.text = currentNumberOfCompletions.ToString();
        }
        if (GameManager_Terminal_Buttons.levelLoaded == "Boosters")
        {
            currentNumberOfCompletions = GameManager_BoostersObstacleActivation.objWins;

            if (currentNumberOfCompletions == 8)
            {
                completionMessage.SetActive(true);
                totalNeeded.SetActive(false);
                displayCompletions.enabled = false;
            }

            //Text component of text object.
            displayCompletions.text = currentNumberOfCompletions.ToString();
        }
        if (GameManager_Terminal_Buttons.levelLoaded == "Shields")
        {
            currentNumberOfCompletions = GameManager_ShieldObstacleActivation.objWins;

            if (currentNumberOfCompletions >= 8)
            {
                completionMessage.SetActive(true);
                totalNeeded.SetActive(false);
                displayCompletions.enabled = false;
            }

            //Text component of text object.
            displayCompletions.text = currentNumberOfCompletions.ToString();
        }
    }
}
