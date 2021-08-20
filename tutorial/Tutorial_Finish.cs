using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial_Finish : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {

        switch (collision.gameObject.name)
        {
            case "Player":
                SceneManager.LoadScene(0, LoadSceneMode.Single);
                break;
            default:
                break;
        }
    }
}
