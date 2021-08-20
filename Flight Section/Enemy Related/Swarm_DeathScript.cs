using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swarm_DeathScript : MonoBehaviour
{
    public int Health;

    BoxCollider EnemyBox;

    [SerializeField] GameObject DeathFx;

    Scoreboard triggerScore;


    private void Start()
    {    
        AddBoxCollider();
    }

    private void AddBoxCollider()
    {
        triggerScore = FindObjectOfType<Scoreboard>();
        EnemyBox = gameObject.AddComponent<BoxCollider>();
        EnemyBox.isTrigger = false;
    }

    void OnParticleCollision(GameObject tag)
    {
        if (tag.gameObject.tag == "Player Laser")
        {
            //StartCoroutine(hitReact.sizeDown());
            dropHealth();

            if (Health == 0)
            {
                Instantiate(DeathFx, transform.position, Quaternion.identity);
                triggerScore.ScoreHit(15);
                Destroy(gameObject);
            }

        }


    }

    public void dropHealth()
    {
        Health -= 1;
    }
}
