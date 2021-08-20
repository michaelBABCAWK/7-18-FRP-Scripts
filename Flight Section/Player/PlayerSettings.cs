using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerSettings : MonoBehaviour
{
    [Header("Ship Speed")]
    //Values to determine speed of horizontal ad vertical movement.
    [Tooltip("In ms^-1")] [SerializeField] float xSpeed = 15;
    [Tooltip("In ms^-1")] [SerializeField] float ySpeed = 15;

    [Header("Boost Settings")]
    float baseBoost = 3.5f;
    float completeBoost;
    float rawNewzPos;

    [Header("Ship Movement Limits")]
    //Clamping values for horizontal and vertical values.
    [SerializeField] float xLimit = 25;
    [SerializeField] float yLimit = 5;

    [Header("Rotation Factors")]

    [SerializeField] float PositionPitchFactor = -5f;
    [SerializeField] float PositionYawFactor = -1f;
    [SerializeField] float PositionRollFactor = 20f;

    [Header("Pitch and Yaw Control Factors")]

    [SerializeField] float ControlPitchFactor = -10f;
    [SerializeField] float ControlYawFactor = -7f;

    [Header("System: Weapons")]

    [SerializeField] private int WeaponsActive;

    //[SerializeField] GameObject MainLaser;
    [SerializeField] GameObject LeftFront;
    [SerializeField] GameObject RightFront;

    [SerializeField] GameObject LeftWing;
    [SerializeField] GameObject RightWing;

    [SerializeField] Transform LFront;
    [SerializeField] Transform RFront;

    [SerializeField] Transform LWing;
    [SerializeField] Transform RWing;

    //[SerializeField] GameObject freePlayerObject;


    [Header("Shield Strength")]

    Rigidbody shipForce;

    //BetterWaypointFollower turnOffWaypoint;

    Player_ControlShip addFreeComponents;

    bool dead = false;

   // bool setFree = false;

    float xThrow, yThrow, zThrow;

    private void Start()
    {
        SetFromRepair();
        //turnOffWaypoint = FindObjectOfType<BetterWaypointFollower>();
        addFreeComponents = FindObjectOfType<Player_ControlShip>();
    }


    private void SetFromRepair()
    {
        //WeaponsActive = CannonObstacleActivation.completionFactor + WeaponsActive;
        WeaponsActive = 5;
        completeBoost = baseBoost * GameManager_BoostersObstacleActivation.completionFactor;

        shipForce = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            print(WeaponsActive);
            //ORDER OF ROTATIONS
            //TEST

            ProcessTranslation();
            ProcessRotation();
            ProcessFiring();
            //ProcessBoost();
        }

    }

    void weaponSystem(bool firingStatus)
    {
        //WeaponsActive = 5;

        LeftFront.transform.position = LFront.transform.position;
        RightFront.transform.position = RFront.transform.position;
        LeftWing.transform.position = LWing.transform.position;
        RightWing.transform.position = RWing.transform.position;



        if (WeaponsActive >= 1)
        {
            LeftFront.SetActive(firingStatus);
        }

        if (WeaponsActive >= 2)
        {
            RightFront.SetActive(firingStatus);
        }

        if (WeaponsActive >= 3)
        {
            LeftWing.SetActive(firingStatus);
        }

        if (WeaponsActive >= 4)
        {
            RightWing.SetActive(firingStatus);

        }
    }


    void Dead()//Called by string ref, CollisionHandler script.
    {
        dead = true;
    }

    private void ProcessRotation()
    {
        float pitchDueToPosition = transform.localPosition.y * PositionPitchFactor;
        float pitchDueToControlFactor = yThrow * ControlPitchFactor;

        float pitch = pitchDueToPosition + pitchDueToControlFactor;

        float yawDueToPosition = transform.localPosition.x * PositionYawFactor;
        float yawDueToControlFactor = xThrow * ControlYawFactor;

        float yaw = yawDueToPosition + yawDueToControlFactor;

        float roll = PositionRollFactor * xThrow;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    private void ProcessTranslation()
    {
        xThrow = CrossPlatformInputManager.GetAxis("Horizontal");
        yThrow = CrossPlatformInputManager.GetAxis("Vertical");

        //Setting speed for horizontal and vertical.
        float xOffset = xThrow * xSpeed * Time.deltaTime;

        float yOffset = yThrow * ySpeed * Time.deltaTime;


        //These float take the current position each frame and add the x/yOffsets to contnue growing/shrinking.
        float rawNewXPos = transform.localPosition.x + xOffset;

        float rawNewYPos = transform.localPosition.y + yOffset;


        //Clamping variables for horizontal and vertical limits.
        float ClampedXPos = Mathf.Clamp(rawNewXPos, -xLimit, xLimit);

        float ClampedYPos = Mathf.Clamp(rawNewYPos, -yLimit, yLimit);

        transform.localPosition = new Vector3(ClampedXPos, ClampedYPos, transform.localPosition.z);
    }


    private void ProcessFiring()
    {
        if (CrossPlatformInputManager.GetButton("Fire"))
        {
            weaponSystem(true);
        }
        else
        {
            weaponSystem(false);

        }
    }

}
