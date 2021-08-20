using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_TrackingCompletionsShield : MonoBehaviour
{
    public int completions;

    // Start is called before the first frame update
    void Start()
    {
        completions = GameManager_AllSectionCompletionTrackers.ShieldCompeltions;
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                completions += 1;
                break;
            default:
                break;
        }
    }
}
