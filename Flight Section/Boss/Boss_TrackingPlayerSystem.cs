using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_TrackingPlayerSystem : MonoBehaviour
{
    public float fireRange = 0.1f;

    GameObject playerSpotToHIt;
    Transform spotToHitTransform;

    Boss_TurretSystem[] bossTurretSystem;
        

    void Start()
    {
        playerSpotToHIt = GameObject.Find("Player");

        spotToHitTransform = playerSpotToHIt.transform;

        bossTurretSystem = GetComponentsInChildren<Boss_TurretSystem>();
    }


    //LateUpdate will run after physics have been calculated. Make a habit of moving character/enemy, anything, movement into LateUpdate.
     void Update()
    {
        //V2 minus V1 to get the requried Vector to travel towards destination. V1 minus V2 in order to go away fro destination.
        //length of vecotr is Magnitude.

        Vector3 direction = spotToHitTransform.position - this.transform.position;

        Debug.DrawRay(this.transform.position, direction, Color.red);

        float distanceFromPlayer = Vector3.Distance(playerSpotToHIt.transform.position, gameObject.transform.position);

        if (distanceFromPlayer < fireRange && spotToHitTransform.position.y > this.transform.position.y)
        {
            for (int i = 0; i < bossTurretSystem.Length; i++)
            {
                bossTurretSystem[i].laserControl(true);

            }
        }
        else
        {
            for (int i = 0; i < bossTurretSystem.Length; i++)
            {
                bossTurretSystem[i].laserControl(false);
            }

        }
    }
}
