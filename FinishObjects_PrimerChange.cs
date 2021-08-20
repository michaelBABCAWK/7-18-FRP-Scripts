using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishObjects_PrimerChange : MonoBehaviour
{
    public Material[] currentColor;

    Renderer finishColor;

    // Start is called before the first frame update
    void Start()
    {

        finishColor = GetComponent<Renderer>();
        SetEventVariables();
    }

    private void SetEventVariables()
    {
        GameObject primerObject_ = GameObject.Find("Primer");
        Objective_Primer primerEvent_ = primerObject_.GetComponent<Objective_Primer>();

        primerEvent_.OnPrimerIgnited_ += ChargeReady;
        primerEvent_.OnPrimerOff_ += ChargeOff;
    }

    private void ChargeReady(object sender, EventArgs e)
    {
        finishColor.sharedMaterial = currentColor[1];//green
        gameObject.tag = "Finish";

    }

    private void ChargeOff(object sender, EventArgs e)
    {
        finishColor.sharedMaterial = currentColor[0];//black
        gameObject.tag = "Untagged";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
