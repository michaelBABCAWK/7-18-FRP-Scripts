using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles_CylinderActivateMovement : MonoBehaviour
{
    Obstacles_Oscillate oscillateScript;

    private void Start()
    {
        oscillateScript = GetComponent<Obstacles_Oscillate>();
    }

    public void activateOscillate()
    {
        oscillateScript.enabled = true;
    }
}
