using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

public class Player_Weapons : MonoBehaviour
{
    enum Enum_heatTracking { okayTofire, coolingDown}
    Enum_heatTracking heatTrackingStage_;

    [Header("DO NOT CHANGE--Expected fireRate")]
    [SerializeField] float overHeatLimit_;
    [SerializeField] float currentHeat_ = 0;

    [Header("UI Variables")]
    public Slider m_slider;
    public Image m_fillImage;
    public Color m_OverHeated;
    public Color m_NoHeat;

    [Header("Overheat Settings ")]
    [SerializeField] float consecutiveShotsBeforeOverheating_;//set by cannons completion
    [SerializeField] float heatPerShot_;//used to calculate overheat limit
    [SerializeField] float timeBeforeWeaponCooling_;
    float timeBeforeWeaponCoolingRef_;
    [SerializeField] float rateTimeGoesDownOnOverHeat_ = .1f;

    [Header("System: Weapons")]
    public int extraStartingWeapons_;
    public float delayBetweenShots_;
    [Range(0,1)] 
    public float delaySpeed_;
    float delayRef_;

    [Header("Weapon Locations")]
    [SerializeField] GameObject LeftFrontWeapon;
    [SerializeField] GameObject RightFrontWeapon;

    [SerializeField] GameObject LeftWingWeapon;
    [SerializeField] GameObject RightWingWeapon;

    [SerializeField] Transform LFrontPosition;
    [SerializeField] Transform RFrontPosition;

    [SerializeField] Transform LWingPosition;
    [SerializeField] Transform RWingPosition;




    bool firing_ = false;
    bool overHeated_ = false;//stop from firing until currentheat = 0

    AudioSource weaponsSound_;



    private void Awake()
    {
        SetVariables();

        SetComponents();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void SetVariables()
    {
        overHeatLimit_ = consecutiveShotsBeforeOverheating_;//setting limit of overheat
        m_slider.maxValue = consecutiveShotsBeforeOverheating_;//set value according to lmit of overheat

        delayRef_ = delayBetweenShots_;//ref for delay between shots
        timeBeforeWeaponCoolingRef_ = timeBeforeWeaponCooling_;//ref for time on cooling

        extraStartingWeapons_ = GameManager_CannonObstacleActivation.completionFactor + extraStartingWeapons_;
        heatTrackingStage_ = Enum_heatTracking.okayTofire;
    }

    private void SetComponents()
    {
        weaponsSound_ = gameObject.GetComponent<AudioSource>();
    }

    void ActivatingWeapons(bool isActive)
    {
        LeftFrontWeapon.transform.position = LFrontPosition.transform.position;
        RightFrontWeapon.transform.position = RFrontPosition.transform.position;
        LeftWingWeapon.transform.position = LWingPosition.transform.position;
        RightWingWeapon.transform.position = RWingPosition.transform.position;


        if (extraStartingWeapons_ >= 1)
        {
            var lFront = LeftFrontWeapon.GetComponent<ParticleSystem>().emission;
            lFront.enabled = isActive;
        }

        if (extraStartingWeapons_ >= 2)
        {
            var rFront = RightFrontWeapon.GetComponent<ParticleSystem>().emission;
            rFront.enabled = isActive;
        }

        if (extraStartingWeapons_ >= 3)
        {
            var lWing = LeftWingWeapon.GetComponent<ParticleSystem>().emission;
            lWing.enabled = isActive;
        }

        if (extraStartingWeapons_ >= 4)
        {
            var rWing = RightWingWeapon.GetComponent<ParticleSystem>().emission;
            rWing.enabled = isActive;
        }
    }


    private void ProcessFiring()
    {
        if (CrossPlatformInputManager.GetButton("Fire"))//overheated needs to be false now//|| Input.GetMouseButtonDown(1)
        {
            Firing();

            firing_ = false;

        }
        else
        {
            NotFiring();

            firing_ = false;

        }
    }

    private void Firing()
    {
        if (!overHeated_)
        {
            timeBeforeWeaponCooling_ = timeBeforeWeaponCoolingRef_;//resset countdown before coolingdown

            if (currentHeat_ <= overHeatLimit_)//below overheat level
            {
                DelayShotControl();
            }

            else if (currentHeat_ >= overHeatLimit_)//above heat, stop firing and start cooling weapons
            {
                overHeated_ = true;

                delayBetweenShots_ = delayRef_;//reset delay timer
            }
        }
        else if (overHeated_)
        {
            NotFiring();
        }
    }

    private void DelayShotControl()
    {

        delayBetweenShots_ -= delaySpeed_;


        if (delayBetweenShots_ <= 0 && !overHeated_)
        {
            delayBetweenShots_ = delayRef_;

            weaponsSound_.PlayOneShot(weaponsSound_.clip);

            currentHeat_ += heatPerShot_;

            ActivatingWeapons(true);

            SetWeaponHeatStatus();

        }
    }
    
    private void NotFiring()
    {
        if (!firing_)
        {
            NotOverHeatedControl();

            OverheatedControl();
        }


        ActivatingWeapons(false);
    }

    private void NotOverHeatedControl()
    {
        if (!overHeated_ && currentHeat_ > 0)//reducing without delay if not overheated
        {
            delayBetweenShots_ = delayRef_;//reset delay timer

            currentHeat_ -= heatPerShot_;

            SetWeaponHeatStatus();
        }
    }

    private void OverheatedControl()
    {
        if (overHeated_ && currentHeat_ > 0)//when time before cooling is up then the heat will go down, after being overheated// && timeBeforeWeaponCooling_ > 0
        {

            currentHeat_ -= heatPerShot_;

            SetWeaponHeatStatus();
        }
        else if (overHeated_ && currentHeat_ <= 0)
        {
            delayBetweenShots_ = delayRef_;//reset 344delay timer

            overHeated_ = false;//makes it so the player can fire again
        }
    }

    public void SetWeaponHeatStatus()
    {
        m_slider.value = currentHeat_;

        m_fillImage.color = Color.Lerp(m_NoHeat, m_OverHeated, currentHeat_ / overHeatLimit_);
    }

    // Update is called once per frame
    void Update()
    {
        ProcessFiring();
    }
}
