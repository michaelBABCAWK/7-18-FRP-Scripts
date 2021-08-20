using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial_Phase2 : MonoBehaviour
{
    void loadNextScene()
    {
        SceneManager.LoadScene(9, LoadSceneMode.Single);
    }

    private void OnCollisionEnter(Collision collision)
    {

        switch (collision.gameObject.tag)
        {
            case "Untagged Player":
                loadNextScene();
                break;
            default:
                break;
        }
    }
}
