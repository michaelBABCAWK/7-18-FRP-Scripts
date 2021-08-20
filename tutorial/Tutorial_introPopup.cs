using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_introPopup : MonoBehaviour
{
    private void Awake()
    {
        Time.timeScale = 0;

    }

    private void Update()
    {
    }

    public void startTutorialLevel()
    {
        Time.timeScale = 1;

        Destroy(gameObject);
    }
}
