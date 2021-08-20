using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;


public class Player_BoostingSystem : MonoBehaviour
{
    [Header("No Change: Tracking Values")]
    [SerializeField] float currentBoostLevel_;
    [SerializeField] public float regulatedSpeed_;

    [Header("UI Variables")]
    public Slider m_slider;
    public Image m_fillImage;
    public Color m_fullBoost;
    public Color m_emptyBoostLevel;

    [Header("Ship Speed")]
    public float baseSpeed_;
    public float reduceSpeed_;
    public float maxSpeed_;


    [Header("Boosting")]
    [SerializeField] float startingBoostLevel_;//will be the slider health
    private float refillBoostToThisLevel_;//ref for shield recharge to  go to. Slider total health ref as well.
    [SerializeField] private float timePassToRegen_;
    private float timeToRegenRef_;//ref for shield recharge to reset to
    [SerializeField] float reducingBoostPerFrame_;
    [SerializeField] int increaseBoostPerFrame_;


    [Header("Testing")]
    public bool testingFullPower_;
    public bool testingHalfPower_;


    GameObject playerRef_;
    Player_ControlShip unPausePlayer_;

    float totalBoost_;
    float availableBoostSpeed_;


    bool regenActive_;
    bool boostDepleted_ = false;
    bool boosting_ = false;
    bool slowingDown_ = false;


    private void Awake()//Set Stats in awake to be set in Start
    {

        boostDepleted_ = false;

        m_slider.maxValue = startingBoostLevel_;

        currentBoostLevel_ = startingBoostLevel_;

        refillBoostToThisLevel_ = startingBoostLevel_;//ref for shield health to revert back to
    }


    private void Start()
    {

        playerRef_ = GameObject.Find("Player");
        unPausePlayer_ = playerRef_.GetComponent<Player_ControlShip>();


        UpdateSlider();


        SetBoostPower();
    }


    private void SetBoostPower()
    {
        if(testingFullPower_ == true)
            availableBoostSpeed_ = baseSpeed_ * 4;
        
        else if(testingHalfPower_ == true)
            availableBoostSpeed_ = baseSpeed_ * 2;

        else
        availableBoostSpeed_ = GameManager_BoostersObstacleActivation.completionFactor * baseSpeed_;
    }


    public void UpdateSlider()//called while boosting and regenerating
    {

        m_slider.value = currentBoostLevel_;

        m_fillImage.color = Color.Lerp(m_emptyBoostLevel, m_fullBoost, currentBoostLevel_ / refillBoostToThisLevel_);

        if (currentBoostLevel_ <= 0)//only when boost is zero or below
        {
            boostDepleted_ = true;
        }

    }


    private void DetectBools()
    {
        BoostingBools();
        SlowingBools();

    }

    private void SlowingBools()
    {
        if (CrossPlatformInputManager.GetAxis("Slow") != 0)
        {
            slowingDown_ = true;
        }
        else
        {
            slowingDown_ = false;
        }
    }

    private void BoostingBools()
    {
        if (CrossPlatformInputManager.GetAxis("Boost") != 0 && !boostDepleted_)//Has to be 1 so the boost overheat does not rise
        {
            boosting_ = true;//This bool will control if the shield can regen. Needs to revert to false.
        }
        else if (CrossPlatformInputManager.GetAxis("Boost") == 1 && !boostDepleted_)//Has to be 1 so the boost overheat does not rise until fully boosting
        {
            timePassToRegen_ = timeToRegenRef_;//reset timetopass before refill while boosting

            currentBoostLevel_ -= reducingBoostPerFrame_;

            UpdateSlider();
        }
        else if (CrossPlatformInputManager.GetAxis("Boost") == 0)
        {
            boosting_ = false;

            BoosterCooldown();

        }
    }

    private void BoosterCooldown()
    {
        if (startingBoostLevel_ < refillBoostToThisLevel_ && !regenActive_)//if below the totalboost, time will count down.
        {
            timePassToRegen_ -= .1f; // drops .1 per frame

            boostDepleted_ = false;//can boost again
        }


        if (timePassToRegen_ <= 0)//if enough time has passed then start regen
        {
            regenActive_ = true;
        }

        if (currentBoostLevel_ >= refillBoostToThisLevel_)//once currentboost is full from regenerate
        {
            regenActive_ = false;
        }

        if (regenActive_)//if enough time has passed to regenerate boost
        {
            currentBoostLevel_ += increaseBoostPerFrame_;


            if (startingBoostLevel_ >= refillBoostToThisLevel_ / 4)//refill to 25% before being able to boost again
            {
                boostDepleted_ = false;//stop regen

                timePassToRegen_ = timeToRegenRef_;//reset timetopass before refill

            }


            UpdateSlider();

        }
    }


    private void SpeedControl()
    {
        Boosting();

        SlowingDown();

        transform.Translate(0f, 0f, regulatedSpeed_, Space.Self);
        
    }

    private void Boosting()
    {
        if (boosting_)
        {
            if (!boostDepleted_)//stop detecting button press
            {
                totalBoost_ = CrossPlatformInputManager.GetAxis("Boost") * availableBoostSpeed_;

                regulatedSpeed_ = Mathf.Clamp(totalBoost_, baseSpeed_, maxSpeed_);
            }
        }
    }


    private void SlowingDown()
    {
        if (slowingDown_)
        {
            regulatedSpeed_ = baseSpeed_ - (CrossPlatformInputManager.GetAxis("Slow") * reduceSpeed_);
        }
    }

 
    private void Update()
    {


        if (unPausePlayer_.paused == false)
        {

            DetectBools();



            SpeedControl();
        }

    }

}
