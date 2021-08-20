using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Mixer_FlightSection : MonoBehaviour
{
    [Header("Mixer Snapshots")]


    public AudioMixerSnapshot flightTutorial_;


    public AudioMixerSnapshot flightSection_;

    [SerializeField] float transitionTime_;

    private void Awake()
    {
        Snapshot_Tutorial();
    }

    public void Snapshot_Tutorial()
    {
        flightTutorial_.TransitionTo(transitionTime_);
    }

    public void Snapshot_FlightSection()
    {
        flightSection_.TransitionTo(transitionTime_);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
