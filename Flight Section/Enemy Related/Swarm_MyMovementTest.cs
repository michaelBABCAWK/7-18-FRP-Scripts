using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swarm_MyMovementTest : MonoBehaviour
{
    public float _moveSpeed;
    float _adjustedSpeed;

    float _rangeToMoveX;
    float _rangeToMoveY;
    float _rangeToMoveZ;

    public float _turnSpeed;
    float _adjustedTurn;

    Vector3 _distanceToMoveForward;

    Vector3 _newRotation;

    bool _contactWithObject = false;

    bool _pursuePlayer = false;

    private IEnumerator rotationCall;

    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine("newRotationCall", 10f);

        //SetNewPosition();
    }

    private IEnumerator newRotationCall( float newTime)
    {

        yield return new WaitForSeconds(newTime);

        SetNewRotations();
    }
    
    public void SetNewPosition()
    {


        _distanceToMoveForward = new Vector3(transform.localPosition.x + _rangeToMoveX, transform.localPosition.y + _rangeToMoveY, transform.localPosition.z + _rangeToMoveZ);//This is constantly adding Up. So it will always go up

        transform.localPosition = Vector3.MoveTowards(this.transform.position, _distanceToMoveForward, _adjustedSpeed);// when destination is met or an obstacle is hit change direction and get new distance.

        print(transform.position);
    }

    public void SetNewRotations()
    {
        _rangeToMoveX = Random.Range(0, 90);

        _rangeToMoveY = Random.Range(0, 360);

        _rangeToMoveZ = Random.Range(0, 90);

        _newRotation = new Vector3(_rangeToMoveX, _rangeToMoveY, _rangeToMoveZ);

        transform.eulerAngles = _newRotation;
    }


   /*
    public Vector3 changeDirection(Vector3 currentPosition)
    {
        Vector3 newDir;

        //This area can be used to track if bots are too close to one another.
        if (_pursuePlayer == true)
        {
            //Set to go towards Swarm spot
        }
        else if (distanceFromTarget > radiueMinMax.y)
        {
            newDir = flyingTarget.position - currentPosition;
        }
        else if (distanceFromTarget < radiueMinMax.x)
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


        //return newDir.normalized;
    }
    */




    private void OnCollisionEnter(Collision collision)
    {
        _contactWithObject = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        _contactWithObject = false;
    }






    private void FixedUpdate()
    {
        transform.position += Vector3.forward* _adjustedSpeed;



        //Should be Vector Force implimentation. Adding values to x,y,z will just send the bot off in the "pos" direction.

        //SetNewPosition();

        //SetNewRotations();

        /*
        if (transform.position == _distanceToMoveForward)
        {
            SetNewPosition();

            SetNewRotations();

        }
        else if (_contactWithObject)
        {
            //adjust rotation and position to be away from collision
            SetNewPosition();
        }
        */
    }


    // Update is called once per frame
    void Update()
    {
        _adjustedTurn = _turnSpeed * Time.deltaTime;

        _adjustedSpeed = _moveSpeed * Time.deltaTime;
    }
}
