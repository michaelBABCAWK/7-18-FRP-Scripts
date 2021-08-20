using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Boss_MissleDefenseSystem : MonoBehaviour
{

    public event EventHandler EventDetectingPlayer_;
    public event EventHandler EventLostPlayer_;


    [Header("Layer Masks")]
    public LayerMask targetMask_;
    public LayerMask obstacleMask_;


    [Header("Player Object to Track")]
     GameObject playerRef_;//[SerializeField]


    [Header("Missle Object to Spawn")]
    [SerializeField] GameObject misslePrefab_;


    [Header("Spwan Missle Here")]
    [SerializeField] Transform MissleSpawn_;
    [SerializeField] int numberToSpawn_;


    [Header("Firing Settings")]
    [SerializeField] float rangeLimit_;
    [SerializeField] float timeToFire_;
    float timetoFireRef_;


    [Header("Radius Detection")]
    public float viewRadius_;
    [Range(0, 360)]
    public float ViewAngle_;

    bool oneFireLeft_ = true;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();


    void Start()
    {

        StartCoroutine("FindTargetsWithDelay", .2f);

        playerRef_ = GameObject.Find("Player");

        timetoFireRef_ = timeToFire_;

    }



    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (this.oneFireLeft_ == true)//keep from repeating the enumerator
        {


            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }


    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius_ = Physics.OverlapSphere(transform.position, viewRadius_, targetMask_);

        for(int i = 0; i < targetsInViewRadius_.Length; i++)
        {
            Transform target_ = targetsInViewRadius_[i].transform;
            Vector3 dirToTarget = (target_.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < ViewAngle_ / 2) 
            {
                float distToTarget = Vector3.Distance(transform.position, target_.position);

                if (Physics.Raycast(transform.position, dirToTarget, distToTarget, targetMask_))
                {
                    visibleTargets.Add(target_);

                    EventDetectingPlayer_?.Invoke(this, EventArgs.Empty);//Triggers primer on Event

                    this.timeToFire_ -= 1 * Time.fixedDeltaTime;//time to fire will be removed. In-sync with the shield going down//

                    if (this.timeToFire_ <= 0 && this.oneFireLeft_ == true)
                    {
                        Vector3 missleSpawnPoint_ = MissleSpawn_.position;


                        ObjectPooler_LIST.Instance.SpawnFromPool("MisslePrefab", missleSpawnPoint_, Quaternion.identity);


                        this.oneFireLeft_ = false;

                        Invoke("ResetFire",2f);
                    }

                    //EventLostPlayer_?.Invoke(this, EventArgs.Empty);//Triggers primer on Event

                }
                if (Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask_))
                {
                    EventLostPlayer_?.Invoke(this, EventArgs.Empty);//Triggers primer on Event
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

    public void ResetFire()
    {
        this.timeToFire_ = timetoFireRef_;
        this.oneFireLeft_ = true;

        StartCoroutine("FindTargetsWithDelay", .2f);

    }
}

