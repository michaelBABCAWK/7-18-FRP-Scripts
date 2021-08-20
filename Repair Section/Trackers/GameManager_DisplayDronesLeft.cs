using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager_DisplayDronesLeft : MonoBehaviour
{
    int remainingLives;

    public Text displayLives;

    GameManager_LivesTracker livesTracker;

    // Start is called before the first frame update
    void Start()
    {
        establishLives();

        droneCounter();
    }

    private void establishLives()
    {
        livesTracker = FindObjectOfType<GameManager_LivesTracker>();

        remainingLives = livesTracker.dronesLeft_;
    }

    private void droneCounter()
    {
        displayLives.text = remainingLives.ToString();
    }
}
