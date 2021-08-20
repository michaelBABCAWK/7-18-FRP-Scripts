using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;

[RequireComponent(typeof(Rigidbody))]
public class Swarm_Job_Patrol : MonoBehaviour
{
    [Header("Swarm Unit Locations to Track")]
    GameObject swarmHere_, patrollingHere_;
    //          player          boss area
    Transform swarmSpotTarget_, PatrolHereTarget_;//orbit spot will be the boss cylinder.

    private Rigidbody body_;

    private Animator animator_;


    [Header("General Movement Values")]
    [SerializeField] float idleSpeed_;
    [SerializeField] float speedAdjust_ = 20f;
    [SerializeField] float turnSpeed_;
    [SerializeField] float switchSeconds_;
    [SerializeField] float idleRatio_;
    [SerializeField] float switchDirectionTime_;

    [Header("Tracking Player Values")]
    [SerializeField] float swarmSpotDrift_ = 10f;
    [SerializeField] float urgencyToPlayer_ = 0.2f;

    [Header("Avoid Objects Values")]
    [SerializeField] float maxEnemyProximity_ = 50f;
    [SerializeField] float timeToStayWithPlayer_ = 10f;

    [Header("Movement Restriction Values")]
    public bool useBoundary_;
    [SerializeField] float xMinMax_;
    [SerializeField] float zMinMax_;
    [System.NonSerialized] public float changeTarget_ = 0f, changeAnim_ = 0f, timeSinceTarget_ = 0f, timeSinceAnim_ = 0f, prveAnim_,
     prevSpeed_, speed_, zTurn_, prevZ_, turnSpeedBackup_;

    [Header("Other Values")]
    public float distanceFromPatrolSpot_;
    [SerializeField] public float distanceFromPlayer_;
    [SerializeField] public float distanceFromSwarmSpot_;//swarmspot is on player, base is center of map
    [SerializeField] public float randomBaseOffSet_ = 5f, delayStart_ = 0f;



    Vector3 rotateTarget_, position_, direction_,velocity_, randomizedHomeTargetPosition_;//enemySpots_
    [SerializeField] Vector2 animSpeedMinMax, moveSpeedMinMax, changeAnimEveryFrameTo, changeTargetEveryFromTo_;
    [SerializeField] Vector2 radiueMinMax_;
    [SerializeField] Vector2 xLimits_,yLimits_,zLimits_;



    [SerializeField] public bool pursuePlayer_ = false, colliding_ = false, outOfBounds_ = false;



    private Quaternion lookRotation_;


    private float deltaTime_;


    [Header("Jobs")]
    public static List<GameObject> swarmList_;
    public bool useJob_;



    private void Start()
    {
        VariableMethods();

        body_.velocity = idleSpeed_ * direction_;

    }

    private void VariableMethods()//holds all methods that set initial variables outside of local ones.
    {
        SetVariablesForTrackingObjects();

        SetVariablesForEnemyDrone();
    }

    private void SetVariablesForTrackingObjects()
    {
        patrollingHere_ = GameObject.Find("Model_BossTube");
        PatrolHereTarget_ = patrollingHere_.transform;

        swarmHere_ = GameObject.Find("spotToSwarm");
        swarmSpotTarget_ = swarmHere_.transform;
    }

    private void SetVariablesForEnemyDrone()
    {
        body_ = GetComponent<Rigidbody>();

        //animator_ = GetComponent<Animator>();

        turnSpeedBackup_ = turnSpeed_;

        //line below: multiplying the Euler by the forward facing vector will give the direction the bird is facing. Maybe by filtering out only what is being multiplied?
        direction_ = Quaternion.Euler(transform.eulerAngles) * (Vector3.forward);

        body_.velocity = idleSpeed_ * direction_;
    }

    //If a method declares a type, in this case Vector3,it also needs to return that type.

    //ignore the methods above, for the JOBS system




    //removed animation control
    private void ChangeTarget()
    {
        if (changeTarget_ < 0f)
        {
            rotateTarget_ = ChangeDirection(body_.transform.position);
            if (pursuePlayer_) changeTarget_ = urgencyToPlayer_;
            else changeTarget_ = UnityEngine.Random.Range(changeTargetEveryFromTo_.x, changeTargetEveryFromTo_.y);
            timeSinceTarget_ = 0f;
        }
    }


    private Vector3 ChangeDirection(Vector3 _currentPosition)//current position is the drones location
    {
        Vector3 newDir;


        if (pursuePlayer_ == true && distanceFromSwarmSpot_ > swarmSpotDrift_)//CHANGE
        {
            randomizedHomeTargetPosition_ = swarmSpotTarget_.position;
            randomizedHomeTargetPosition_.y += UnityEngine.Random.Range(-randomBaseOffSet_, randomBaseOffSet_);
            newDir = swarmSpotTarget_.position - _currentPosition;
        }
        else if (!pursuePlayer_ && distanceFromPatrolSpot_ > radiueMinMax_.y)
        {
            newDir = PatrolHereTarget_.position - _currentPosition;
        }
        else if (!pursuePlayer_ && distanceFromPatrolSpot_ < radiueMinMax_.x)
        {
            newDir = _currentPosition - PatrolHereTarget_.position;
        }
        else
        {
            float _angleXZ = UnityEngine.Random.Range(-Mathf.PI, Mathf.PI); //This will pick a point between -180(-mathf.pi) and 180 (mathf.pi). 
            float _angleY = UnityEngine.Random.Range(-Mathf.PI / 48f, Mathf.PI / 48f);//this equals to 180/48f roughly 3.75. That means the max y change will be between -3.75 and 3.75

            newDir = Mathf.Sin(_angleXZ) * Vector3.forward + Mathf.Cos(_angleXZ) * Vector3.right + Mathf.Sin(_angleY) * Vector3.up;
        }

        return newDir.normalized;
    }

    private void RotatingTheDrone()
    {
        if (rotateTarget_ != Vector3.zero) lookRotation_ = Quaternion.LookRotation(rotateTarget_, Vector3.up);
        Vector3 _rotation = Quaternion.RotateTowards(body_.transform.rotation, lookRotation_, turnSpeed_ * Time.fixedDeltaTime).eulerAngles;
        body_.transform.eulerAngles = _rotation;
    }

    private void RollOnTurn()
    {



        float _temp = prevZ_;
        if (prevZ_ < zTurn_) prevZ_ += Mathf.Min(turnSpeed_ * Time.fixedDeltaTime, zTurn_ - prevZ_);
        else if (prevZ_ >= zTurn_) prevZ_ -= Mathf.Min(turnSpeed_ * Time.fixedDeltaTime, prevZ_ - zTurn_);

        prevZ_ = Mathf.Clamp(prevZ_, -45f, 45f);

        body_.transform.Rotate(0f, 0f, prevZ_ - _temp, Space.Self);



        if (pursuePlayer_ && distanceFromPlayer_ < idleSpeed_)
        {
            body_.velocity = Mathf.Min(idleSpeed_, distanceFromPlayer_) * direction_;
        }
        else
        {

        }


    }






    //Ignore these for the jobs system


    public void StayWithPlayer()
    {
        if (!pursuePlayer_)
        {
            pursuePlayer_ = true;

            Invoke("DontStayWithPlayer", timeToStayWithPlayer_);
        }

    }

    public void DontStayWithPlayer()
    {
        if (pursuePlayer_)
            pursuePlayer_ = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 _direction = body_.position - collision.transform.position;

        Vector3 _moveThere = Vector3.MoveTowards(transform.position, _direction, speed_ / speedAdjust_);// * Time.deltaTime

        body_.transform.position = _moveThere;

        body_.transform.eulerAngles = _moveThere;

        colliding_ = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        Invoke("collisionOff", 1.0f);
    }








    // Update is called once per frame
    void Update()
    {
        print(swarmList_.Count);

        /*
        if (pursuePlayer_) {
            //this is an array containing 
            //all of the swarm units that are added to the list
            //NativeArray<float3> swarmDroneMoveTowards_ = new NativeArray<float>(swarmList_.Count, Allocator.TempJob);

            NativeArray<float3> _droneSpot = new NativeArray<float3>(swarmList_.Count, Allocator.TempJob);//swarmList_.Count is in the length position


            //the lists containing the values for each variable is being stored then passed into the Job below

            //this is calling the parallel job

            for (int i = 0; i < swarmList_.Count; i++)
            {
                _droneSpot[i] = swarmList_[i].transform.position;
            }

            ///Pass in values

            //SwarmBotPatrolJob swarmMethods = new SwarmBotPatrolJob
            {
              //  fixedDeltaTime_ = Time.fixedDeltaTime,
              //  droneSpot_ = _droneSpot,

            };
        }
        else if (!pursuePlayer_)
        {

        }
        //JobHandle jobHandle = SwarmBotPatrolJob.Schedule(swarmList_.Count, 50);
        //jobHandle.Complete();

        */
    }

    private void FixedUpdate()
    {
        //Off to be used in Jobs system
        //JobsReference();

    }

    //Gameobjects


    //Trasforms
    //ref for driftspot
    //ref to player
    //ref for body

    //Bools
    //ref for outofbounds(No because there are no more boundaries now that the level is in space.)
    //ref for colliding
    /*
    public void JobsReference()
    {
        distanceFromPlayer_ = Vector3.Magnitude(randomizedHomeTargetPosition_ - body_.position);//randombase is already set as a vector3 so it does not need to specify .position
        distanceFromSwarmSpot_ = Vector3.Magnitude(swarmSpotTarget_.position - body_.position);
        distanceFromPatrolSpot_ = Vector3.Magnitude(PatrolHereTarget_.position - body_.position);//flyingtarget is a transform, therefore the vector3 needs to be specified with the .position


        if (pursuePlayer_ && !colliding_ && !outOfBounds_ && distanceFromPlayer_ > swarmSpotDrift_)
        {
            Vector3 moveThere = Vector3.MoveTowards(transform.position, swarmSpotTarget_.position, speed_ / speedAdjust_);// * Time.deltaTime
            transform.position = moveThere;

        }

        ChangeTarget();

        zTurn_ = Mathf.Clamp(Vector3.SignedAngle(rotateTarget_, direction_, Vector3.up), -45f, 45f);

        changeTarget_ -= Time.fixedDeltaTime;
        timeSinceTarget_ += Time.fixedDeltaTime;
        changeAnim_ -= Time.fixedDeltaTime;
        timeSinceAnim_ += Time.fixedDeltaTime;

        RotatingTheDrone();

        RollOnTurn();

        direction_ = Quaternion.Euler(transform.eulerAngles) * Vector3.forward;
        body_.velocity = Mathf.Lerp(prevSpeed_, idleSpeed_, Mathf.Clamp(timeSinceAnim_ / switchSeconds_, 0f, 1f)) * direction_;//CHANGE//lerping between prevspeed and speed_, can be replaced with idleSpeed_. (speed_ set in animationcontrol method.

    }

    [BurstCompile]//This is where the methods will run for movement
                  //Reference the public methods in the attached components of the swarm unit?
                  //Then it will be ran in the FixedUpdate section
    public struct SwarmBotPatrolJob : IJobParallelFor
    {
        //will need each value used above written out here. Then passed through from the above Class

        public float fixedDeltaTime_;

        //public NativeArray<float3> droneDirection_;// direction_ variable in Swarm_Patrol Class
        //public List<Transform> playerRef_;
        //public List<Transform> droneRef_;

        //Drone distance from objects
       // public NativeArray<float3> distanceFromPlayer_;//can use this
        //public NativeArray<float3> distanceFromSwarmSpot_;//can use this
        //public NativeArray<float3> distanceFromPatrolSpot_;//can use this

        //public NativeArray<float3> swarmSpotTarget_;//can use this


        public NativeArray<float3> droneSpot_;//can use this

        public NativeArray<Vector3> randomizedHomeTargetPosition_;//can use this


        //execute the methods with passed in values
        public void Execute(int index)
        {//index is equal to a different thread which will run alongside the other threads controlling behaviour of the swarm units.

            // vector3 for: randomizedHomeTargetPosition_ swarmSpotTarget_ PatrolHereTarget_
            distanceFromPlayer_ = Vector3.Magnitude(randomizedHomeTargetPosition_ - droneSpot_);//randombase is already set as a vector3 so it does not need to specify .position

            distanceFromSwarmSpot_ = Vector3.Magnitude(swarmSpotTarget_sition - droneRef_);
            distanceFromPatrolSpot_ = Vector3.Magnitude(swarmSpotTarget_ - droneRef_);//flyingtarget is a transform, therefore the vector3 needs to be specified with the .position


            if (pursuePlayer_ && !colliding_ && !outOfBounds_ && distanceFromPlayer_ > swarmSpotDrift_)
            {
                Vector3 moveThere = Vector3.MoveTowards(transform.position, swarmSpotTarget_.position, speed_ / speedAdjust_);// * Time.deltaTime
                transform.position = moveThere;

            }

            ChangeTarget();

            zTurn_ = Mathf.Clamp(Vector3.SignedAngle(rotateTarget_, direction_, Vector3.up), -45f, 45f);

            changeTarget_ -= Time.fixedDeltaTime;
            timeSinceTarget_ += Time.fixedDeltaTime;
            changeAnim_ -= Time.fixedDeltaTime;
            timeSinceAnim_ += Time.fixedDeltaTime;

            RotatingTheDrone();

            RollOnTurn();

            direction_ = Quaternion.Euler(transform.eulerAngles) * Vector3.forward;
            body_.velocity = Mathf.Lerp(prevSpeed_, idleSpeed_, Mathf.Clamp(timeSinceAnim_ / switchSeconds_, 0f, 1f)) * direction_;//CHANGE//lerping between prevspeed and speed_, can be replaced with idleSpeed_. (speed_ set in animationcontrol method.


        }

    }
    */
}
