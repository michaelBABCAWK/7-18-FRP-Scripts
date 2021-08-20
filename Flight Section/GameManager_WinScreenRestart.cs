using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager_WinScreenRestart : MonoBehaviour
{
    public void Restart()//not reseting the section percentages
    {
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);//All gamemanagers destroyed at this point
    }
}

