using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyEnemy : MonoBehaviour
{
    public int Health = 10;

    bool alive;

    BoxCollider EnemyBox;

    [SerializeField] GameObject DeathFx;
    [SerializeField] int ScoreFromKill = 15;

    Scoreboard scoreboard;

    private void Start()
    {
        SetGlobals();

        AddBoxCollider();
    }

    private void SetGlobals()
    {
        scoreboard = FindObjectOfType<Scoreboard>();
        //alive = true;
    }

    private void AddBoxCollider()
    {
        EnemyBox = gameObject.AddComponent<BoxCollider>();
        EnemyBox.isTrigger = false;
    }

    private void OnParticleCollision(GameObject other)
    {
        Health -= 1;
    }

    private void Update()
    {
        KillEnemy();
    }

    private void KillEnemy()
    {
        if (Health == 0)
        {
            RemoveModel();

            scoreboard.ScoreHit(ScoreFromKill);
        }
    }

    private void RemoveModel()
    {
        EnemyBox.enabled = false;
        //alive = false;
        Destroy(gameObject);
        Instantiate(DeathFx, transform.position, Quaternion.identity);
    }
}
