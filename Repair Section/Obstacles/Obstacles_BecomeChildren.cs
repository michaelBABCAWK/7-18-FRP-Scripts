using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles_BecomeChildren : MonoBehaviour
{

    public GameObject finishParent;

    Obstacles_Oscillate movementClass;

    private void Start()
    {
        /*
        movementClass = GetComponent<Obstacles_Oscillate>();

        if(movementClass != null)
        {
            movementClass.enabled = false;
        }
        */
        gameObject.transform.parent = finishParent.transform;
    }

}
