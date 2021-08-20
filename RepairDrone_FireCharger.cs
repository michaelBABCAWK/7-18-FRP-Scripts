using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class RepairDrone_FireCharger : MonoBehaviour
{
    [Header("Sound Components")]
    //public GameObject soundSource_;
    AudioSource soundSourceAudio_;

    [Header("Charger Particle Object")]
    GameObject charger_;

    private void Awake()
    {
        charger_ = GameObject.Find("Charger");
        soundSourceAudio_ = GameObject.Find("Audio object: Charger").GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    private void FireChargeLaser()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            FireCharger(true);
            if (!soundSourceAudio_.isPlaying)
            {
                soundSourceAudio_.Play();
            }
        }
        else
        {
            FireCharger(false);
            if (soundSourceAudio_.isPlaying)
            {
                soundSourceAudio_.Stop();
            }

        }
    }

    private void FireCharger(bool activateCharger)
    {
        ParticleSystem _thisGun = charger_.GetComponent<ParticleSystem>();
        var _chargerBeam = _thisGun.emission;
        _chargerBeam.enabled = activateCharger;

    }

    // Update is called once per frame
    void Update()
    {
        FireChargeLaser();

    }
}
