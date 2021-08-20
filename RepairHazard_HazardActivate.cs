using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairHazard_HazardActivate : MonoBehaviour
{
    public GameObject myHazard_;
    public GameObject myLight_;
    public GameObject myManager_;


    public float TelegraphLength_;
    public float hazardLength_;



    public bool lastHazard_ = false;
    // Start is called before the first frame update


    public void HasBeenChosen()
    {
        lastHazard_ = true;

        myLight_.SetActive(true);

        Invoke("FireHazardOn", TelegraphLength_);
    }

    private void FireHazardOn()
    {
        ParticleControl(true);

        AudioSource _hazardNoiseSource = gameObject.GetComponentInChildren<AudioSource>();
        _hazardNoiseSource.Play();

        Invoke("FireHazardOff", hazardLength_);
    }



    public void FireHazardOff()
    {
        ParticleControl(false);

        myLight_.SetActive(false);

        SelectingNewHazard();
    }


    private void ParticleControl(bool activateCharger_)
    {
        ParticleSystem thisHazard_ = myHazard_.GetComponent<ParticleSystem>();

        var hazardParticles_ = thisHazard_.emission;

        hazardParticles_.enabled = activateCharger_;
    }


    private void SelectingNewHazard()
    {
        GameManager_RepairHazards callNewChoice_ = myManager_.GetComponent<GameManager_RepairHazards>();

        callNewChoice_.InvokeHazard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
