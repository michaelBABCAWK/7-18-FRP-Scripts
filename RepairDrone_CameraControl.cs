using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairDrone_CameraControl : MonoBehaviour
{
    GameManager_CameraTracker updateSectionCameras_;


    GameObject manager_;
    public GameObject mainCamObject_;
    public GameObject noseCamObject_;


    [HideInInspector] public bool camObjectOverall_;

    private void Awake()
    {
        SetInitiaCamBool();


        MainCamSoundMix();


        SetCamera();
    }

    private void SetCamera()
    {
        if (camObjectOverall_ == true)
        {
            mainCamObject_.SetActive(true);
            noseCamObject_.SetActive(false);
            MainCamSoundMix();
        }
        else if (camObjectOverall_ == false)
        {
            mainCamObject_.SetActive(false);
            noseCamObject_.SetActive(true);
            NoseCamSoundMix();
        }
    }



    private void SetInitiaCamBool()
    {
        manager_ = GameObject.Find("Game Manager");

        updateSectionCameras_ = manager_.GetComponent<GameManager_CameraTracker>();

        if (GameManager_Terminal_Buttons.levelLoaded == "Cannons")
        {
            camObjectOverall_ = updateSectionCameras_.cannonOverall_;

        }
        if (GameManager_Terminal_Buttons.levelLoaded == "Boosters")
        {
            camObjectOverall_ = updateSectionCameras_.boosterOverall_;

        }
        if (GameManager_Terminal_Buttons.levelLoaded == "Shields")
        {
            camObjectOverall_ = updateSectionCameras_.shieldOverall_;

        }
    }



    private void UpdateCamBool()
    {
        manager_ = GameObject.Find("Game Manager");

        updateSectionCameras_ = manager_.GetComponent<GameManager_CameraTracker>();

        if (GameManager_Terminal_Buttons.levelLoaded == "Cannons")
        {
             updateSectionCameras_.cannonOverall_ = camObjectOverall_;
        }
        if (GameManager_Terminal_Buttons.levelLoaded == "Boosters")
        {
            updateSectionCameras_.boosterOverall_ = camObjectOverall_;
        }
        if (GameManager_Terminal_Buttons.levelLoaded == "Shields")
        {
            updateSectionCameras_.shieldOverall_ = camObjectOverall_;
        }
    }



    private void CamControl()
    {
        if (Input.GetKey(KeyCode.E))
        {
            noseCamObject_.SetActive(true);
            mainCamObject_.SetActive(false);

            camObjectOverall_ = false;//sets assigned bool from current levels camera

            UpdateCamBool();

            NoseCamSoundMix();
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            mainCamObject_.SetActive(true);
            noseCamObject_.SetActive(false);

            camObjectOverall_ = true;

            UpdateCamBool();

            MainCamSoundMix();
        }
    }


    private static void NoseCamSoundMix()
    {
        GameObject managerObject_ = GameObject.Find("Game Manager");
        Mixer_RepairSection startTerminalMix_;
        startTerminalMix_ = managerObject_.GetComponentInChildren<Mixer_RepairSection>();
        startTerminalMix_.NoseCamSnapshot();
    }

    private static void MainCamSoundMix()
    {
        GameObject managerObject_ = GameObject.Find("Game Manager");
        Mixer_RepairSection startMix_;
        startMix_ = managerObject_.GetComponentInChildren<Mixer_RepairSection>();
        startMix_.OverAllCamSnapshot();
    }




    public void ToMainMenu()
    {
        camObjectOverall_ = true;//resets the overall cam

        UpdateCamBool();
    }

    // Update is called once per frame
    void Update()
    {
        CamControl();
    }
}
