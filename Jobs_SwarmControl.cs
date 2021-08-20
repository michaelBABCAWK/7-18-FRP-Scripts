using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;


public class Jobs_SwarmControl : MonoBehaviour
{
    public bool trackingForJob_ = false;

    List<SwarmUnit> swarmList_;

    List<GameObject> swarmUnitClassGameObjectsList_;

    //NativeArray<float3> swarmUnitClassPositionList_;
    NativeArray<Vector3> swarmUnitClassRotationList_;

    NativeArray<bool> swarmUnitClassPursueBoolListMainRef_;
    NativeArray<bool> swarmUnitClassProximityBoolListMainRef_;


    [Header("Values Need Refernces in Class")]

    [Header("Player Attack Variables for Swarm Unit")]
    public GameObject playerGameObjectMainRef;
    //PlayerLocations for tracking threshhold
    [HideInInspector] public Vector3 playerTransformScriptRef_;

    [HideInInspector] public Vector3 swarmSpotTarget_;//orbit spot will be the boss cylinder.


    [Header("Spots to Follow")]
    public GameObject swarmHereGameObjectMainRef_;

    [SerializeField] public bool pursuePlayer_ = false;
    [SerializeField] public bool colliding_ = false;


    [Header("General Swarm Values")]
    public float distanceToStartTrackingMainRef_;
    public float SwarmUnitSpeed_;
    public float speedAdjust_ = 20f;
    public float turnSpeed_;
    public float switchSeconds_;
    public float idleRatio_;
    public float switchDirectionTime_;

    [Header("General Swarm Spot Values")]
    [SerializeField] float swarmSpotDrift_ = 10f;
    [SerializeField] float urgencyToPlayer_ = 0.2f;

    [Header("General Swarm Unit Avoid Objects Values")]
    [SerializeField] float maxEnemyProximity_ = 50f;
    [SerializeField] float timeToStayWithPlayer_ = 10f;

    [Header("Spectating Values Section")]
    [SerializeField] float distanceFromPatrolSpot_;
    [SerializeField] float distanceFromPlayer_;
    [SerializeField] float distanceFromSwarmSpot_;//swarmspot is on player, base is center of map
    [SerializeField] float randomBaseOffSet_ = 5f, delayStart_ = 0f;

    Vector3 rotateTarget_, position_, direction_, velocity_, randomizedHomeTargetPosition_;//enemySpots_

    [SerializeField] Vector2 moveSpeedMinMax, changeTargetEveryFromTo_;
    [SerializeField] Vector2 radiueMinMax_;


    [System.NonSerialized]
    public float changeTarget_ = 0f, changeAnim_ = 0f, timeSinceTarget_ = 0f, timeSinceAnim_ = 0f, prveAnim_,
currentAnim_ = 0f, prevSpeed_, speed_, zTurn_, prevZ_, turnSpeedBackup_;

    //Pass These lists to the JOB


    // Start is called before the first frame update
    void Start()
    {
        //create all lsits here
        swarmList_ = new List<SwarmUnit>();

        //List of Swarm unit class components
        swarmUnitClassGameObjectsList_ = new List<GameObject>();
    }

    //only values that pertain to the Swarm unit. Nothing else
    public class SwarmUnit//variables that only pertain to the swarm units abilities
    {
        [HideInInspector] public GameObject _classSwarmObject;
        //[HideInInspector] public GameObject _classSwarmTransform;

        //Swarm Unit Componets
        [HideInInspector] public Transform _classSwarmTransform;

        //Swarm unit Position control
        //[HideInInspector] public Transform _classSwarmTransfrom;

        [HideInInspector] public int _classPursuePlayervalue;

        //[HideInInspector] public bool _classProximityBool;

        private Quaternion lookRotation_;

        //8 values assigned
    }

    public void StartTracking()
    {
        trackingForJob_ = true;
    }

    public void AddToList( GameObject objectToAdd)
    {
        Transform _swarmTranform = objectToAdd.transform;

        //object to add is the game object being created in this methods only reference
        swarmList_.Add(new SwarmUnit
        {

            _classSwarmObject = objectToAdd,//swarm unit object

            //_classSwarmTransfrom = swarmTranform,//getting transform from created gameobject

            _classSwarmTransform = _swarmTranform,

            _classPursuePlayervalue = 1,

            //_classProximityBool = false,

        });; ;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (trackingForJob_)
        {

            print("tracking for job");

            NativeArray<Vector3> swarmUnitClassPositionList_ = new NativeArray<Vector3>(swarmList_.Count, Allocator.TempJob);
            //swarmUnitClassRotationList_ = new NativeArray<Vector3>(swarmList_.Count, Allocator.TempJob);

            NativeArray<float> swarmPursueValueArrayMainRef = new NativeArray<float>(swarmList_.Count, Allocator.TempJob);

            NativeArray<float> distanceFromPlayerArrayMainRef = new NativeArray<float>(swarmList_.Count, Allocator.TempJob);


            for (int i = 0; i < swarmList_.Count; i++)
            {

                //pass in swarm unit gameobject and components for maipulation in JOB

                //swarm unit components

                //list objects
                //swarmUnitClassGameObjectsList_.Add(swarmList_[i]._classSwarmObject);


                //NativeArrays

                //_pursueBoolsLisMainRef.Add(swarmList_[i]._classPursuePlayerBool);


                //positions and rotations
                swarmUnitClassPositionList_[i] = swarmList_[i]._classSwarmTransform.position;
                //swarmUnitClassRotationList_[i] = swarmList_[i]._classSwarmTransfrom.eulerAngles;//position array are the length of the swarmlist and recieves the position


                //Bools
                //swarmUnitClassPursueBoolListMainRef_[i] = swarmList_[i]._classPursuePlayervalue;
                //swarmUnitClassProximityBoolListMainRef_[i] = swarmList_[i]._classProximityBool;

                swarmPursueValueArrayMainRef[i] = swarmList_[i]._classPursuePlayervalue;

            }

            for(int i = 0; i < swarmList_.Count; i++)
            {
                //var jobsArray = new NativeArray<>

                JOB_Swarm_Movement swarmMovementJob = new JOB_Swarm_Movement
                {
                    //player info passed, swarmspot on player passed, patrol spot passed, swarm unit info passed


                    numberOfUnits_ = swarmList_.Count,


                    pursueValueInJob_ = swarmPursueValueArrayMainRef,

                    //Player info
                    //playerInJob_ = playerGameObjectMainRef,
                    playerPositionInJob_ = playerGameObjectMainRef.transform.position,

                    //swarm spot for swarm unit info
                    //playerSwarmSpotInJob_ = swarmHereGameObjectMainRef_,
                    playerSwarmSpotPositionInJob_ = swarmHereGameObjectMainRef_.transform.position,

                    //Patrol here info for swarm unit
                    //patrollingObjectInJob_ = patrollingHereGameObjectMainRef_,
                    //patrolSpotPositionInJob_ = patrolSpotPositionGameObjectMainRef_.transform.position,

                    //swarm unit components and transforms
                    //swarmUnitObjectInJob_ = swarmUnitClassGameObjectsList_,
                    swarmUnitPositionNotPursueInJob_ = swarmUnitClassPositionList_,//telling the job each position of each Swarm unit in the list
                                                                                   //swarmUnitRotationInJob_ = swarmUnitClassRotationList_,

                    //swarm unit bools
                    //pursuePoolsListInJob_ = _pursueBoolsLisMainRef,
                    //proximityBoolInJob_ = swarmUnitClassProximityBoolListMainRef_,

                    //Swarm unit general settings settings
                    idleSpeedInJob_ = SwarmUnitSpeed_,//each speed gets its own spot in the JOB list for speed
                    speedAdjustInJob_ = speedAdjust_,
                    turnSpeedInJob_ = turnSpeed_,
                    switchSecondsInJob_ = switchSeconds_,
                    idleRatioInJob_ = idleRatio_,
                    switchDirectionTimeInJob_ = switchDirectionTime_,
                    distanceFromPlayerArrayInJob_ = distanceFromPlayerArrayMainRef,

                    randomBaseOffSetLow_ = radiueMinMax_.x,
                    randomBaseOffSetHigh_ = radiueMinMax_.y,
                    deltaTime_ = Time.deltaTime,
                    distanceToStartTrackingInJob_ = distanceToStartTrackingMainRef_

                    //jobDistanceToTriggerTracking_ = randomTrackingThresholdArray_//telling the job what the chosen threshhold is
                };


                JobHandle jobHandle = swarmMovementJob.Schedule(swarmList_.Count, 100);


                jobHandle.Complete();
            }




            for (int i = 0; i < swarmList_.Count; i++)
            {
                swarmList_[i]._classSwarmTransform.position = swarmUnitClassPositionList_[i];
                //swarmList_[i].moveY = moveYArray[i];
            }


            //transform dispose
            swarmUnitClassPositionList_.Dispose();

            distanceFromPlayerArrayMainRef.Dispose();

            //bool dispose
            swarmPursueValueArrayMainRef.Dispose();

        }

    }
}


[BurstCompile]
public struct JOB_Swarm_Movement : IJobParallelFor
{
    //ALL Lists in JOBS receive their attributes from the job.schedule
    public int numberOfUnits_;

    //PlayerLocations for tracking threshhold
    //public GameObject playerInJob_;
    public Vector3 playerPositionInJob_;

    //player swarming spot
    //public GameObject playerSwarmSpotInJob_;
    public Vector3 playerSwarmSpotPositionInJob_;

    //swarm unit object and transform
    //public List <GameObject> swarmUnitObjectInJob_;
    public NativeArray<Vector3> swarmUnitPositionNotPursueInJob_;//orbit spot will be the boss cylinder.
    //public NativeArray<Vector3> swarmUnitRotationInJob_;//orbit spot will be the boss cylinder.
    //public List<Rigidbody> swarmRigidbodyList_;

    //Patrol object references
    //public GameObject patrollingObjectInJob_;
    //public Vector3 patrolSpotPositionInJob_;//orbit spot will be the boss cylinder.

    //list of bools for each Swarm Unit
    public NativeArray<float> pursueValueInJob_;// = false// = false
    //public NativeArray<bool> proximityBoolInJob_;// = false// = false

    //in case async bool hold up is required
    //[HideInInspector] public List<bool> tracking_;

    [Header("General Movement Values")]
    //Any lists will be created outside the 
    public float distanceToStartTrackingInJob_;
    [HideInInspector] public float idleSpeedInJob_;
    [HideInInspector] public float speedAdjustInJob_;//= 20f
    [HideInInspector] public float turnSpeedInJob_;
    [HideInInspector] public float switchSecondsInJob_;
    [HideInInspector] public float idleRatioInJob_;
    [HideInInspector] public float switchDirectionTimeInJob_;
    [HideInInspector] public float randomBaseOffSetLow_;
    [HideInInspector] public float randomBaseOffSetHigh_;

    [Header("Tracking Player Values")]
    [HideInInspector] public float swarmSpotDriftInJob_;// = 10f
    [HideInInspector] public float urgencyToPlayerInJob_;// = 0.2f

    [Header("Avoid Objects Values")]
    [HideInInspector] public float maxEnemyProximityInJob_;// = 50f
    [HideInInspector] public float timeToStayWithPlayerInJob_;//= 10f

    [System.NonSerialized]
    public float changeTarget_, changeAnim_, timeSinceTarget_, timeSinceAnim_, prveAnim_,
    currentAnim_, prevSpeed_, speed_, zTurn_, prevZ_, turnSpeedBackup_;//changeTarget_ = 0f// changeAnim_ = 0f
    // timeSinceTarget_ = 0f// timeSinceAnim_ = 0f// currentAnim_ = 0f

    [Header("Other Values")]//different for each swarm unit
    //[HideInInspector] public NativeArray<float> distanceFromPatrolSpot_;
    //[HideInInspector] public NativeArray<float> distanceFromPlayer_;
    //[HideInInspector] public NativeArray<float> distanceFromSwarmSpot_;//swarmspot is on player, base is center of map
    //[HideInInspector] public NativeArray<float> randomBaseOffSet_;// = 5f
    //[HideInInspector] public NativeArray<float> delayStart_;// = 0f

    [HideInInspector] public Vector3 rotateTarget_, position_, direction_, velocity_, randomizedHomeTargetPosition_, rotateTowards_;//enemySpots_
    [HideInInspector] public Vector2 animSpeedMinMax, moveSpeedMinMax, changeAnimEveryFrameTo, changeTargetEveryFromTo_;
    [HideInInspector] public Vector2 radiueMinMax_;
    [HideInInspector] public Vector2 xLimits_, yLimits_, zLimits_;

    [HideInInspector] public Quaternion lookRotation_;

    //public List<bool> pursuePoolsListInJob_;

    //List<float> distanceFromPlayer_ = new List<float>();

    public NativeArray<float> distanceFromPlayerArrayInJob_;

    public float deltaTime_;
    /// <summary>
    /// aim at player, add random value to x/y of new destination 
    /// will keep the unit moving towards the player nut not in a direct line
    /// </summary>
    // Start is called before the first frame update

    public void Execute(int index)
    {


        //changeTarget_ = 0f;
        //timeSinceTarget_ = 0f;

        //speedAdjustInJob_ = 20f;


        //Set Position variables


        //Set Rotation variables
        //direction_ = Quaternion.Euler(swarmUnitTransformInJob_[index].eulerAngles) * (Vector3.forward);
        //swarmRigidbodyList_[index].velocity = idleSpeedInJob_ * direction_;


        //Set Speed variables
        //turnSpeedBackup_ = turnSpeedInJob_;

        //change what script is effecting the swarmUnit based on values?
        //i,e. if the distacnce to the player is low enough: a new jobs manager will begin to effect the swarmunit


        //Setting Positions at the start
        distanceFromPlayerArrayInJob_[index] = Vector3.Distance(playerPositionInJob_, swarmUnitPositionNotPursueInJob_[index]);//randombase is already set as a vector3 so it does not need to specify .position
                                                                                                           //float distanceFromPatrolSpot_ = Vector3.Distance(patrolSpotPositionInJob_, swarmUnitPositionInJob_[index]);//flyingtarget is a transform, therefore the vector3 needs to be specified with the .position
                                                                                                           //float distanceFromSwarmSpot_ = Vector3.Distance(playerSwarmSpotPositionInJob_, swarmUnitPositionInJob_[index]);
        //new way to trigger bools. ONLY IF STATEMENTS

        //triggering tracking player distance
        //if (Vector3.Magnitude(playerSwarmSpotVector3InJob_ - swarmUnitPositionInJob_[index]) < distanceToStartTracking_)
        if (distanceFromPlayerArrayInJob_[index] < distanceToStartTrackingInJob_)
        {
            swarmUnitPositionNotPursueInJob_[index] = Vector3.MoveTowards(swarmUnitPositionNotPursueInJob_[index], playerSwarmSpotPositionInJob_, idleSpeedInJob_);


            //pursueValueInJob_[index] -= 1;
        }

        if(pursueValueInJob_[index] <= 0)
        {


        }
        //pursuePlayerBoolInJob_[index] = false;

        /*

        if (changeTarget_ < 0f)
            {
                //rotateTarget_ = ChangeDirection(swarmRigidbodyList_.Count, swarmRigidbodyList_[index].transform.position);
                //Vector3 newDir;

                if (pursuePlayerBoolInJob_[index] == true)//CHANGE// && distanceFromSwarmSpot_ > swarmSpotDriftInJob_
                {
                    //randomizedHomeTargetPosition_ = swarmUnitPositionInJob_[index];
                    //randomizedHomeTargetPosition_.y += UnityEngine.Random.Range(randomBaseOffSetLow_, randomBaseOffSetHigh_);
                    //newDir = swarmUnitPositionInJob_[index] - swarmUnitPositionInJob_[index];
                    //newDir = new Vector3(0, 0, 0);

                    //swarmUnitPositionInJob_[index] += Vector3.up * idleSpeedInJob_ * deltaTime_;
                }
            }
        }

        else if (!pursuePlayerBoolInJob_[index] && distanceFromPatrolSpot_ > radiueMinMax_[index])
        {
            newDir = patroSpotPositionInJob_ - swarmUnitPositionInJob_[index];
        }
        else if (!pursuePlayerBoolInJob_[index] && distanceFromPatrolSpot_ < radiueMinMax_.x)
        {
            newDir = swarmUnitPositionInJob_[index] - patroSpotPositionInJob_;
        }
        else
        {
            float _angleXZ = UnityEngine.Random.Range(-Mathf.PI, Mathf.PI); //This will pick a point between -180(-mathf.pi) and 180 (mathf.pi). 
            float _angleY = UnityEngine.Random.Range(-Mathf.PI / 48f, Mathf.PI / 48f);//this equals to 180/48f roughly 3.75. That means the max y change will be between -3.75 and 3.75

            newDir = Mathf.Sin(_angleXZ) * Vector3.forward + Mathf.Cos(_angleXZ) * Vector3.right + Mathf.Sin(_angleY) * Vector3.up;
        }

        //return newDir.normalized;

        rotateTarget_ = newDir;


        if (pursuePlayerBoolInJob_[index]) changeTarget_ = urgencyToPlayerInJob_;
        else changeTarget_ = UnityEngine.Random.Range(changeTargetEveryFromTo_.x, changeTargetEveryFromTo_.y);
        timeSinceTarget_ = 0f;
    }
    /*
    void ChangeTarget(int index)
    {

    }
    */
        /*
        //things in this need to be triggered outside of the method.
        Vector3 ChangeDirection(int index, Vector3 _currentPosition)//current position is the drones location
        {

        }
        */

        //replacement for rigidbody and quaternions
        //Change lists to NAtive Arrays. Lists not needed anymore because no rigidbodies and gameob


        /*
        private void OnCollisionEnter(Collision collision)
        {

        }
        Vector3 _direction = swarmRigidbodyList_[index].position - swarmUnitTransformInJob_[index].position;

        Vector3 _moveThere = Vector3.MoveTowards(swarmUnitTransformInJob_[index].position, _direction, speed_ / speedAdjust_);// * Time.deltaTime

        swarmRigidbodyList_[index].transform.position = _moveThere;

        swarmRigidbodyList_[index].transform.eulerAngles = _moveThere;

        swarmUnitClassProximityBoolList_[] = true;



        public void collisionOff()
        {


        }
        if (colliding_) colliding_ = false;


        */



        /*
        void RotatingTheDrone(int index)
        {

        }

        if (rotateTarget_ != Vector3.zero) lookRotation_ = Quaternion.LookRotation(rotateTarget_, Vector3.up);
        rotateTowards_ = new Vector3(swarmUnitPositionInJob_[index].x, swarmUnitPositionInJob_[index].y, swarmUnitPositionInJob_[index].z);// * Time.fixedDeltaTime
                                                                                                                                           //Vector3 _rotation = new Vector3.RotateTowards(swarmUnitPositionInJob_[index], lookRotation_, turnSpeedInJob_).eulerAngles;// * Time.fixedDeltaTime
        swarmUnitRotationInJob_[index] = rotateTowards_;


        /*

        void RollOnTurn(int index)
        {

        }

        float _temp = prevZ_;
        if (prevZ_ < zTurn_) prevZ_ += Mathf.Min(turnSpeedInJob_ * deltaTime_, zTurn_ - prevZ_);
        else if (prevZ_ >= zTurn_) prevZ_ -= Mathf.Min(turnSpeedInJob_ * deltaTime_, prevZ_ - zTurn_);

        prevZ_ = Mathf.Clamp(prevZ_, -45f, 45f);

        swarmUnitRotationInJob_[index] = rotateTarget_;
        //swarmUnitRotationInJob_[index].Rotate(0f, 0f, prevZ_ - _temp, Space.Self);



        if (pursuePlayerBoolInJob_[index] && distanceFromPlayer_ < idleSpeedInJob_)
        {
            swarmUnitRotationInJob_[index] = Mathf.Min(idleSpeedInJob_, distanceFromPlayer_) * direction_;

            //swarmRigidbodyList_[index].velocity = Mathf.Min(idleSpeedInJob_, distanceFromPlayer_[index]) * direction_;
        }
        else
        {

        }

        //Fixdupdate section



        if (pursuePlayerBoolInJob_[index] && !proximityBoolInJob_[index] && distanceFromPlayer_ > swarmSpotDriftInJob_)
        {
            Vector3 moveThere = Vector3.MoveTowards(swarmUnitPositionInJob_[index], swarmUnitPositionInJob_[index], speed_ / speedAdjustInJob_ * deltaTime_);// * Time.deltaTime
            swarmUnitPositionInJob_[index] = moveThere;

        }



        //ChangeTarget(index);

        zTurn_ = Mathf.Clamp(Vector3.SignedAngle(rotateTarget_, direction_, Vector3.up), -45f, 45f);

        changeTarget_ -= deltaTime_;
        timeSinceTarget_ += deltaTime_;
        changeAnim_ -= deltaTime_;
        timeSinceAnim_ += deltaTime_;

        //RotatingTheDrone();



        //RollOnTurn(index);

        direction_ = new Vector3(playerSwarmSpotPositionInJob_.x,
    playerSwarmSpotPositionInJob_.y,
    playerSwarmSpotPositionInJob_.z);


        Vector3.MoveTowards(swarmUnitPositionInJob_[index], direction_, idleSpeedInJob_);


        ///direction_ = Quaternion.Euler(swarmUnitTransformInJob_[index].eulerAngles) * Vector3.forward;
        swarmUnitRotationInJob_[index] = Mathf.Lerp(prevSpeed_, idleSpeedInJob_, Mathf.Clamp(timeSinceAnim_ / switchSecondsInJob_, 0f, 1f)) * direction_;//CHANGE//lerping between prevspeed and speed_, can be replaced with idleSpeed_. (speed_ set in animationcontrol method.
    }
            */


    }
}
