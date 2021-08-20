
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Player_ControlShip : MonoBehaviour
{
   

    [Header("Components In Ship")]
    [SerializeField] GameObject partsToRotate_;


    [Header("Ship Direction Rotation Factors")]
    [SerializeField] private Vector3 rotateShipOnTurn_;


    public float mouseSpeedToTurnCamera_;
    public float buttonSpeedToTurnCamera_;


    [Header("Ship Body Rotation Speeds")]
    [SerializeField] float mouseSpeedToRotateShipBody_;
    [SerializeField] float buttonSpeedToRotateShipBody_;
    [SerializeField] float buttonSpeedToRollShipBody_;



    [Header("Pitch and Yaw Control Factors")]

    bool dead = false;
    [HideInInspector] public bool paused = true;


    [HideInInspector] public float completeBoost;
    [HideInInspector] public float boostingSpeed_;
    [HideInInspector] public float clampedSpeed_;
    float cameraYawValue_, cameraPitchValue_, cameraRollValue_;

    [Header("Range of Ship Body Rotation")]
    [SerializeField] private float autoReduceBodyRotation_ = 1.5f;

    [Range(-15, 15)]
    [SerializeField] private float shipBodyPitchRange_;
    [Range(-15, 15)]
    [SerializeField] private float shipBodyYawRange_;
    [Range(-15, 15)]
    [SerializeField] private float shipBodyRollRange_;

    private Vector3 rotRefForRolling_;


    private void Awake()
    {
        Time.timeScale = 0f;//starts time for game

        //gameObject.AddComponent<compto>();
    }

    public void notPuased()
    {
        paused = false;
        Time.timeScale = 1f;//starts time for game

    }

    public void Dead()
    {
        dead = false;
    }

 
    public void RanOutOfTime()
    {
        dead = true;
    }

    private void ShipBodyPitch()//Turns the ship model not the player cam
    {
        if (CrossPlatformInputManager.GetAxis("Pitch") != 0)
        {

            float _pitch = CrossPlatformInputManager.GetAxis("Pitch") * rotateShipOnTurn_.x;//-2 to 2
            float _fixedPitch = _pitch * Time.fixedDeltaTime;
            float _pitchAdd = _fixedPitch * mouseSpeedToRotateShipBody_;
            float _pitchClamp = shipBodyPitchRange_ += _pitchAdd;
            shipBodyPitchRange_ = Mathf.Clamp(_pitchClamp, -rotateShipOnTurn_.x, rotateShipOnTurn_.x);
        }
        else if (Input.GetAxis("Mouse Y") != 0)
        {
            float _pitch = Input.GetAxis("Mouse Y") * -rotateShipOnTurn_.x;
            float _fixedPitch = _pitch * Time.fixedDeltaTime;
            float _pitchAdd = _fixedPitch * mouseSpeedToRotateShipBody_;
            float _pitchClamp = shipBodyPitchRange_ += _pitchAdd ; 
            shipBodyPitchRange_ = Mathf.Clamp(_pitchClamp, -rotateShipOnTurn_.x, rotateShipOnTurn_.x);
        }
        else if (Input.GetAxis("Mouse Y") == 0 && CrossPlatformInputManager.GetAxis("Pitch") == 0)
        {
            float _reducePitch = shipBodyPitchRange_;
            float _reduce = 1;
            _reduce -= autoReduceBodyRotation_;
            shipBodyPitchRange_ = _reducePitch * _reduce;
        }
    }


    private void ShipBodyYaw()//Turns the ship model not the player cam
    {
        if (CrossPlatformInputManager.GetAxis("Yaw") != 0)
        {

            float _yaw = CrossPlatformInputManager.GetAxis("Yaw") * rotateShipOnTurn_.y;//-2 to 2
            float _fixedYaw = _yaw * Time.fixedDeltaTime;
            float _yawAdd = _fixedYaw * mouseSpeedToRotateShipBody_;
            float _yawClamp = shipBodyYawRange_ += _yawAdd;
            shipBodyYawRange_ = Mathf.Clamp(_yawClamp, -rotateShipOnTurn_.y, rotateShipOnTurn_.y);
        }
        else if (Input.GetAxis("Mouse X") != 0)
        {
            float _yaw = Input.GetAxis("Mouse X") * rotateShipOnTurn_.y;
            float _fixedYaw = _yaw * Time.fixedDeltaTime;
            float _yawAdd = _fixedYaw * mouseSpeedToRotateShipBody_;
            float _yawClamp = shipBodyYawRange_ += _yawAdd;
            shipBodyYawRange_ = Mathf.Clamp(_yawClamp, -rotateShipOnTurn_.y, rotateShipOnTurn_.y);
        }
        else if (Input.GetAxis("Mouse X") == 0 && CrossPlatformInputManager.GetAxis("Yaw") == 0)
        {
            float _reducedYaw = shipBodyYawRange_;
            float _reduce = 1;
            _reduce -= autoReduceBodyRotation_;
            shipBodyYawRange_ = _reducedYaw * _reduce;
        }
    }



    private void ShipBodyRotations()
    {
        ShipBodyPitch();
        ShipBodyYaw();

        partsToRotate_.transform.localRotation = Quaternion.Euler(partsToRotate_.transform.localRotation.x + shipBodyPitchRange_,
            partsToRotate_.transform.localRotation.y + shipBodyYawRange_, 
            partsToRotate_.transform.localRotation.z);

    }


    private void PlayerHorizontalTurn()//not rotating the ship body//not cam sway
    {

        if (CrossPlatformInputManager.GetAxis("Mouse X") != 0.0f)
        {
            cameraYawValue_ = mouseSpeedToTurnCamera_ * CrossPlatformInputManager.GetAxis("Mouse X");

            float _fixedYaw = cameraYawValue_ * Time.fixedDeltaTime;

            transform.Rotate(Vector3.up * _fixedYaw, Space.Self);
        }
        else if (CrossPlatformInputManager.GetAxis("Yaw") != 0.0f)
        {
            cameraYawValue_ = buttonSpeedToTurnCamera_ * CrossPlatformInputManager.GetAxis("Yaw");

            float _fixedYaw = cameraYawValue_ * Time.fixedDeltaTime;

            transform.Rotate(Vector3.up * _fixedYaw, Space.Self);
        }
    }

    private void PlayerVerticalTurn()//not rotating the ship body//not cam sway
    {

        if(Input.GetAxis("Mouse Y") != 0)
        {
            cameraPitchValue_ = mouseSpeedToTurnCamera_ * CrossPlatformInputManager.GetAxis("Mouse Y");

            float _fixedPitch = cameraPitchValue_ * Time.fixedDeltaTime;

            float _adjustedPitch = _fixedPitch * -1f;

            transform.Rotate(Vector3.right * _adjustedPitch, Space.Self);
        }

        else if (CrossPlatformInputManager.GetAxis("Pitch") != 0.0f)
        {
            cameraPitchValue_ = buttonSpeedToTurnCamera_ * CrossPlatformInputManager.GetAxis("Pitch");

            float _fixedPitch = cameraPitchValue_ * Time.fixedDeltaTime;

            float _adjustedPitch = _fixedPitch * -1f;

            transform.Rotate(Vector3.right * _adjustedPitch * -1f, Space.Self);


        }

    }

    private void PlayerPerspectiveRoll()//not rotating the ship body//not cam sway
    {

        if (CrossPlatformInputManager.GetAxis("Roll") != 0.0f)//if not zero
        {
            cameraRollValue_ = buttonSpeedToRollShipBody_ * CrossPlatformInputManager.GetAxis("Roll");

            float _fixedRoll = cameraRollValue_ * Time.fixedDeltaTime;

            float _adjustedRoll = _fixedRoll * -1;

            transform.Rotate(Vector3.forward * _adjustedRoll, Space.Self);

        }

    }


    void RotateMethods()
    {
        //camera and player rotation
        PlayerHorizontalTurn();
        PlayerVerticalTurn();
        PlayerPerspectiveRoll();


        //ship body movement
        ShipBodyRotations();
    }



    private void Update()
    {
        if (!dead && !paused)
        {
            RotateMethods();

        }
    }

    private void FixedUpdate()
    {



    }

}
