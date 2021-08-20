using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager_ManagerScript : MonoBehaviour 
{
    public delegate void MissleEvent();
    public static MissleEvent onBossMissleFire;
    
    public static void MyEvent()
    {
        if(onBossMissleFire != null)    onBossMissleFire();
    }

}
