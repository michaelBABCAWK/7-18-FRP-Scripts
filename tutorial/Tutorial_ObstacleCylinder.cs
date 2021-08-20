using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_ObstacleCylinder : MonoBehaviour
{
    [SerializeField] Vector3 _movementVector = new Vector3(10f, 10f, 10f);

    [SerializeField] float _period = 2f;

    [Range(0, 1)] [SerializeField] public float _movementFactor;

    Vector3 _startingPos;

    [SerializeField] ParticleSystem sparkToTrigger;

    public bool _stayStill = true;

    // Start is called before the first frame update
    void Start()
    {
        _startingPos = transform.position; //Sets position for current space
    }

    public void activateOscillate()
    {
        _stayStill = false;
    }

    public void particleEmissionControl(bool emissionControl)
    {
        var spark = sparkToTrigger.emission;
        spark.enabled = emissionControl;
    }

    // Update is called once per frame
    void Update()
    {
        OscillateMethod();

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

}
