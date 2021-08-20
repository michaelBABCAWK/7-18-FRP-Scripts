using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_MissleSmallBehavior : MonoBehaviour
{
    GameObject playerRef_;
    Transform playerSpot_;
    Rigidbody playerRigid_;

    [HideInInspector]
    public GameObject objectToFollow_;
    public Transform objectPosition_;

    [HideInInspector]
    public float speedToFollow_;
    [HideInInspector]
    public float speedToTurn_;
    [HideInInspector]
    public float maxDist_ = 50f;
    [HideInInspector]
    public float outOfFormationSpeedBoost_ = 5;//change this to adjust clmaped speed adjustment
    [HideInInspector]
    public float outOfFormationSpeed_;

    [HideInInspector]
    public int rangeToTrackPlayer_ = 35;

    bool trackingPlayer_;

    public void TrackingSpot()
    {
        trackingPlayer_ = false;
    }

    public void TrackingPlayer()
    {
        trackingPlayer_ = true;
    }

    private void Awake()
    {
        TrackingSpot();

        playerRef_ = GameObject.Find("Player");
        playerRigid_ = playerRef_.GetComponent<Rigidbody>();
        playerSpot_ = playerRef_.GetComponent<Transform>();
}

    private void LateUpdate()//boold control what the target is
    {
        float distance_ = Vector3.Distance(this.transform.position, playerSpot_.transform.position);

        if(distance_ < rangeToTrackPlayer_)//once this happens the missle tracks the player
        {
            TrackingPlayer();//now the misslet tracks the player
        }

        if(trackingPlayer_ == true)
        {
            outOfFormationSpeed_ = outOfFormationSpeedBoost_ + speedToFollow_;//Needs to move

            //section needs to be if Statement
            //if player is alive
            if(objectPosition_ != null)
            {
                float distanceToCalcSpeed_ = Vector3.Distance(this.transform.position, objectPosition_.transform.position);//will be used to calculate speed if the missles fall out of formation
                float speedBasedOnDistance_ = outOfFormationSpeed_ * (maxDist_ / distanceToCalcSpeed_);
                float minAndMaxSpeeds_ = Mathf.Clamp(speedBasedOnDistance_, speedToFollow_, 105f);//100f is the default speed speed set in the formation spawn method. so a n

                print(outOfFormationSpeed_);



                Vector3 lookAtSpotToFollow_ = new Vector3(playerSpot_.position.x,
            playerSpot_.position.y,
            playerSpot_.position.z);



                Vector3 directionsTowardsSpotToFollow_ = lookAtSpotToFollow_ - this.transform.position;



                this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                    Quaternion.LookRotation(directionsTowardsSpotToFollow_),
                    Time.deltaTime * speedToTurn_);



                this.transform.Translate(0, 0, minAndMaxSpeeds_ * Time.deltaTime);
            }


        }
        if (trackingPlayer_ == false)
        {
            if (objectPosition_ != null)
            {
                outOfFormationSpeed_ = outOfFormationSpeedBoost_ + speedToFollow_;//Needs to move

                //section needs to be if Statement
                float distanceToCalcSpeed_ = Vector3.Distance(this.transform.position, objectPosition_.transform.position);//will be used to calculate speed if the missles fall out of formation
                float speedBasedOnDistance_ = outOfFormationSpeed_ * (maxDist_ / distanceToCalcSpeed_);
                float minAndMaxSpeeds_ = Mathf.Clamp(speedBasedOnDistance_, speedToFollow_, 105f);//100f is the default speed speed set in the formation spawn method. so a n



                Vector3 lookAtSpotToFollow_ = new Vector3(objectPosition_.position.x,
            objectPosition_.position.y,
            objectPosition_.position.z);



                Vector3 directionsTowardsSpotToFollow_ = lookAtSpotToFollow_ - this.transform.position;



                this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                    Quaternion.LookRotation(directionsTowardsSpotToFollow_),
                    Time.deltaTime * speedToTurn_);



                this.transform.Translate(0, 0, minAndMaxSpeeds_ * Time.deltaTime);
            }
            else//if main missle gets destroyed
            {
                Destroy(gameObject, 5f);
            }

        }



    }
}
