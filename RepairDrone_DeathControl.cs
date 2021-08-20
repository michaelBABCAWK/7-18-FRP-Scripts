using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RepairDrone_DeathControl : MonoBehaviour
{
    Mixer_RepairSection changeAudio_;


    GameObject repairDrone_;
    GameObject finish_;
    GameObject livesTracking_;

    [Header("Death Art")]
    [SerializeField] GameObject turnOffModels_;
    [SerializeField] GameObject deathSparks_;


    [Header("Death Audio Determined By Current Camera")]
    public AudioSource deathOverallSource_;
    public AudioSource deathNoseSource_;
    //public AudioClip deathExplosion_;
    //public AudioClip deathStatic_;

    [Header("Death Values")]
    public float deathReloadDelay_;
    public float deathSoundVolume_;


    string sceneLoaded_;
    [Header("Death Messages")]
    public GameObject deathCanvas_;
    public Text noseMessage_;
    public Text overalMessage_;

    [HideInInspector] public bool notDead_;

    private void Awake()
    {
        ResetVars();

    }



    // Start is called before the first frame update
    void Start()
    {
        SetScene();

        SetFinish();

        AuidoVariables();

        AssignLivesTracker();

    }

    private void ResetVars()
    {
        notDead_ = true;
    }


    private void AssignLivesTracker()
    {
        livesTracking_ = GameObject.Find("Game Manager");
    }

    private void AuidoVariables()
    {
        changeAudio_ = GetComponent<Mixer_RepairSection>();

        //overallViewDeathSource_ = gameObject.GetComponent<AudioSource>();
    }
    

    void SetScene()
    {
        sceneLoaded_ = GameManager_Terminal_Buttons.levelLoaded;
    }

    void DeathSoundMix()
    {
        GameObject managerObject_ = GameObject.Find("Game Manager");
        Mixer_RepairSection startTerminalMix_;
        startTerminalMix_ = managerObject_.GetComponentInChildren<Mixer_RepairSection>();
        startTerminalMix_.DeathSnapshot();
    }

    void SetFinish()
    {
        if (GameManager_Terminal_Buttons.levelLoaded == "Cannons")
        {
            finish_ = GameObject.Find("Finish Cannons");
        }
        if (GameManager_Terminal_Buttons.levelLoaded == "Boosters")
        {
            finish_ = GameObject.Find("Finish Boosters");
        }
        if (GameManager_Terminal_Buttons.levelLoaded == "Shields")
        {
            finish_ = GameObject.Find("Finish Shields");
        }
    }

    void ReduceDroneCount()
    {
        GameObject theManager_ = GameObject.Find("Game Manager");
        GameManager_LivesTracker managerLivesTracker_ = theManager_.GetComponent<GameManager_LivesTracker>();

        managerLivesTracker_.ReduceDrones();
    }

    private void DeathSoundDependingOnCamera()
    {
        RepairDrone_CameraControl _whichCam;
        bool _camObjectOverall;

        _whichCam = gameObject.GetComponent<RepairDrone_CameraControl>();
        _camObjectOverall = _whichCam.camObjectOverall_;

        changeAudio_ = GameObject.Find("Game Manager").GetComponentInChildren<Mixer_RepairSection>();

        DeathSoundMix();

        if (_camObjectOverall)
        {

            deathOverallSource_.PlayOneShot(deathOverallSource_.clip, deathSoundVolume_);
            changeAudio_.DeathSnapshot();

            // print("Overall sound death");
            //overallViewDeathSource_.Play();
            //changeAudio_.DeathSnapshot();
        }
        else if (!_camObjectOverall)
        {
            deathNoseSource_.PlayOneShot(deathNoseSource_.clip, deathSoundVolume_);
            changeAudio_.DeathSnapshot();
            // print("Nose sound death");
            // noseDeathSource_.Play();
            //changeAudio_.DeathSnapshot();


            deathCanvas_.SetActive(true);
        }
    }

    private void DestroyRoutine()
    {
        DeathSoundDependingOnCamera();//tells camera to trigger which 

        ReduceDroneCount();

        notDead_ = false;

        DeathArt();

        Invoke("LoadAfterDeathSound", deathReloadDelay_);
    }

    private void LoadAfterDeathSound()
    {
        Destroy(repairDrone_, 0);
        DontDestroyOnLoad(livesTracking_);
        DontDestroyOnLoad(finish_);

        SceneManager.LoadScene(sceneLoaded_, LoadSceneMode.Single);


    }

    private void DeathArt()
    {
        ParticleSystem _sparkSource = deathSparks_.GetComponent<ParticleSystem>();
        var _sparkEmit = _sparkSource.emission;

        _sparkSource.Play();
        _sparkEmit.enabled = true;

        turnOffModels_.SetActive(false);

        gameObject.transform.DetachChildren();

    }

    void OnParticleCollision(GameObject tag)
    {
        repairDrone_ = GameObject.Find("Ship");

        if (notDead_)
        {
            if (tag.gameObject.tag == "Deadly")
            {
                DestroyRoutine();

            }
        }
        else if (!notDead_)
        {

        }


    }

    void OnCollisionEnter(Collision collision)
    {
        repairDrone_ = GameObject.Find("Ship");

        if (notDead_)
        {
            switch (collision.gameObject.tag)
            {
                case "Deadly":
                    DestroyRoutine();
                    break;
                default:
                    break;
            }
        }
        else if (!notDead_)
        {

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
