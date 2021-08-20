using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public class Player_CameraSwayOnMovement : MonoBehaviour
{
    [Header("Range of Camera Sway")]

    [SerializeField] private Vector3 camSwayMax_;


    [Range(-15, 15)]
    [SerializeField] private float camBodyPitchSway_;
    private float pitchSway_;

    [Range(-15, 15)]
    [SerializeField] private float camBodyYawSway_;
    private float yawSway_;

    [Range(-15, 15)]
    [SerializeField] private float camBodyRollSway_;
    private float currentRoll_;


    [Header("Ship Direction Rotation Factors")]
    [SerializeField] float mouseSpeedToSwayCamera_;
    [SerializeField] float buttonSpeedToSwayCameraYaw_;
    [SerializeField] float buttonSpeedToSwayCameraPitch_;


    [Header("GameObjects")]
    [SerializeField] private GameObject cameraObject_;
    [SerializeField] private GameObject basePosObject_;




    float _limitedRange;



    private void CameraYawSway()
    {
        if (CrossPlatformInputManager.GetAxis("Yaw") != 0)
        {
            yawSway_ = CrossPlatformInputManager.GetAxis("Yaw") * camSwayMax_.x;//controlled yaw not pitch

            float _fixedSway = yawSway_ * Time.fixedDeltaTime;

            camBodyYawSway_ = _fixedSway * buttonSpeedToSwayCameraYaw_;
        }
        else if (Input.GetAxis("Mouse X") != 0)
        {
            yawSway_ = Input.GetAxis("Mouse X") * camSwayMax_.x;//controlled yaw not pitch

            float _fixedSway = yawSway_ * Time.fixedDeltaTime;

            camBodyYawSway_ = _fixedSway * mouseSpeedToSwayCamera_;        
        }

    }

    private void CameraPitchSway() 
    {
         if (CrossPlatformInputManager.GetAxis("Pitch") != 0)
        {
            pitchSway_ = CrossPlatformInputManager.GetAxis("Pitch") * camSwayMax_.y;

            float _fixedSway = pitchSway_ * Time.fixedDeltaTime;

            camBodyPitchSway_ = _fixedSway * buttonSpeedToSwayCameraPitch_;
        }

        else if (Input.GetAxis("Mouse Y") != 0)
        {
            pitchSway_ = Input.GetAxis("Mouse Y") * camSwayMax_.y;

            float _fixedSway = pitchSway_ * Time.fixedDeltaTime;

            camBodyPitchSway_ = _fixedSway * mouseSpeedToSwayCamera_;
        }
    }


    private void CameraPushBack()
    {
        Player_BoostingSystem speedValue_ = gameObject.GetComponent<Player_BoostingSystem>();

        if (Input.GetAxis("Boost") > 0)
        {
            float _currentDivideByMaxSpeed = speedValue_.regulatedSpeed_ / speedValue_.maxSpeed_;

            float _currentPushBackValue = _currentDivideByMaxSpeed * camSwayMax_.z;
            float _fixedCurrentPushBackValue = _currentPushBackValue * Time.fixedDeltaTime;

            float _baseDivideBySpeed = speedValue_.baseSpeed_ / speedValue_.maxSpeed_;

            float _adjust = _baseDivideBySpeed * camSwayMax_.z;
            float _fixedAdjust = _adjust * Time.fixedDeltaTime;

            float _camPushBack = _fixedCurrentPushBackValue - _fixedAdjust;


            _limitedRange = Mathf.Clamp(_camPushBack, 0, camSwayMax_.z);
        }
        else if (Input.GetAxis("Boost") == 0)
        {
            if (_limitedRange > 0)
            {
                float _toZero = 1;
                float _backValue = _limitedRange;

                _toZero -= .1f;//reduces ti zero to get roll * 0

                _limitedRange = _backValue * _toZero;
            }
            else if(_limitedRange <= 0)
            {

            }
        }

        cameraObject_.transform.Translate(Vector3.back * _limitedRange, Space.Self);
    }

    private void CameraSwayMethods()
    {
        CameraPitchSway();

        CameraYawSway();//left to right

        CameraPushBack();


        float _xVal = basePosObject_.transform.localPosition.x;
        float _yVal = basePosObject_.transform.localPosition.y;
        float _zVal = basePosObject_.transform.localPosition.z;


        cameraObject_.transform.localPosition = new Vector3(_xVal + camBodyYawSway_ * 1.25f,_yVal + camBodyPitchSway_ * 1.25f, _zVal);

        // 

    }

    // Update is called once per frame
    void Update()
    {

        CameraSwayMethods();

        //CameraRoll();



    }



}
