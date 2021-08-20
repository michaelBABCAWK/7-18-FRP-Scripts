using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial_restartPort : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {

        switch (collision.gameObject.tag)
        {
            case "Untagged Player":
                SceneManager.LoadScene(9, LoadSceneMode.Single);
                print("ahh");

                break;
            case "Player":
                SceneManager.LoadScene(9, LoadSceneMode.Single);
                print("ahh");

                break;
            default:
                break;
        }
    }
}
