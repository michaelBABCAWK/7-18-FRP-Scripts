using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public class Player_Pause : MonoBehaviour
{
    [SerializeField] GameObject pauseScreen_;
    bool paused_ = false;

    GameObject playerRef_;
    Player_ControlShip unPausePlayer_;


    private void Start()
    {
        playerRef_ = GameObject.Find("Player");
        unPausePlayer_ = playerRef_.GetComponent<Player_ControlShip>();

    }





    public void Continue()
    {
        Time.timeScale = 1;
        pauseScreen_.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        print("Pause");
        paused_ = false;

    }

    public void Pause()
    {
        Time.timeScale = 0;
        pauseScreen_.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        paused_ = true;

        print("Pause");
    }

    public void Quit()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        if (unPausePlayer_.paused == false)
        {
            if (CrossPlatformInputManager.GetButtonDown("Pause") && !paused_ || Input.GetKeyDown(KeyCode.P) && !paused_)
            {
                Pause();
            }
            if (CrossPlatformInputManager.GetButtonDown("Pause") && paused_ || Input.GetKeyDown(KeyCode.P) && paused_)
            {
                //Continue();
            }
        }

    }
}
