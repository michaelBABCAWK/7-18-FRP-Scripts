using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_TurretSystem : MonoBehaviour
{
    public GameObject[] Lasers;

    GameObject player;

    Transform playerTarget;

    Swarm_DeathScript dropHealth;

    bool genActive = true;

    //public Transform playerLocation;
    private void Start()
    {
        player = GameObject.Find("BossTarget");

        playerTarget = player.transform;

        dropHealth = GetComponentInParent<Swarm_DeathScript>();
    }

    public void turretOff()
    {
        this.genActive = false;
    }

    /// <summary>
    /// Check if it is possible to reset number of lasers listed in array.
    /// </summary>
    /// 



    public void laserControl(bool activateWeapons)
    {
        for (int i = 0; i < Lasers.Length; i++)
        {
            ParticleSystem[] theseLasers = gameObject.GetComponentsInChildren<ParticleSystem>();
            var chargerBeam = theseLasers[i].emission;
            chargerBeam.enabled = activateWeapons;
        }

    }

    void Update()
    {
        //aimingAtPlayer
        if (genActive)
        {
            transform.LookAt(playerTarget);

        }

    }
}
