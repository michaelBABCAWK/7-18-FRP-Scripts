using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terminal_PauseGame : MonoBehaviour
{

    bool paused_ = false;

    // Update is called once per frame
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(paused_ == false)
            {
                TurnOnMenu();
            }
            else if (paused_ == true)
            {
                TurnOffMenu();
            }
        }


    }

    private void TurnOnMenu()
    {
        Time.timeScale = 0;

        paused_ = true;

        for (int i = 0; i < transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    private void TurnOffMenu()
    {
        Time.timeScale = 1;

        paused_ = false;

        for (int i = 0; i < transform.childCount; i++)
        {
            gameObject.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
