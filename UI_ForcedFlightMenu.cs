using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;


public class UI_ForcedFlightMenu : MonoBehaviour
{
    GameObject finish_;

    public GameObject[] CanvasButtons_;

    public void ReturnToFlightOnlyTerminal()
    {


        ResetStatsCall();



        SceneManager.LoadScene("Launch Flight Terminal");

        if (GameManager_Terminal_Buttons.SceneLoaded == 1)
        {
            finish_ = GameObject.Find("Finish Cannons");
            Destroy(finish_);
            DestroyNonDestroyObjects();


        }
        if (GameManager_Terminal_Buttons.SceneLoaded == 2)
        {
            finish_ = GameObject.Find("Finish Boosters");
            Destroy(finish_);

            DestroyNonDestroyObjects();
        }
        if (GameManager_Terminal_Buttons.SceneLoaded == 3)
        {
            finish_ = GameObject.Find("Finish Shields");
            Destroy(finish_);

            DestroyNonDestroyObjects();
        }
    }

    public void RestartGame()
    {
        ResetStatsCall();

        SceneManager.LoadScene("Main Menu");
        Destroy(gameObject);


    }

    private void DestroyNonDestroyObjects()
    {
        Destroy(finish_);
        Destroy(gameObject);

        GameObject[] gos = GameObject.FindGameObjectsWithTag("Untagged");
        foreach (GameObject go in gos)
            Destroy(go);

        GameObject[] gos2 = GameObject.FindGameObjectsWithTag("Deadly");
        foreach (GameObject go in gos)
            Destroy(go);


    }

    public void ResetStatsCall()
    {
        GameManager_AllSectionCompletionTrackers resetButton = GetComponent<GameManager_AllSectionCompletionTrackers>();

        resetButton.resetAllStats();
    }

    public void TurnOnMenu()
    {
        for (int i = 0; i < CanvasButtons_.Length; i++)
        {
            CanvasButtons_[i].SetActive(true);
        }
    }
}
