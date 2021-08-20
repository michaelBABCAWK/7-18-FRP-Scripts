using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_tutorialManager : MonoBehaviour
{
    RepairDrone_Movement activateObstacle;

    public int wins;

    public GameObject Obstacle;

    bool IsMoving;
    // Start is called before the first frame update
    void Start()
    {
        //triggers check for completion.
        activateObstacle = GameObject.FindObjectOfType<RepairDrone_Movement>();

        //bool used in update
        IsMoving = RepairDrone_Movement.TutorialFinishStatus;

        //keeps from destroying until going back to main menu.
        DontDestroyOnLoad(gameObject);
    }


}
