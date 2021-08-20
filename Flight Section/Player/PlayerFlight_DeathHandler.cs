using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerFlight_DeathHandler : MonoBehaviour
{
    [SerializeField] GameObject DeathFx_;
    [SerializeField] GameObject playerBody_;

    public static int ShieldStrength = 25;

    [SerializeField] private float loadDelay;

    private void Awake()
    {
        DeathFx_.SetActive(false);

    }

    private void Start()
    {
        //shield stength multiple each death. fix
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnDeathEvents();
    }

    public void OnDeathEvents()
    {
        gameObject.SendMessage("Dead");
        DeathFx_.SetActive(true);
        playerBody_.SetActive(false);
        Invoke("LoadScene", loadDelay);
    }

    void LoadScene()
    {
        SceneManager.LoadScene("Final Flight", LoadSceneMode.Single);
    }
}
