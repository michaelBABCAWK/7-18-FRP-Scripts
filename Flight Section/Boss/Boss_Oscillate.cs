using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Oscillate : MonoBehaviour
{
    [SerializeField] Vector3 _movementVector = new Vector3(10f, 10f, 10f);

    [SerializeField] public float _period;//Time to go through cycle

    [SerializeField] private float _timeToPause;//Amount of time cycle stops.

    [Range(0, 1)] [SerializeField] public float _movementFactor;


    Vector3 startingPos_;//pos at start
    Vector3 stopHighPos_;//pause float value at this pos// = new Vector3(0, 447, 0)
    bool beenUp_;
    Vector3 stopLowPos_;//pause float value at this pos// = new Vector3(0, 1, 0)
    bool beenDown_;

    float currentCycleValue_;

    bool keepCounting_;//controls cycle interpolating

    private void Awake()
    {
        stopHighPos_ = new Vector3(0, gameObject.transform.position.y + (_movementVector.y - 10f), 0);//pause float value at this pos
        stopLowPos_ = new Vector3(0, gameObject.transform.position.y + 10f, 0);//pause float value at this pos



        keepCounting_ = true;
        beenUp_ = false;
        beenDown_ = true;//keep true so boss comes up from tube

        startingPos_ = gameObject.transform.position;
    }

    private void Update()
    {
        //OldDisableOscillateMethod();

    }

    private void OldDisableOscillateMethod()
    {
        if (transform.position.y >= stopHighPos_.y && beenUp_ == false)
        {
            changeShaderToDissolve();//turns off shield


            beenUp_ = true;//now the if statement will not keep the Boss frozen in place

            keepCounting_ = false;//stops Boss movement

            Invoke("ContinueCounting", _timeToPause);//contibue movement

            beenDown_ = false;//resets low Pause so it can pause when down
        }
        else if (transform.position.y <= stopLowPos_.y && beenDown_ == false)
        {
            changeShaderToDissolve();//turns off shield


            beenDown_ = true;//now the if statement will not keep the Boss frozen in place

            keepCounting_ = false;//stops Boss movement

            Invoke("ContinueCounting", _timeToPause);//contibue movement

            beenUp_ = false;//resets high Pause so it can pause when up


        }
    }

    public void changeShaderToDissolve()//turn off collider on Shield
    {
        GameObject shield_ = GameObject.Find("Shield");
        SphereCollider theCollider_ = shield_.GetComponent<SphereCollider>();
        Renderer shieldMat_ = shield_.GetComponent<Renderer>();
        Boss_ShaderCollection shaderRef_ = shield_.GetComponent<Boss_ShaderCollection>();

        theCollider_.enabled = false;

        shieldMat_.sharedMaterial = shaderRef_.bossMaterials_[1];
    }


    public void changeShaderToOn()//turn off collider on Shield
    {
        GameObject shield_ = GameObject.Find("Shield");
        SphereCollider theCollider_ = shield_.GetComponent<SphereCollider>();
        Renderer shieldMat_ = shield_.GetComponent<Renderer>();
        Boss_ShaderCollection shaderRef_ = shield_.GetComponent<Boss_ShaderCollection>();

        theCollider_.enabled = false;

        shieldMat_.sharedMaterial = shaderRef_.bossMaterials_[0];
    }

    private void LateUpdate()
    {
        OscillateMethod();//moved out until the new short-circuit mechanics work

        /*
        if (keepCounting_ == true)
        {
            currentCycleValue_ += 0.5f * Time.deltaTime;

        }
        */
    }

    public void ContinueCounting()//also turn on collider and changes mat back
    {
        GameObject shield_ = GameObject.Find("Shield");
        SphereCollider theCollider_ = shield_.GetComponent<SphereCollider>();

        changeShaderToOn();

        theCollider_.enabled = true;

        keepCounting_ = true;
    }


    private void OscillateMethod()//two methods? one with a neg value and a pos value to keep boss position correct
    {
        //float _cycles = currentCycleValue_ / _period; // grows continually from 0 depending on status of a bool
        float _cycles = Time.time / _period; 

        const float tau = Mathf.PI * 2f;//tau = pi * 2 or 2pi
        float rawSinWave = Mathf.Sin(_cycles * tau);

        _movementFactor = rawSinWave / 2f + 0.5f;
        Vector3 offset = _movementVector * _movementFactor;
        transform.position = startingPos_ + offset;
    }
}
