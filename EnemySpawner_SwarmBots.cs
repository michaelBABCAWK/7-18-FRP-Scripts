using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner_SwarmBots : MonoBehaviour
{
    GameObject Player;
    
    
    Transform playersPosition;
    
    
    float timeinRange;


    bool enoughTimeInRange;
    bool spawnedBaby = false;


    ObjectPooler_LIST objectPooler;


    [Header("Spawn Zones of Swarm Bots")]

    [SerializeField]
    int xMin;
    [SerializeField]
    int xMax;
    [SerializeField]
    int yMin;
    [SerializeField]
    int yMax;
    [SerializeField]
    int zMin;
    [SerializeField]
    int zMax;


    [Header("Trigger Settings")]

    [SerializeField]// ADJUSTABLE
    float triggerDistance = 300;
    [SerializeField]// ADJUStABLE and used to reset the timer on the spawn timer.
    float timeToTriggerSpawn;


    [Header("Spawn at a Time")]

    public int babiesToSpawn;

    //Jobs Objects reference
    public GameObject jobsObject_;
    Jobs_SwarmControl jobsControl_;


    // Start is called before the first frame update
    private void Start()
    {
        objectPooler = ObjectPooler_LIST.Instance;

        Player = GameObject.Find("Player");
        playersPosition = Player.transform;

        enoughTimeInRange = false;

        //sets timeInRange to equal value set by designer.
        timeinRange = timeToTriggerSpawn;


        IdentifyJobs();
    }

    public void IdentifyJobs()
    {
        //jobsObject_ = GameObject.Find("Jobs Object");
        jobsControl_ = jobsObject_.GetComponent<Jobs_SwarmControl>();

    }

    // Update is called once per frame
    void Update()
    {
        spawnBaddie();
    }

    public GameObject creatingJobsObject()
    {
        //GameObject _returnedObject = PoolObject;


        int xRandom = Random.Range(xMin, xMax);
        int yRandom = Random.Range(yMin, yMax);
        int zRandom = Random.Range(zMin, zMax);

        Vector3 spawnHere = new Vector3(xRandom, yRandom, zRandom);


        GameObject newUnit = objectPooler.SpawnFromPool("SwarmBot", spawnHere, Quaternion.identity);
        //creating a gameobject not a transform

        //newUnit.AddComponent<EnemySpawner_WeActive>();
        //adding component that lets JOBS object know there are ACTIVE units from the POOL

        jobsControl_.AddToList(newUnit);//calls swarmControl script to add a new unit with the newly made transform

        //if(!jobsControl_.trackingForJob_)
        jobsControl_.StartTracking();

        return newUnit;
    }

    private void spawnBaddie()
    {
        //Create spawn zones for instantiate.

        //SET BY POSITION OF PLAYER/SPAWN TRANSFORMS. DO NOT SET MANUALLY.
        float distanceFromPlayer = Vector3.Distance(playersPosition.position, gameObject.transform.position);

        if (distanceFromPlayer <= triggerDistance)
        {

            timeinRange -= 1 * Time.deltaTime;

            if (timeinRange <= 0)
            {
                enoughTimeInRange = true;

                if (enoughTimeInRange && spawnedBaby == false)
                {

                    for (int i = 0; i < babiesToSpawn; i++)
                    {

                        creatingJobsObject();

                    }

                    spawnedBaby = true;

                    return;
                }
            }
        }
        else
        {
            //wiil reset the timer.
            timeinRange = timeToTriggerSpawn;
            spawnedBaby = false;
        }
    }

}
