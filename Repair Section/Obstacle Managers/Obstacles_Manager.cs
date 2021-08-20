using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles_Manager : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        CheckForDuplicates();
    }

    private void CheckForDuplicates()
    {
        int numberOfFinishes = FindObjectsOfType<Obstacles_Manager>().Length;

        if (numberOfFinishes > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
