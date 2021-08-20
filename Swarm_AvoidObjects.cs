using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Swarm_AvoidObjects : MonoBehaviour
{
    GameObject[] obstaclesToAvoid_;
    GameObject[] terrainToAvoid_;
    GameObject[] enemyObjectsToAvoid_;

    [Header("Radius Detection")]
    public float viewRadius_;
    [Range(0, 360)]
    public float ViewAngle_;
    public float moveAwayDist_;


    public LayerMask targetMask_;
    public LayerMask obstacleMask_;
    public LayerMask terrainLayer_;



    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();


    Swarm_MovementBehavior thisOnesMovement_;



    bool pursuePlayer_;//turn off when the swarm is avoiding an object
    bool avoidObject_ ;//turn on when avoiding objects

    private void Awake()
    {

    }

    void Start()
    {
        SetSwarm_MovementBools();


        StartCoroutine("FindTargetsWithDelay", .2f);



    }


    public void RegisterAllObstacles()
    {
        obstaclesToAvoid_ = GameObject.FindGameObjectsWithTag("Obstacle");
        terrainToAvoid_ = GameObject.FindGameObjectsWithTag("Terrain");
        enemyObjectsToAvoid_ = GameObject.FindGameObjectsWithTag("Enemy");
    }

    public void SetSwarm_MovementBools()//set bools and call methods with new vectors
    {
        thisOnesMovement_ = gameObject.GetComponent<Swarm_MovementBehavior>();

        pursuePlayer_ = thisOnesMovement_.pursuePlayer_;//turn off when the swarm is avoiding an object

        avoidObject_ = thisOnesMovement_.avoidObject_;//turn on when avoiding objects
    }


    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius_ = Physics.OverlapSphere(transform.position, viewRadius_, targetMask_);

        for (int i = 0; i < targetsInViewRadius_.Length; i++)
        {
            Transform target_ = targetsInViewRadius_[i].transform;
            Vector3 dirToTarget = (target_.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < ViewAngle_ / 2)
            {
                float distToTarget = Vector3.Distance(transform.position, target_.position);



                if (Physics.Raycast(transform.position, dirToTarget, distToTarget, terrainLayer_))//move up from terrain
                {
                    pursuePlayer_ = false;
                    avoidObject_ = true;
                    //terrain mthod 
                }
                if (Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask_))//move in a directoin away from all objects in range with this layer
                {
                    pursuePlayer_ = false;
                    avoidObject_ = true;
                    //obst method
                }
                    if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask_))//player detection with no interference
                {
                    //send in direction opposite of whatever is picked up.
                    //adjust behavior of Swarm_MovementBehavior to determine movement, not in this script

                    //visibleTargets.Add(target_);


                }

            }
        }
    }



    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)//, float vertAngleInDegrees control y axis too
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

}

