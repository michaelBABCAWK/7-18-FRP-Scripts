using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_AllSectionCompletionTrackers : MonoBehaviour
{
    public static int BoostersCompletions;

    public static int CannonsCompletions;

    public static int ShieldCompeltions;

    Objective_Primer _callIncreaseTimer;
    GameObject _PrimerObject;

    private void Start()
    {
        _PrimerObject = GameObject.Find("Prmer");
        //everythingZeroOnWins();
    }

    public void SetObjWins()
    {
        BoostersCompletions = GameManager_BoostersObstacleActivation.objWins;

        CannonsCompletions = GameManager_CannonObstacleActivation.objWins;

        ShieldCompeltions = GameManager_ShieldObstacleActivation.objWins;

        InitiateTimer();
    }

    public void resetAllStats()
    {
        print("Called reset");

        BoostersCompletions = GameManager_BoostersObstacleActivation.objWins = 0;//displayed in each section during repair as Repair Cycles

        CannonsCompletions = GameManager_CannonObstacleActivation.objWins = 0;//displayed in each section during repair as Repair Cycles

        ShieldCompeltions = GameManager_ShieldObstacleActivation.objWins = 0;//displayed in each section during repair as Repair Cycles

        GameManager_BoostersObstacleActivation.completionFactor = 1;

        GameManager_CannonObstacleActivation.completionFactor = 1;

        GameManager_ShieldObstacleActivation.completionFactor = 1;
    }

    private void InitiateTimer()
    {
        if (_PrimerObject != null)
        {
            _callIncreaseTimer = _PrimerObject.GetComponent<Objective_Primer>();
            _callIncreaseTimer.ManageTimer();
        }
    }

}
