using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Flight_InstructionPages : MonoBehaviour
{
    [Header("Instuctions")]
    public GameObject[] instructionPages_;
    public GameObject UiAfterInstructions_;


    [Header("Music Tracks")]
    public GameObject tutorialTrack_;
    public GameObject fightTrack_;

    public static bool launched_;


    GameObject instructionsCanvasRef_;
    GameObject playerRef_;
    LoseCondition_TimeToLoseLevel refToTimerScript_;


    Player_ControlShip unPausePlayer_;


    int currentPage = 0;//current page active

    private void Start()
    {
        SetReferences();

        Time.timeScale = 0f;//starts time for game


        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void SetReferences()
    {
        SetPlayerRefs();

    }

    private void SetPlayerRefs()
    {
        playerRef_ = GameObject.Find("Player");
        unPausePlayer_ = playerRef_.GetComponent<Player_ControlShip>();
        launched_ = false;
    }


    private static void Mixer_FlightSection()
    {
        GameObject managerObject_ = GameObject.Find("Mixer Object");
        Mixer_FlightSection _flightSectionMix;
        _flightSectionMix = managerObject_.GetComponent<Mixer_FlightSection>();
        _flightSectionMix.Snapshot_FlightSection();
    }

    public void NextInstructionPage()
    {
        if (currentPage == instructionPages_.Length - 1)//if
        {

            InstructionsOver();


            Mixer_FlightSection();
        }

        else
        {
            instructionPages_[currentPage].SetActive(false);//turn off current page

            currentPage += 1;//assign new page

            instructionPages_[currentPage].SetActive(true);//turn on new page

        }


    }

    public void InstructionsOver()
    {
        tutorialTrack_.SetActive(false);
        fightTrack_.SetActive(true);
        UiAfterInstructions_.SetActive(true);

       // SetInstructionsCanvassRef();
        unPausePlayer_.notPuased();//unpause player

        Destroy(gameObject);//kills this


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    public void LastInstructionPage()
    {
        if (currentPage == 0)
        {
            print("nothing back there");
        }

        else
        {
            instructionPages_[currentPage].SetActive(false);//turn off current page

            currentPage -= 1;//assign new page

            instructionPages_[currentPage].SetActive(true);//turn on new page
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
