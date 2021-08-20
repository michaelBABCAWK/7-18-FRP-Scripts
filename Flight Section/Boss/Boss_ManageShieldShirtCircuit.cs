using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_ManageShieldShirtCircuit : MonoBehaviour
{
    Boss_Oscillate shutOffPeriodScriptRef_;



    float shutOffPeriod_;//referenced from the Oscillate method
    public float reselectTheGenPeriod_;
    private void Awake()
    {
        shutOffPeriodScriptRef_ = gameObject.GetComponentInParent<Boss_Oscillate>();

        shutOffPeriod_ = shutOffPeriodScriptRef_._period;
    }

    private void OnTriggerEnter(Collider other)//once shield hits trigger: Disable
    {//working. Now disable shield and then trigger again after period of time
        if(other.gameObject.tag == "Shorted Out")
        {

            changeShaderToDissolve();

            Invoke("changeShaderToOn", shutOffPeriod_);//turn back on after Oscillate period time

            other.gameObject.tag = "Untagged";
        }
        else
        {

        }
    }


    public void changeShaderToDissolve()//turn off collider on Shield
    {

        GameObject shield_ = GameObject.Find("Shield");//Gameobject with material references
        Boss_ShaderCollection shaderRef_ = shield_.GetComponent<Boss_ShaderCollection>();//script with material ref on the gameobject referenced above
        SphereCollider theCollider_ = shield_.GetComponent<SphereCollider>();//collider on shield object
        Renderer shieldMat_ = shield_.GetComponent<Renderer>();//renderer for the shield object


        theCollider_.enabled = false;//collider on shield object

        shieldMat_.sharedMaterial = shaderRef_.bossMaterials_[1];//changes the material on the shield
    }



    public void changeShaderToOn()//turn off collider on Shield
    {
        GameObject shield_ = GameObject.Find("Shield");
        SphereCollider theCollider_ = shield_.GetComponent<SphereCollider>();
        Renderer shieldMat_ = shield_.GetComponent<Renderer>();
        Boss_ShaderCollection shaderRef_ = shield_.GetComponent<Boss_ShaderCollection>();


        theCollider_.enabled = true;

        shieldMat_.sharedMaterial = shaderRef_.bossMaterials_[0];

        Invoke("TriggerTheGenSelect", reselectTheGenPeriod_);//turn back on after Oscillate period time

    }

    public void TriggerTheGenSelect()
    {
        //event system?
        GameObject theGenSelector_ = GameObject.Find("Gen Select");
        BossTube_GenSelect GenSelectRef_ = theGenSelector_.GetComponent<BossTube_GenSelect>();

        GenSelectRef_.SelectRandomGen();//calls method to select new gen
    }
}
