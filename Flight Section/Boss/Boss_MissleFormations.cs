using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_MissleFormations : MonoBehaviour
{

    Vector3 newRotationForMissles_;
    Vector3 spotsForMisslesToFllow_;//the missles does not need to match the rotation of the object just follow


    GameObject spotsForMissleToFollowObject;
    GameObject playerRef_;
    [HideInInspector]
    public GameObject formationMissles_;
    [HideInInspector]
    public GameObject refToTransformSpots;// gameobject of transform. Child of Main Missle 


    private int rotationForBabyMissleSpot;
    private int bigMissleToSpawn;
    private int setRotSpeed_ = 10;
    private int setMissleSpeed_ = 100;


    [HideInInspector]
    public Transform transformSpots_;
    private Transform spawnsToClone;
    Transform playerTrans_;


    private void Awake()
    {
        bigMissleToSpawn = 6;

        playerRef_ = GameObject.Find("Player");
        playerTrans_ = playerRef_.transform;

    }

    // Start is called before the first frame update
    void Start()
    {
        //this.CircleFormation();
        //CircleFormation();

        refToTransformSpots = gameObject.transform.GetChild(0).gameObject;//reference to child object of missle

        Invoke("CircleFormation", 2f);
    }

  


    public void CircleFormation()
    {

        //Vector3 targetPos_ = Vector3.zero;
        //targetPos_ = gameObject.transform.position;//set to parent objects pos

        for (int i = 0; i < bigMissleToSpawn; i++)//for every bigmissletospawn we loop and create a game object
        {
            rotationForBabyMissleSpot = 60 * i;

            spotsForMissleToFollow_(refToTransformSpots);

            //formationBabiesMethod(gameObject);//pos of instance is set outside instance and processed ebfore the return
        }
        //delay spawning from main missle

        //branch out from missle 

        //set so transform spawn and baby missle follows at varying speeds

        //Create Formation. Transforms spawned around main missle that baby missles try to stay with. 
        //This way on ly one object is tracking the player. The others are maintaing their formation until 
        //they reach a certain distance to the player then they break off and pursue

    }

    //empty game object needed to be replaced

    public GameObject spotsForMissleToFollow_(GameObject spotsForMissleToFollow)//this will return an object for the smaller missles to follow
    {
        //call make missle in this script too? for int i loop.

        //thisSpot_ = spotsForMissleToFollow;//everytime i++ this gameobject is set to what parameter was used in the Gameobject method call

        spotsForMissleToFollow = Instantiate(refToTransformSpots, spotsForMissleToFollow.transform.position, gameObject.transform.rotation);


        //setting variables to make sure tracking object is facing player
        Vector3 targetDirection = playerTrans_.position - transform.position;
        // The step size is equal to speed times frame time.
        float singleStep = 100f * Time.deltaTime;
        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
        // Draw a ray pointing at our target in
        Debug.DrawRay(transform.position, newDirection, Color.red);
        // Calculate a rotation a step closer to the target and applies rotation to this object
        transform.rotation = Quaternion.LookRotation(newDirection);


        //spotsForMissleToFollow.transform.rotation = Quaternion.Euler(0f, 0f, rotationForBabyMissleSpot);

        spotsForMissleToFollow.transform.Rotate(0, 0, rotationForBabyMissleSpot);//set position after new rotation is set

        spotsForMissleToFollow.transform.Translate(0, 20, -35, Space.Self);//this line will move in any axis with a value. The clone will move the value that is placed in its axis spot.

        spotsForMissleToFollow.transform.SetParent(gameObject.transform);

        spotsForMissleToFollowObject = spotsForMissleToFollow;//equal to new transform clone
        //gonna be a gameobject. use movetowards on missle objects

        formationBabiesMethod(gameObject);//this will create the object that will follow the position
        //gameobject would be the missle

        return spotsForMissleToFollow;//does not need to return parameter

    }

    //with formation missles: want them to follow a "path" of gameobejcts being spawned/ object pooled by the main spot to follow. This will allow the homing missles to stay in a consistent formation. Even if the spotToFollow changes orientation 
    //during a turn
    public GameObject formationBabiesMethod(GameObject bigMissle)//created this function to remove missleFormation class from all children
    {
        GameObject localSpotToFollow_ = spotsForMissleToFollowObject;//reference set in previous spotsForMissleToFollow method

        Boss_MissleSmallBehavior trackingVariables_;//small missle variables

        //set rotation in for loop outside of this method
       //showing values

        formationMissles_ = Instantiate(bigMissle, bigMissle.transform.position, Quaternion.identity);//set position after new rotation is set

        formationMissles_.AddComponent<Boss_MissleSmallBehavior>();//after this behaviour is assigned we can adjust the variables inside to follow turn and back off as we like
        Destroy(formationMissles_.GetComponent<Boss_MissleBehavior>());
        Destroy(formationMissles_.GetComponent<Boss_MissleFormations>());//stop clones having clones


        for(int i = 0; i < formationMissles_.transform.childCount; i++)//without this each new missles has the transform gameobject
        {
            Destroy(formationMissles_.transform.GetChild(i).gameObject);

        }

        trackingVariables_ = formationMissles_.GetComponent<Boss_MissleSmallBehavior>();//holds variables to set in order for the smaller missles to track.
        //might work better with an object pooler typeof

        trackingVariables_.objectToFollow_ = localSpotToFollow_;//this needs to be set to the leading object gameobject
        trackingVariables_.objectPosition_ = localSpotToFollow_.transform;//this needs to be set to the gameojbects transform
        trackingVariables_.speedToFollow_ = setMissleSpeed_;//move speed
        trackingVariables_.speedToTurn_ = setRotSpeed_;//turn speed


        formationMissles_.transform.localScale = new Vector3(.75f, .75f, .75f);

        return formationMissles_;//does not need to return parameter

    }





}

