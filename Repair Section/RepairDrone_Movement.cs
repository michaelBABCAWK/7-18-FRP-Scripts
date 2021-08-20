using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RepairDrone_Movement : MonoBehaviour
{

    GameObject repairDrone_;
    GameObject finish;
    GameObject livesTracking;


    [Header("Speed Variables")]
    [SerializeField] float mainThrust = 10f;
    [SerializeField] float rcsThrust = 10f;


    GameManager_LivesTracker dronesLeft;
    GameManager_tutorialManager tutorialCompletion;
    RepairDrone_DeathControl deathScriptRef_;


    [HideInInspector]
    public int tutorialFinishes;


    Rigidbody Rigidbody;


    AudioSource EngineThrust;



    bool deathRef_;
    [HideInInspector]
    public static bool isNoseCam;
    [HideInInspector]
    public static bool TutorialFinishStatus;

    string sceneLoaded;

    // Start is called before the first frame update
    void Start()
    {
        SetGlobals();

        ShipComponents();

        SetFinish();

        SetEventVariables();

        deathScriptRef_ = gameObject.GetComponent<RepairDrone_DeathControl>();

    }

    private void SetGlobals()
    {
        sceneLoaded = GameManager_Terminal_Buttons.levelLoaded;
    }

    private void ShipComponents()
    {
        livesTracking = GameObject.Find("Game Manager");

        Rigidbody = GetComponent<Rigidbody>();

        EngineThrust = GetComponent<AudioSource>();
    }

    private void SetFinish()
    {
        if (GameManager_Terminal_Buttons.levelLoaded == "Cannons")
        {
            finish = GameObject.Find("Finish Cannons");
        }
        if (GameManager_Terminal_Buttons.levelLoaded == "Boosters")
        {
            finish = GameObject.Find("Finish Boosters");
        }
        if (GameManager_Terminal_Buttons.levelLoaded == "Shields")
        {
            finish = GameObject.Find("Finish Shields");
        }
    }

    private void FinishRoutine()
    {
        ReduceDroneCount();

        DontDestroyOnLoad(livesTracking);
        DontDestroyOnLoad(finish);

        //Change to load variable set by RepairMenu Script.
        SceneManager.LoadScene(sceneLoaded, LoadSceneMode.Single);
    }


    private void OnCollisionEnter(Collision collision)
    {
        repairDrone_ = GameObject.Find("Ship");

        SetFinish();

        switch (collision.gameObject.tag)
        {
            case "Finish":
                FinishRoutine();
                break;
        }
    }

    private static void ReduceDroneCount()
    {
        GameObject theManager_ = GameObject.Find("Game Manager");
        GameManager_LivesTracker managerLivesTracker_ = theManager_.GetComponent<GameManager_LivesTracker>();

        managerLivesTracker_.ReduceDrones();
    }


    private void SetEventVariables()
    {
        int _currScene = SceneManager.GetActiveScene().buildIndex;

        if(_currScene < 7)
        {
            GameObject primerObject_ = GameObject.Find("Primer");
            Objective_Primer primerEvent_ = primerObject_.GetComponent<Objective_Primer>();

            primerEvent_.OnPrimerIgnited_ += PrimerOn;
            primerEvent_.OnPrimerOff_ += PrimerOff;
        }
    }


 
    public void PrimerOn(object sender, EventArgs e)
    {
        gameObject.tag = "Player";
    }

    

    public void PrimerOff(object sender, EventArgs e)
    {
        gameObject.tag = "Untagged Player";
    }


    private void ApplyThrust()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Rigidbody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
            if (!EngineThrust.isPlaying)
            {
                EngineThrust.Play();
            }
        }
        else if (!Input.GetKey(KeyCode.Space))
        {
            EngineThrust.Stop();
        }
    }

    void ProcessRotation()
    {
        Vector3 ZFacing;

        Rigidbody.freezeRotation = true;

        float deltaTimeRotate = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            ZFacing = Vector3.forward * rcsThrust * deltaTimeRotate;
            transform.Rotate(ZFacing);
        }
        if (Input.GetKey(KeyCode.D))
        {
            ZFacing = Vector3.back * rcsThrust * deltaTimeRotate;
            transform.Rotate(ZFacing);
        }

        var rot = transform.rotation;

        rot.x = 0;
    }



    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        deathRef_ = deathScriptRef_.notDead_;

        if (deathRef_)
        {
            ApplyThrust();
            ProcessRotation();
        }
        else
        {
            return;
        }

    }


}
