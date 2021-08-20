using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
//using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.SceneManagement;

public class Boss_ManageDeath : MonoBehaviour
{
    public void beenKilled()//set explosions and boss falling. Then load up the Win Screen
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        gameObject.SetActive(false);

        SceneManager.LoadScene(10, LoadSceneMode.Single);//10 is win screen
    }
}
