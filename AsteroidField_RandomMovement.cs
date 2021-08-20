using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidField_RandomMovement : MonoBehaviour
{
    public float minSpinSPeed_ = 1f;
    public float maxSpinSPeed_ = 5f;
    public float minThrust_ = .1f;
    public float maxThrust_ = .75f;
    private float spinSpeed_;
    private float thrust_;

    private void Start()
    {
        spinSpeed_ = Random.Range(minSpinSPeed_, maxSpinSPeed_);
        thrust_ = Random.Range(minThrust_, maxThrust_);


    }

    private void Update()
    {
        transform.Rotate(Vector3.up, spinSpeed_ * Time.fixedDeltaTime, Space.Self);
        transform.Translate(0f, 0f, thrust_, Space.Self);

    }

}
