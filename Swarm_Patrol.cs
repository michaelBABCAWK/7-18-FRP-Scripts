using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class Swarm_Patrol : MonoBehaviour
{
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
    currentAnim_ = 0f, prevSpeed_, speed_, zTurn_, prevZ_, turnSpeedBackup_;

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

    /// <summary>
    /// aim at player, add random value to x/y of new destination 
    /// will keep the unit moving towards the player nut not in a direct line
    /// </summary>
    // Start is called before the first frame update

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
        if(pursuePlayer_)
        pursuePlayer_ = false;
    }

    private void AnimationControl()
    {
        if (changeAnim_ < 0f)
        {
            prveAnim_ = currentAnim_;
            currentAnim_ = ChangeAnim(currentAnim_);
            changeAnim_ = Random.Range(changeAnimEveryFrameTo.x, changeAnimEveryFrameTo.y);
            timeSinceAnim_ = 0f;
            prevSpeed_ = speed_;
            if (currentAnim_ == 0) speed_ = idleSpeed_;
            else speed_ = Mathf.Lerp(moveSpeedMinMax.x, moveSpeedMinMax.y, (currentAnim_ - animSpeedMinMax.x) / (animSpeedMinMax.y - animSpeedMinMax.x));
        }
    }

    private float ChangeAnim(float currentAnim)
    {
        float newState;

        if (Random.Range(0f, 1f) < idleRatio_) newState = 0f; else
        {
            newState = Random.Range(animSpeedMinMax.x, animSpeedMinMax.y);
        }
        if(newState != currentAnim_)
        {
            animator_.SetFloat("flyspeed", newState);
            if (newState == 0) animator_.speed = 1f; else animator_.speed = newState;
        }

        return newState;
    }

    private void ChangeTarget()
    {
        if (changeTarget_ < 0f)
        {
            rotateTarget_ = ChangeDirection(body_.transform.position);
            if (pursuePlayer_) changeTarget_ = urgencyToPlayer_;
            else changeTarget_ = Random.Range(changeTargetEveryFromTo_.x, changeTargetEveryFromTo_.y);
            timeSinceTarget_ = 0f;
        }
    }


    private Vector3 ChangeDirection(Vector3 _currentPosition)//current position is the drones location
    {
        Vector3 newDir;


        if (pursuePlayer_ == true && distanceFromSwarmSpot_ > swarmSpotDrift_)//CHANGE
        {
            randomizedHomeTargetPosition_ = swarmSpotTarget_.position;
            randomizedHomeTargetPosition_.y += Random.Range(-randomBaseOffSet_, randomBaseOffSet_);
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
            float _angleXZ = Random.Range(-Mathf.PI, Mathf.PI); //This will pick a point between -180(-mathf.pi) and 180 (mathf.pi). 
            float _angleY = Random.Range(-Mathf.PI / 48f, Mathf.PI / 48f);//this equals to 180/48f roughly 3.75. That means the max y change will be between -3.75 and 3.75

            newDir = Mathf.Sin(_angleXZ) * Vector3.forward + Mathf.Cos(_angleXZ) * Vector3.right + Mathf.Sin(_angleY) * Vector3.up;
        }

        return newDir.normalized;
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

    public void collisionOff()
    {

        if(colliding_)        colliding_ = false;

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


    public Vector3 OutOfBoundsX(Rigidbody _this)
    {
        position_ = new Vector3(0, 0, 0);

        _this.transform.position = Vector3.MoveTowards(body_.transform.position, position_, idleSpeed_ * Time.fixedDeltaTime);

        outOfBounds_ = true;

        return position_;
    }


    public Vector3 OutOfBoundsY(Rigidbody _this)
    {

        position_ = _this.transform.position;
        position_.y = Mathf.Clamp(position_.y, yLimits_.x, yLimits_.y);


        body_.transform.position = Vector3.MoveTowards(body_.transform.position, position_, idleSpeed_ * Time.fixedDeltaTime);

        
        outOfBounds_ = true;

        return position_;
    }

    public Vector3 OutOfBoundsZ(Rigidbody _this)
    {
        position_ = new Vector3(0, 0, 0);

        _this.transform.position = Vector3.MoveTowards(body_.transform.position, position_, idleSpeed_ * Time.fixedDeltaTime);

        outOfBounds_ = true;

        return position_;
    }
    private void PursuitBasedOnPlayerY()
    {
        if (swarmHere_.transform.position.y < yLimits_.x || swarmHere_.transform.position.y > yLimits_.y)
        {
            Vector3 lowSwarmSpot_ = new Vector3(swarmHere_.transform.position.x, this.transform.position.y, swarmHere_.transform.position.z);//excludes the targets y so the swarm unit will ont pursue lower than the y point

            Vector3 lowMoveThere = Vector3.MoveTowards(transform.position, lowSwarmSpot_, idleSpeed_ * Time.deltaTime);


            transform.position = lowMoveThere;

        }


    }


    private void DroneMovement_Boundary()
    {
        //these (if sections) below rotate the y axis making the drone turn around.
        //If drone goes past any of these points: one of the axis will turn to send the drone in a different direction.

        if (body_.transform.position.y < yLimits_.x)// || body_.transform.position.y > yLimits_.y
        {
            OutOfBoundsY(body_);
        }

        if (body_.transform.position.x < -xLimits_.x || body_.transform.position.x > xLimits_.y && !pursuePlayer_)
        {
            OutOfBoundsX(body_);
        }

        else if (body_.transform.position.z < -zLimits_.x || body_.transform.position.z > zLimits_.y && !pursuePlayer_)
        {
            OutOfBoundsZ(body_);
        }
        else
        {
            outOfBounds_ = false;
        }
    }







    // Update is called once per frame
    void Update()
    {

    }


    private void FixedUpdate()
    {

        distanceFromPlayer_ = Vector3.Magnitude(randomizedHomeTargetPosition_ - body_.position);//randombase is already set as a vector3 so it does not need to specify .position
        distanceFromSwarmSpot_ = Vector3.Magnitude(swarmSpotTarget_.position - body_.position);
        distanceFromPatrolSpot_ = Vector3.Magnitude(PatrolHereTarget_.position - body_.position);//flyingtarget is a transform, therefore the vector3 needs to be specified with the .position


        //Replace with a unit manager that can keep them tracked via a list or dictionary? if that is how those work, and distances



        if (useBoundary_)
        {
            DroneMovement_Boundary();
        }





        PursuitBasedOnPlayerY();


        if (pursuePlayer_ && !colliding_ && !outOfBounds_ && distanceFromPlayer_ > swarmSpotDrift_)
        {
            Vector3 moveThere = Vector3.MoveTowards(transform.position, swarmSpotTarget_.position, speed_/ speedAdjust_);// * Time.deltaTime
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
