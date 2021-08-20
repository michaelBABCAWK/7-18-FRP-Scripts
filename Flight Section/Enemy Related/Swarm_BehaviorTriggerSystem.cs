using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swarm_BehaviorTriggerSystem : MonoBehaviour
{
    enum currentFocus { flying, attacking, reposition, maintainAroundPlayer, lookingAtPlayer };

    [Header("Firing Pattern")]

    private float fireTime_;
    [SerializeField] float fireTimeMin_ = 2;
    [SerializeField] float fireTimeMax_ = 5;
    [SerializeField] float reduceTimerRate_ = .01f;
    private float breakInFireTime_;
    [SerializeField] float breakAdjuster_ = .8f;


    [Header("Trigger Zones")]

    [SerializeField] float triggerFiringZone_;
    [SerializeField] float triggerPursueZone_;


    currentFocus dronesStatus_;

    Swarm_TurretFireSystem swarmFire_;

    Swarm_Patrol swarmMoveBahvior_;

    GameObject player_;

    float distanceFromPlayer_;


    private void Awake()
    {
        fireTime_ = Random.Range(fireTimeMin_, fireTimeMax_);// for first tme firing period


        breakInFireTime_ = fireTime_;//* breakAdjuster_

    }

    private void Start()
    {
        dronesStatus_ = currentFocus.flying;

        player_ = GameObject.FindGameObjectWithTag("Player");

        swarmFire_ = GetComponentInChildren<Swarm_TurretFireSystem>();

        swarmMoveBahvior_ = GetComponent<Swarm_Patrol>();

    }

    private void Update()
    {
        distanceFromPlayer_ = Vector3.Distance(player_.transform.position, gameObject.transform.position);
        killZonetrigger();
        TrackPlayerNow();
    }


    private void TrackPlayerNow()
    {
        if (distanceFromPlayer_ <= triggerPursueZone_)
        {
            swarmMoveBahvior_.StayWithPlayer();
        }
    }

    public void FiringPattern()
    {
        if (fireTime_ > 0)//if there is still a firetime then keep firing
        {
            fireTime_ -= reduceTimerRate_;

            swarmFire_.fireLasers(true);
        }



        if (fireTime_ <= 0)//if firetime is 0 start reducing the breaktim
        {
            breakInFireTime_ -= reduceTimerRate_;

            swarmFire_.fireLasers(false);

            if (breakInFireTime_ <= 0)//if breaktime is over, set a new firingn time
            {
                fireTime_ = Random.Range(fireTimeMin_, fireTimeMax_);

                breakInFireTime_ = fireTime_;//* breakAdjuster_

            }
        }




    }

    private void killZonetrigger()
    {
        if (distanceFromPlayer_ <= triggerFiringZone_)
        {
            FiringPattern();
            //whatEnemyDoing = currentFocus.attacking;
        }
        else
        {
            //whatEnemyDoing = currentFocus.flying;
            swarmFire_.fireLasers(false);

        }

    }
}
