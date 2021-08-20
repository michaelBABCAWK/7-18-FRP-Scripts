using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager_RepairTimer : MonoBehaviour
{
    public Text currentTime;
    


    public GameObject Header;
    public GameObject TimeLeft;
    public GameObject livesTracking;


    [SerializeField] private float minutes_;
    private float timeLeft_;



    public static bool gameStarted_ = false;
    bool outOfTime_;

    private void Awake()
    {
        timeLeft_ = minutes_ * 60 + 18;
        outOfTime_ = false;
    }

    private void Start()
    {
        Header.SetActive(false);

        TimeLeft.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (outOfTime_ == false)
        TrackingTime();
        else
        {
            return;
        }
    }




    private void TrackingTime()
    {
        if (gameStarted_)
        {
            timeLeft_ -= 1 * Time.deltaTime;

            string minutes = ((int)timeLeft_ / 60).ToString();
            string seconds = (timeLeft_ % 60).ToString("f2");

            currentTime.text = minutes + ":" + seconds;

            Header.SetActive(true);

            TimeLeft.SetActive(true);
        }

        if(timeLeft_ <= 0)
        {

            GameManager_AllSectionCompletionTrackers resetButton = GetComponent<GameManager_AllSectionCompletionTrackers>();

            resetButton.resetAllStats();

            //triggger event here
            UI_ForcedFlightMenu forcedFlightMenu = GetComponent<UI_ForcedFlightMenu>();

            forcedFlightMenu.TurnOnMenu();



            //canvas with options
            outOfTime_ = true;
        }
    }
}
