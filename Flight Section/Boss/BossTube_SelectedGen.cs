using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTube_SelectedGen : MonoBehaviour
{
    GameObject genSelector_;//gameobject that holds the material ref
    GameObject shortCircuitPlane_;



    Transform genChild_;



    BossTube_GenSelect scriptRef_;//script that holds the material varable



    private float health_ = 100f;//health



    Renderer rendererOnChosenGen_;//this gameobjects renderer



    Material newColorOnChosenGen_;//material to assign to this gameobject



    private void Awake()
    {
        ChangingChosenGenToGreen();
    }

    private void ChangingChosenGenToGreen()
    {
        genSelector_ = GameObject.Find("Gen Select");//gameobject has material to put on THIS gameobject
        scriptRef_ = genSelector_.GetComponent<BossTube_GenSelect>();//other objects script that has the material
        newColorOnChosenGen_ = scriptRef_.newColorOnChosenGen_[1];//material from other objects script

        rendererOnChosenGen_ = gameObject.GetComponent<Renderer>();

        rendererOnChosenGen_.material = newColorOnChosenGen_;//changing material from red to green

    }

    public void chosenGenBackToRed()
    {
        genSelector_ = GameObject.Find("Gen Select");//gameobject has material to put on THIS gameobject
        scriptRef_ = genSelector_.GetComponent<BossTube_GenSelect>();//other objects script that has the material
        newColorOnChosenGen_ = scriptRef_.newColorOnChosenGen_[0];//material from other objects script

        rendererOnChosenGen_ = gameObject.GetComponent<Renderer>();

        rendererOnChosenGen_.material = newColorOnChosenGen_;//changing material from green to red
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.tag == "Player Laser")
        {
            health_ -= 5f;
            if(health_ <= 0)//when gen is disabled: color back to red
            {
                //turn on short circuit trigger object 

                ChangeShortCircuitPlane();//effectsa plane within the BossTube

                chosenGenBackToRed();//Deactivates the Green color back to red

                Destroy(this);//destroy this script after tag change and 
            }
        }

    }

    public void ChangeShortCircuitPlane()
    {
        GameObject shortCircuitPlane_ = GameObject.Find("Short Circuit Plane");//Gameobject with material references

        shortCircuitPlane_.tag = "Shorted Out";//sets tag of shield ONLY AFTER the chosen gen has been destroyed
    }
}
