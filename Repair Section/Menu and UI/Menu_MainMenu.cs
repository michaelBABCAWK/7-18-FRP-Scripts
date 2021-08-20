using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_MainMenu : MonoBehaviour
{


    public GameObject[] instructionPages;
    public GameObject StartButton;
    public GameObject nextButton;
    public GameObject lastButton;
    [HideInInspector] public GameObject tutorialTracker;
    public GameObject livesTracker;
    public GameObject mainMenuMusicObject_;
    public GameObject repairTermMusicObject_;

    int instructionPage;

    [Header("Audio")]
    public AudioSource selectSoundSource_;

    private void Awake()
    {
        instructionPage = 0;

        Time.timeScale = 1f;

    }

    private void Start()
    {
        GameManager_RepairTimer.gameStarted_ = false;
        
        //tutorialTracker
    }

    // Start is called before the first frame update
    public void StartGame()
    {
        //stop main menu sounds
        //start reper music sounds



        //DestroyImmediate(tutorialTracker, true);
        SceneManager.LoadScene("Terminal", LoadSceneMode.Single);
        GameManager_RepairTimer.gameStarted_ = true;

        TerminalSoundMix();


        SetAudioPlayers();

    }

    private static void TerminalSoundMix()
    {
        GameObject _managerObject = GameObject.Find("Game Manager");
        Mixer_RepairSection _startTerminalMix;
        _startTerminalMix = _managerObject.GetComponentInChildren<Mixer_RepairSection>();
        _startTerminalMix.TerminalSnapshot();
    }

    private void SetAudioPlayers()
    {
        mainMenuMusicObject_.SetActive(false);
        repairTermMusicObject_.SetActive(true);
    }

    public void StartInstructions()
    {
        instructionPage = 0;

        instructionPages[instructionPage].SetActive(true);

        nextButton.SetActive(true);

        lastButton.SetActive(true);
    }

    public void tutorial()
    {
        selectSoundSource_.PlayOneShot(selectSoundSource_.clip);

        SceneManager.LoadScene(7, LoadSceneMode.Single);//7 is first tutorial scene in buiild order

    }

    public void nextPage()
    {
        if(instructionPage == instructionPages.Length -1)
        {
            instructionPages[instructionPage].SetActive(false);
            nextButton.SetActive(false);
            lastButton.SetActive(false);
        }
        else
        {
            instructionPage = instructionPage + 1;
            instructionPages[instructionPage - 1].SetActive(false);
            instructionPages[instructionPage].SetActive(true);
        }
    }

    public void lastPage()
    {
        if (instructionPage == 0)
        {
            instructionPages[instructionPage].SetActive(false);
            nextButton.SetActive(false);
            lastButton.SetActive(false);
        }
        else
        {
            instructionPage = instructionPage - 1;
            instructionPages[instructionPage + 1].SetActive(false);
            instructionPages[instructionPage].SetActive(true);
        }
    }
}
