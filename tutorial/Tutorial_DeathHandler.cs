using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial_DeathHandler : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Deadly Tutorial":
                SceneManager.LoadScene(8, LoadSceneMode.Single);
                break;

            default:
                break;
        }
    }

    private void OnParticleCollision(GameObject collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Deadly Tutorial":
                SceneManager.LoadScene(8, LoadSceneMode.Single);
                break;

            default:
                break;
        }
    }
    private void Update()
    {
        
    }

}
