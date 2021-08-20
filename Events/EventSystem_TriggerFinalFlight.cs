using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem_TriggerFinalFlight : MonoBehaviour
{
    public event EventHandler onTriggerForcedFlightMenu;

    public class OnForcedMenu: EventArgs
    {
        public int count;
    }

    // Start is called before the first frame update
    void Start()
    {
        onTriggerForcedFlightMenu += onTriggerForcedFlight;
    }

    private void onTriggerForcedFlight(object Sender, EventArgs e)
    {

    }

    // Update is called once per frame
    void Update()
    {


    }
}
