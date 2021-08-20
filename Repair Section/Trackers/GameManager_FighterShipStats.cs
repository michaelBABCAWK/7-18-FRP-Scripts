using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_FighterShipStats : MonoBehaviour
{
    public static int CannonsStrength;
    int CannonsCompletion;

    public static int ShieldStrength;
    int ShieldCompletion;

    public static int EnginePower;
    int EngineCompletion;

    private void update()
    {
        CannonsCompletion = GameManager_CannonObstacleActivation.completionFactor;
        CannonsStrength = CannonsCompletion;
    }
}
