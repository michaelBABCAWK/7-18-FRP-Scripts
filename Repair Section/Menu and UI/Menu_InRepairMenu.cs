using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu_InRepairMenu : MonoBehaviour
{
    public static bool GameIsPaused_ = false;

    [SerializeField] GameObject[] pauseImages_;

    [SerializeField] GameObject[] buttonsPanel_;

    GameObject tutFinish_;

    GameObject tutTracker_;

    GameObject finish_;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (GameIsPaused_)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Pause()
    {

        for (int i = 0; i < pauseImages_.Length; i++)
        {
            pauseImages_[i].SetActive(true);
        }

        for (int i = 0; i < buttonsPanel_.Length; i++)
        {
            buttonsPanel_[i].SetActive(true);
        }

        Time.timeScale = 0f;

        GameIsPaused_ = true;
    }

    public void backToMain()
    {
        HandleTutorialReturn();

        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);

        Time.timeScale = 1f;
    }

    private void HandleTutorialReturn()
    {
        tutTracker_ = GameObject.Find("Tut Tracker");

        tutFinish_ = GameObject.Find("Finish Tutorial");

        Destroy(tutTracker_);

        Destroy(tutFinish_);
    }

    public void Resume()
    {

        for (int i = 0; i < pauseImages_.Length; i++)
        {
            pauseImages_[i].SetActive(false);
        }

        for (int i = 0; i < buttonsPanel_.Length; i++)
        {
            buttonsPanel_[i].SetActive(false);
        }
        Time.timeScale = 1f;

        GameIsPaused_ = false;
    }

    private static void TerminalSoundMix()
    {
        GameObject managerObject_ = GameObject.Find("Game Manager");
        Mixer_RepairSection startTerminalMix_;
        startTerminalMix_ = managerObject_.GetComponentInChildren<Mixer_RepairSection>();
        startTerminalMix_.TerminalSnapshot();
    }

    public void ReturnToTerminal()
    {
        if (GameManager_Terminal_Buttons.levelLoaded == "Cannons")
        {
            finish_ = GameObject.Find("Finish Cannons");
            Destroy(finish_);
        }
        if (GameManager_Terminal_Buttons.levelLoaded == "Boosters")
        {
            finish_ = GameObject.Find("Finish Boosters");
            Destroy(finish_);
        }
        if (GameManager_Terminal_Buttons.levelLoaded == "Shields")
        {
            finish_ = GameObject.Find("Finish Shields");
            Destroy(finish_);
        }

        ResetCam();

        BackToTerminal();

        TerminalSoundMix();
    }

    private static void BackToTerminal()
    {
        Time.timeScale = 1f;

        GameIsPaused_ = false;

        SceneManager.LoadScene("Terminal", LoadSceneMode.Single);
    }

    private static void ResetCam()
    {
        RepairDrone_CameraControl _resetCam;
        GameObject _player = GameObject.Find("Player_RepairDrone");
        _resetCam = _player.GetComponent<RepairDrone_CameraControl>();
        _resetCam.ToMainMenu();
    }
}
