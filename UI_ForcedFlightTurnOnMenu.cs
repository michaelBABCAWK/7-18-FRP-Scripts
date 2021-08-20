using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class UI_ForcedFlightTurnOnMenu : MonoBehaviour
{
    GameObject menuRef_;
    UI_ForcedFlightMenu turnOnMenu_;

    // Start is called before the first frame update
    void Start()
    {
        menuRef_ = GameObject.Find("Forced Flight Pause Screen");
        turnOnMenu_ = menuRef_.GetComponent<UI_ForcedFlightMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {

            turnOnMenu_.TurnOnMenu();
        }
    }
}
