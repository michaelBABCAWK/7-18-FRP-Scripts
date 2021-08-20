using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Mixer_RepairSection : MonoBehaviour
{
    [Header("Mixer Snapshots")]


    public AudioMixerSnapshot menuSound_;
    public AudioMixerSnapshot pausedSound_;
    public AudioMixerSnapshot playingNoseSound_;
    public AudioMixerSnapshot playingOverallCamSound__;
    public AudioMixerSnapshot terminalSound__;
    public AudioMixerSnapshot deathSound_;


    [SerializeField] float transitionTime_;

    private void Start()
    {
        MainMenuSnapshot();
    }


    public void PauseGameSnapshot()
    {
        pausedSound_.TransitionTo(transitionTime_);
    }

    public void MainMenuSnapshot()
    {
        menuSound_.TransitionTo(transitionTime_);
    }

    public void NoseCamSnapshot()
    {
        playingNoseSound_.TransitionTo(transitionTime_);
    }

    public void OverAllCamSnapshot()
    {
        playingOverallCamSound__.TransitionTo(transitionTime_);
    }

    public void TerminalSnapshot()
    {
        terminalSound__.TransitionTo(transitionTime_);
    }

    public void DeathSnapshot()
    {
        deathSound_.TransitionTo(transitionTime_);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
