using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss_SightAroundBody : MonoBehaviour
{
    string playerTag;

    Swarm_MovementBehavior tooClose;

    public float rayDistance = 250f;

    private void Start()
    {
        tooClose = GetComponent<Swarm_MovementBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        ProduceRay();
    }

    private void ProduceRay()
    {
        RaycastHit hit;

        float theDistance;

        Vector3 forward = transform.TransformDirection(Vector3.forward) * rayDistance;

        Debug.DrawRay(transform.position, (forward), Color.green);

        if (Physics.Raycast(transform.position, Vector3.forward, out hit))
        {
            float DistanceFromImpact = Vector3.Distance(this.transform.position , hit.transform.position);

            if(DistanceFromImpact <= 100f)
            {

            }

            theDistance = hit.distance;
         
            playerTag = hit.collider.tag;
        }


        if (playerTag == "Player")
        {


        }
    }
}
