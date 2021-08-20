using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]

public class Swarm_MovementBehavior : MonoBehaviour
{
    
    [SerializeField] float idleSpeed_, turnSpeed, switchSeconds, idleRatio;
    [SerializeField] float xMinMax;
    [SerializeField] float zMinMax;
    [SerializeField] public float randomBaseOffSet = 5, delayStart = 0f;//Keep distance from other ships? delay can be used to keep enemies from
    //turning at the same time.

    [SerializeField] Vector2 animSpeedMinMax, moveSpeedMinMax, changeAnimEveryFrameTo, changeTargetEveryFromTo;
    //[SerializeField] Transform homeTarget, flyingTarget;
    //replacing above line in oder to assign values at birth.
    Transform swarmSpotTarget, lowSwarmSpot_, flyingTarget;
    //Line below conrols how close the enemy will get to the target. Duplicate to control distance between enemies.
    [SerializeField] Vector2 radiueMinMax;
    //controls how high object can go. Mightneed to restrict lowest level butunrestrict highest level.
    [SerializeField] Vector2 yMinMax;


    GameObject swarmSpot, flyingTargetObjects;

    private Animator animator;
    private Rigidbody body;
    [System.NonSerialized] public float changeTarget = 0f, changeAnim = 0f, timeSinceTarget = 0f, timeSinceAnim_ = 0f, prevAnim_, currentAnim_ = 0f, speed_, prevSpeed_, zTurn, prevZ,
        turnSpeedBackup;
    private Vector3 rotateTarget, position, direction, velocity, randomizedBase;
    private Quaternion lookRotation;
    [System.NonSerialized]  public float distanceFromSwarmSpot, distanceFromTarget;

    [SerializeField] private float trackThreshold, turnValue, notPursueTrackThreshold;



    public bool pursuePlayer_;//can be used to return ships to different areas of the map.
    public bool avoidObject_;



    // Start is called before the first frame update
    void Start()
    {
        SetComponents();

        SettingBools();

        if (delayStart < 0f)
        {
            //making velocity equal to direction which is now equal to the forward facing direction of the bot.
            //MAKE BOT GO FORWARD.
            //velocity is being pushed by direction, and since the direction equals the direction the bot is facing: thevelocity will be going in that direction

            body.velocity = idleSpeed_ * direction;
        }
    }


    public void  MoveAwayFromObstacle(Vector3 obstacle,float distanceToTravel)
    {

    }

    public void MoveAwayFromTerrain(Vector3 terrain, float distanceToTravel)
    {

    }

    private void SetComponents()
    {
        //centerofMap = new Vector3(0, 0, 0);
        //TO-do: Set enemies as children of spawn.
        // reference Parent as homeTargetObject.

        swarmSpot = GameObject.Find("spotToSwarm");
        swarmSpotTarget = swarmSpot.transform;

        flyingTargetObjects = GameObject.Find("SpotToHit");
        flyingTarget = flyingTargetObjects.transform;

        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody>();
        turnSpeedBackup = turnSpeed;

        distanceFromTarget = Vector3.Magnitude(flyingTarget.position - body.position);

        //line below: multiplying the Euler by the forward facing vector will give the direction the bird is facing. Maybe by filtering out only what is being multiplied?
        direction = Quaternion.Euler(transform.eulerAngles) * (Vector3.forward);
    }

    private void SettingBools()
    {
        pursuePlayer_ = false;
        avoidObject_ = false;
    }

    public void StayWithPlayer()
    {
        pursuePlayer_ = true;
    }

    void randomPatrol()
    {

        if (delayStart > 0f)
        {
            delayStart -= Time.fixedDeltaTime;
            return;
        }

        //magnitude equals the shortest distance between two points (ROUGHLY WATCH MOER TUTORIALS TO BETTER UNDERSTAND.)
        distanceFromSwarmSpot = Vector3.Magnitude(swarmSpotTarget.position - transform.position);

        //magnitude equals the shortest distance between two points (ROUGHLY WATCH MOER TUTORIALS TO BETTER UNDERSTAND.)
        distanceFromTarget = Vector3.Magnitude(flyingTarget.position = body.position);

        //delayStart can be used as delay for different bots.
        AnimationControl();

        if (pursuePlayer_ == false && distanceFromSwarmSpot >= trackThreshold && swarmSpotTarget.position.y >= yMinMax.x)// 
        {
            if (turnSpeed != 300f && body.velocity.magnitude != 0f)
            {
                print("Pursue ffalse");

                Vector3 moveThere = Vector3.MoveTowards(transform.position, swarmSpotTarget.position, idleSpeed_ * Time.deltaTime);
                transform.position = moveThere;
                turnSpeedBackup = turnSpeed;
                turnSpeed = turnValue;
            }
            else if (distanceFromSwarmSpot <= 1f)
            {
                turnSpeed = turnSpeedBackup;
                //Return benath stops bots when it reaches target.
                return;
            }

        }



        //When Swarm is tracking the player
        if (pursuePlayer_ && distanceFromSwarmSpot >= trackThreshold && swarmSpotTarget.position.y >= yMinMax.x)// 
        {

            PursuitBasedOnPlayerY();


            if (turnSpeed != 300f && body.velocity.magnitude != 0f)
            {
                Vector3 moveThere = Vector3.MoveTowards(transform.position, swarmSpotTarget.position, idleSpeed_ * Time.deltaTime);
                transform.position = moveThere.normalized;
                turnSpeedBackup = turnSpeed;
                turnSpeed = turnValue;
            }
            else if (distanceFromSwarmSpot <= 1f)
            {
                turnSpeed = turnSpeedBackup;
                //Return benath stops bots when it reaches target.
                return;
            }
        }

        if (changeTarget < 0f)
        {
            rotateTarget = changeDirection(body.transform.position);
            if (pursuePlayer_) changeTarget = 10f;//updates target every 0.2 seconds.
            else changeTarget = Random.Range(changeAnimEveryFrameTo.x, changeAnimEveryFrameTo.y);
            timeSinceTarget = 0f;
        }

        DroneMovement_Boundary();

        //important to use SignedAngle because angle would only return a positive value. But we need pos/neg in order to process rotations for turning in all directions.
        zTurn = Mathf.Clamp(Vector3.SignedAngle(rotateTarget, direction, Vector3.up), -45f, 45f);

        changeAnim -= Time.fixedDeltaTime;
        changeTarget -= Time.fixedDeltaTime;
        timeSinceTarget += Time.fixedDeltaTime;
        timeSinceAnim_ += Time.fixedDeltaTime;

        //if the tramsform is not at 0,0,0 then the bird Gameobject will continually getanew look rotation.
        if (rotateTarget != Vector3.zero)
        {
            lookRotation = Quaternion.LookRotation(rotateTarget, Vector3.up);
        }

        //From what I understand Time Code stamp (10:00) * Time.fixedDeltaTime is causing the rotation to turn ased on the current frame, and not all at once.
        Vector3 rotation = Quaternion.RotateTowards(body.transform.rotation, lookRotation, turnSpeed * Time.fixedDeltaTime).eulerAngles;

        body.transform.eulerAngles = rotation;

        float temp = prevZ;

        if (prevZ < zTurn)
        {
            prevZ += Mathf.Min(turnSpeed * Time.fixedDeltaTime, zTurn - prevZ);
        }
        else if (prevZ >= zTurn)
        {
            prevZ -= Mathf.Min((turnSpeed * Time.fixedDeltaTime), prevZ - zTurn);
        }

        prevZ = Mathf.Clamp(prevZ, -45f, 45f);

        body.transform.Rotate(0f, 0f, prevZ - temp, Space.Self);

        //line below: multiplying the Euler by the forward facing vector will give the direction the bird is facing. Maybe by filtering out only what is being multiplied?
        direction = Quaternion.Euler(transform.eulerAngles) * Vector3.forward;

        //checking if distance is low enough to start reducing speed so object can glide into landing spot.
        if (pursuePlayer_ && distanceFromSwarmSpot < idleSpeed_)
        {
            body.velocity = Mathf.Min(idleSpeed_, distanceFromSwarmSpot) * direction;
        }
        //the lerp function is causing a smooth transition between the various speeds keeping the increase/decrease smooth rather than choppy. Will effect direction which is forward facing vector3.
        body.velocity = Mathf.Lerp(prevSpeed_, speed_, Mathf.Clamp(timeSinceAnim_ / switchSeconds, 0f, 1f)) * direction;

        if (body.transform.position.y < yMinMax.x + 10f || body.transform.position.y > yMinMax.y - 10f)
        {
            position = body.transform.position;
            //Line below is reseting the y to be equal to something within a clamped range. Then making the body.trans equal to the new position.
            //position.y is updated if the bird goes too low. body.trans is then updated with all positions (x,y,z) and then repositions itself depending on those new X,Y,Z inputs.
            position.y = Mathf.Clamp(position.y, yMinMax.x, yMinMax.y);
            body.transform.position = position;
        }
    }


    private void AnimationControl()
    {
        if (changeAnim < 0f)
        {
            prevAnim_ = currentAnim_;
            currentAnim_ = ChangeAnim(currentAnim_);
            changeAnim = Random.Range(changeAnimEveryFrameTo.x, changeAnimEveryFrameTo.y);
            timeSinceAnim_ = 0f;
            prevSpeed_ = speed_;
            if (currentAnim_ == 0) speed_ = idleSpeed_;
            else speed_ = Mathf.Lerp(moveSpeedMinMax.x, moveSpeedMinMax.y, (currentAnim_ - animSpeedMinMax.x) / (animSpeedMinMax.y - animSpeedMinMax.x));
        }
    }



    private void DroneMovement_Boundary()
    {
        //these (if sections) below rotate the y axis making the drone turn around.
        //If drone goes past any of these points: one of the axis will turn to send the drone in a different direction.
        if (body.transform.position.y < yMinMax.x || body.transform.position.y > yMinMax.y)
        {
            if (body.transform.position.y < yMinMax.x)
            {
                rotateTarget.y += 1f;
            }
            else
            {
                rotateTarget.y += -1f;
            }
        }
        if (body.transform.position.x < -xMinMax)
        {
            transform.Rotate(0f, transform.rotation.y + 180f, 0f, Space.World);
            transform.Translate(5f, 0f, 0f, Space.World);
        }
        if (body.transform.position.x > xMinMax)
        {
            transform.Rotate(0f, transform.rotation.y + 180f, 0f, Space.World);
            transform.Translate(-5f, 0f, 0f, Space.World);
        }

        if (body.transform.position.z < -zMinMax)
        {
            transform.Rotate(0f, transform.rotation.y + 180f, 0f, Space.World);
            transform.Translate(0f, 0f, 5f, Space.World);
        }
        if (body.transform.position.z > zMinMax)
        {
            transform.Rotate(0f, transform.rotation.y + 180f, 0f, Space.World);
            transform.Translate(0f, 0f, -5f, Space.World);
        }
    }

    private void PursuitBasedOnPlayerY()
    {
        if (this.position.y > yMinMax.x)
        {
            print("target above");

        }

        //if player below the y level
        if (this.position.y < yMinMax.x)
        {
            print("target below");
            Vector3 lowSwarmSpot_ = new Vector3(swarmSpot.transform.position.x, this.transform.position.y, swarmSpot.transform.position.z);//excludes the targets y so the swarm unit will ont pursue lower than the y point

            Vector3 lowMoveThere = Vector3.MoveTowards(transform.position, lowSwarmSpot_, idleSpeed_ * Time.deltaTime);


            transform.position = lowMoveThere;
            turnSpeedBackup = turnSpeed;
            turnSpeed = turnValue;
        }
    }

    private void Update()
    {
        
    }

    void FixedUpdate()
    {
        randomPatrol();
    }

    //Timestamp(11:00) if you wanna revisit this. If a method declares a type, in this case float,it also needs to return that type.
    private float ChangeAnim(float currentAnim)
    {
        float newState;
        if (Random.Range(0f, 1f) < idleRatio)
        {
            newState = 0f;
        }
        else
        {
            newState = Random.Range(animSpeedMinMax.x, animSpeedMinMax.y);
        }

        if (newState != currentAnim)
        {
            animator.SetFloat("flySpeed", newState);
            if (newState == 0)
            {
                animator.speed = 1f;
            }
            else
            {
                animator.speed = newState;
            }

        }
        return newState;
    }

    //If a method declares a type, in this case Vector3,it also needs to return that type.
    public Vector3 changeDirection(Vector3 currentPosition)
    {
        Vector3 newDir;

        //This area can be used to track if bots are too close to one another.
        if(pursuePlayer_ == true)
        {
            randomizedBase = swarmSpotTarget.position;
            //Line beneath controls the y axis of where the bot will go to return to position.
            randomizedBase.y += Random.Range(-randomBaseOffSet, randomBaseOffSet); 
            newDir = randomizedBase - currentPosition;
            //newDir = homeTarget.position - currentPosition;
        }
        else if(distanceFromTarget > radiueMinMax.y)
        {
            newDir = flyingTarget.position - currentPosition;
        }
        else if(distanceFromTarget < radiueMinMax.x)
        {
            newDir = currentPosition - flyingTarget.position;
        }
        else
        {
            //selecting any angle on the horizontal plane to keep the object fromgoing straight up or down. Also use .PI to receive a value. Not Sin or Cos because those work with radians.
            float angleXZ = Random.Range(-Mathf.PI, Mathf.PI);

            //single out y to control the steepnees of the up/down angles. Here you can change the straight up or down option.
            float angleY = Random.Range(-Mathf.PI / 40f, Mathf.PI / 40f);

            //Line below is turning the XZ into a forward vector, then into a right vector, and finally the angleY is converted into an up vetor.s
            newDir = Mathf.Sin(angleXZ) * Vector3.forward + Mathf.Cos(angleXZ) * Vector3.right + Mathf.Sin(angleY) * Vector3.up;
        }

        
        return newDir.normalized;
    }

}
