using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swarm_SphereCastAvoidance : MonoBehaviour
{
    //public GameObject _currentHitObject;

    public LayerMask _mask;
    public float _sphereRadius;
    public float _maxDistance;


    private Vector3 _origin;
    private Vector3 _direction;

    private float _currentHItDistance;

    // Update is called once per frame
    void Update()
    {
        //TutorialMethod();
        personalMethod();
    }


    private void personalMethod()
    {
        _origin = transform.position;
        _direction = transform.forward;

        RaycastHit _hit;

        //Vector3 _hitSpot;

        //float _moveAway;

        if (Physics.SphereCast(_origin, _sphereRadius, _direction, out _hit, _maxDistance, _mask, QueryTriggerInteraction.UseGlobal))
        {
            Vector3 _contactPoint;
            //Call method in here on Swarm_MovementBehavior to override ChangeDirection method
            //print("Collision with OBject");
            _contactPoint = _hit.point;
            //print(_contactPoint);
           // print(_hit.collider.name);
            this.transform.position = Vector3.MoveTowards(this.transform.position, _contactPoint, -10f);
        }
        else
        {
            _currentHItDistance = _maxDistance;
            //_currentHitObject = null;

        }
    }




    private void TutorialMethod()
    {
        _origin = transform.position;
        _direction = transform.forward;

        RaycastHit _hit;

        if (Physics.SphereCast(_origin, _sphereRadius, _direction, out _hit, _maxDistance, _mask, QueryTriggerInteraction.UseGlobal))
        {
            //_currentHitObject = _hit.transform.gameObject;

            _currentHItDistance = _hit.distance;
        }
        else
        {
            _currentHItDistance = _maxDistance;
            //_currentHitObject = null;

        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(_origin, _origin + _direction * _currentHItDistance);

        Gizmos.DrawWireSphere(_origin + _direction, _sphereRadius);
    }

}
