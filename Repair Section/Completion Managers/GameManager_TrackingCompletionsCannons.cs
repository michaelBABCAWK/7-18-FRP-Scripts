using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_TrackingCompletionsCannons : MonoBehaviour
{
    public int completions;
    //THIS SCRIPT and others like it maybe unnecessary. But I made it because the obstacle activation scripts were getting long.

    // Start is called before the first frame update
    void Start()
    {
        //Starts Value at zero.
        completions = GameManager_AllSectionCompletionTrackers.CannonsCompletions;
    }
}
