using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]

public class Obstacles_Oscillate : MonoBehaviour
{
    [Header("Spark Control")]
    [SerializeField] ParticleSystem sparkToTrigger;
    [SerializeField] float onOrOffValue_;


    [Header("Directions To Move")]
    [SerializeField] Vector3 _movementVector = new Vector3(10f, 10f, 10f);
    [Range(0, 1)] public float _movementFactor;


    [Header("Time To Complete Cycle")]
    [SerializeField] float _period = 2f;

    [Header("Slide Audio")]
    [SerializeField] AudioSource audioOfObstacle_;


    Vector3 _startingPos;


    public bool _stayStill = true;

    // Start is called before the first frame update
    void Start()
    {
        _startingPos = transform.position; //Sets position for current space
        //audioOfObstacle_ = gameObject.GetComponent<AudioSource>();
    }

    public void activateOscillate()
    {
        _stayStill = false;

        audioOfObstacle_.Play();
    }

    public void particleEmissionControl(bool emissionControl)
    {
        var spark = sparkToTrigger.emission;
        spark.enabled = emissionControl;
    }

    // Update is called once per frame
    void Update()
    {
        if (_stayStill == false)
        {
            OscillateMethod();

            ControlSpark();
        }

    }

    private void OscillateMethod()
    {
        float cycles = Time.time / _period; // grows continually from 0.

        const float tau = Mathf.PI * 2f;
        float rawSinWave = Mathf.Sin(cycles * tau);

        _movementFactor = rawSinWave / 2f + 0.5f;
        Vector3 offset = _movementVector * _movementFactor;
        transform.position = _startingPos + offset;
    }

    private void ControlSpark()
    {

        if (this._movementFactor >= onOrOffValue_)//Triggers Emissions
        {
            particleEmissionControl(true);
        }
        else if(this._movementFactor <= onOrOffValue_)
        {
            particleEmissionControl(false);
        }






    }
}
