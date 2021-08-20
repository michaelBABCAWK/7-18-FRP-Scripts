using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_MissleBehavior : MonoBehaviour
{

    public GameObject playerRef_;
    Transform playerPositioin_;
    Player_ControlShip playerMethods_;

    LayerMask playerMask = 12;

    [SerializeField] float speed_;
    [SerializeField] float rotSpeed_;
    [SerializeField] float autoDetonation_;
    [SerializeField] float detonationRange;


    Vector3 directionTowardsPlayer_;

    bool trackingPlayer_;

    //Trigger event on being instantiated

    // Start is called before the first frame updat

    private void Awake()
    {
        playerRef_ = GameObject.Find("Player");
        playerPositioin_ = playerRef_.transform;

        //Raycast that shoots out from the missle point. If it hits anything before the player, no hit. if the ray hits the player, a hit.

    }

    void Start()
    {

    }

    private void OnParticleCollision(GameObject other)
    {
        Destroy(gameObject);
    }

    void LateUpdate()
    {
        Vector3 lookAtPlayer_ = new Vector3(playerPositioin_.position.x,
            playerPositioin_.position.y,
            playerPositioin_.position.z);

        directionTowardsPlayer_ = lookAtPlayer_ - this.transform.position;

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
            Quaternion.LookRotation(directionTowardsPlayer_),
            Time.deltaTime * rotSpeed_);



        if (Vector3.Distance(transform.position, lookAtPlayer_) > detonationRange)
        {
            this.transform.Translate(0, 0, speed_ * Time.deltaTime);
        }
        else
        {
            ShootRayAtPlayer();

            Debug.DrawRay(this.transform.position, directionTowardsPlayer_, Color.red);
        }

    }



    private void ShootRayAtPlayer()
    {
        RaycastHit hit;

        Vector3 direction = playerRef_.transform.position - this.transform.position;


        if (Physics.Raycast(this.transform.position, direction, out hit))
        {





            if (hit.collider.tag == "Player")
            {


                Player_Shield damageCall;

                damageCall = hit.collider.gameObject.GetComponent<Player_Shield>();

                if(damageCall != null)
                {
                    damageCall.DamageCalls(12);

                    Destroy(gameObject);
                }
            }
            else
            {




            }
        }
    }
}
