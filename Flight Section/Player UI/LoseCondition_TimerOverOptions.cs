using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseCondition_TimerOverOptions : MonoBehaviour
{
    // Start is called before the first frame update
    public void RestartWholeGame()
    {
        SceneManager.LoadScene("Main Menu");
        
        //gameObject.GetComponent<GameManager_AllSectionCompletionTrackers>().everythingZeroOnWins();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("Final Flight");
    }

    public void EndGame()
    {
        Application.Quit();//Will only work in build of game
    }
}
