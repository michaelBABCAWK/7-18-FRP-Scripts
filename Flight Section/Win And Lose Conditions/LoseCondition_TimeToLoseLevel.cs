using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoseCondition_TimeToLoseLevel : MonoBehaviour
{
    [SerializeField] public float MinutesUntilEnd_;
    private float timeLeft_;

    public Text currentTime_;

    bool launched_;

    GameObject playerRef_;

    Player_ControlShip playerMovementRef_;

    public GameObject[] timesUpOptions_;


    private void Start()
    {
        //refToStartingTutorial = Menu_FlightTutorial;

        SetInitialComponent();


        PlayerReferences();
    }

    private void PlayerReferences()
    {
        playerRef_ = GameObject.Find("Player");

        playerMovementRef_ = playerRef_.GetComponent<Player_ControlShip>();
    }

    private void SetInitialComponent()
    {
        timeLeft_ = MinutesUntilEnd_ * 60; // Magic # is 60 so the result is in minutes.

        launched_ = true;
        //launched_ = false;
    }

    // Update is called once per frame
    void Update()
    {
        TrackingTime();
    }

    public void activateTimer()
    {
        launched_ = true;
    }

    private void TrackingTime()
    {
        //launched_ = Menu_FlightTutorial.launched_;

        if (launched_)// Activate Timer after instructions pages are gone.
        {

            timeLeft_ -= 1 * Time.deltaTime;

            string minutes = ((int)timeLeft_ / 60).ToString();
            string seconds = (timeLeft_ % 60).ToString("f2");

            currentTime_.text = minutes + ":" + seconds;
        }

        if (timeLeft_ <= 0)
        {
            for(int i = 0; i < timesUpOptions_.Length; i++)
            {
                timesUpOptions_[i].SetActive(true);
            }

            Time.timeScale = 0f;

            playerMovementRef_.RanOutOfTime();//Calls method on player that stops movement
            //Destroy(gameObject);
            //SceneManager.LoadScene("Game Over");
           //ADD buttons to instruction pages to scroll through neccessary info before flight section starts.
        }
    }
}
