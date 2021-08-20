using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTube_GenSelect : MonoBehaviour
{
    //Do not choose the one last used

    GameObject genObject_;



    Transform chosenGen_;//Select random child//add scrpt//remove script



    Renderer rendererOnChosenGen_;//change chosen child color while it has the script



    public Material[] newColorOnChosenGen_;//material assigned to chosen gen


    [HideInInspector]
    public bool hasAChosen_;//controls if new gen is chosen



    public float checkForGenTime_;//coroutine timer
    private void Awake()
    {
        hasAChosen_ = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        SelectRandomGen();


        //StartCoroutine("selectingRandomGen", checkForGenTime_);//not an enumerator. Needs to be called
        //after a specific time. Only after the shield is back up.


        //start enumerator that checks for the bool
    }

    public void SelectRandomGen()
    {
        chosenGen_ = gameObject.transform.GetChild(Random.Range(0, gameObject.transform.childCount));
        genObject_ = chosenGen_.gameObject;



        genObject_.AddComponent<BossTube_SelectedGen>();



        //rendererOnChosenGen_ = genObject_.GetComponent<Renderer>();
        //rendererOnChosenGen_.material = newColorOnChosenGen_[1];

    }

    public IEnumerator selectingRandomGen()
    {

        if (!hasAChosen_)
        {
            print("false");
            SelectRandomGen();
        }
        else
        {
            print("True");
        }

        yield return checkForGenTime_;
    }

}
