using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swarm_TurretFireSystem : MonoBehaviour
{
    public GameObject[] Lasers;

    ParticleSystem laserShot;

    GameObject player;

    Transform playerTarget;

    Swarm_DeathScript dropHealth;

    //public Transform playerLocation;
    private void Start()
    {
        player = GameObject.Find("Player");
        playerTarget = player.transform;

        dropHealth = GetComponentInParent<Swarm_DeathScript>();
    }

    public void fireLasers(bool activateWeapons)
    {
        for (int i = 0; i < Lasers.Length; i++)
        {
            ParticleSystem[] theseLasers = gameObject.GetComponentsInChildren<ParticleSystem>();
            var chargerBeam = theseLasers[i].emission;
            chargerBeam.enabled = activateWeapons;
        }
    }


    void OnParticleCollision(GameObject tag)
    {
        if (tag.gameObject.tag == "Player Laser")
        {

            dropHealth.dropHealth();
        }
    }



    void Update()
    {
            //aimingAtPlayer
            transform.LookAt(playerTarget);
    }
}
