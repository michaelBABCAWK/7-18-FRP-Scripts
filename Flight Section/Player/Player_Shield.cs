using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Player_Shield : MonoBehaviour
{
    [Header("UI Variables")]
    public Slider m_slider;
    public Image m_fillImage;
    public Color m_FullHealthColor;
    public Color m_ZeroHealthColor;


    [Header("Testing")]
    public bool testingFullPower_;
    public bool testingHalfPower_;


    [Header("Shield Strengths")]
    public float baseShieldStrength_;
    public static float ShieldStrength_;//will be the slider health
    private float totalHealth_;//ref for shield recharge to  go to. Slider total health ref as well.


    [Header("Regen Properties")]
    [SerializeField] private float timePassToRegen_;
    private float timeToRegenRef_;//ref for shield recharge to reset to
    [SerializeField] private int regenPerFrame_;


    bool beingHit_ = false;


    private void Awake()//Set Stats in awake to be set in Start
    {

        if(testingFullPower_ == true)
            ShieldStrength_ = baseShieldStrength_ * 4;
        else if(testingHalfPower_ == true)
            ShieldStrength_ = baseShieldStrength_ * 2;
        else
            ShieldStrength_ = baseShieldStrength_ * GameManager_CannonObstacleActivation.completionFactor;

    }

    private void Start()
    {
        StartCoroutine("setNotBeingHit", .01f);//keeping beinghit bool false

        SetShieldValues();

        SetShieldStatusUI();

        m_slider.maxValue = ShieldStrength_;//setting the max value

    }

    private void SetShieldValues()
    {
        timeToRegenRef_ = timePassToRegen_;//ref for timetopass before shield recharge starts

        totalHealth_ = ShieldStrength_;//ref for shield health to revert back to

        m_slider.maxValue = ShieldStrength_;

        m_slider.value = ShieldStrength_;
    }

    public void SetShieldStatusUI()
    {
        m_slider.value = ShieldStrength_;

        m_fillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, ShieldStrength_ / totalHealth_);
    }

    IEnumerator setNotBeingHit(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            beingHit_ = false;
        }
    }

    private void Update()
    {
        ShieldRegenMethod();
    }

    private void ShieldRegenMethod()
    {
        if (ShieldStrength_ < totalHealth_ && !beingHit_)//if below the totalhealth, and not been hit, time will count down.
        {
            timePassToRegen_ -= 1 * Time.deltaTime; // drops 1 per frame

            if (timePassToRegen_ <= 0 && ShieldStrength_ < totalHealth_)
            {
                ShieldStrength_ += 1 * Time.deltaTime;
                SetShieldStatusUI();
            }
        }
        else if(beingHit_)
        {
            timePassToRegen_ = timeToRegenRef_;
        }
    }

    public void DamageCalls(float damageDone_)
    {
        ShieldStrength_ -= damageDone_;

        SetShieldStatusUI();

        beingHit_ = true;//This bool will control if the shield can regen. Needs to revert to false.

        if (ShieldStrength_ <= 0)
        {
            gameObject.GetComponent<PlayerFlight_DeathHandler>().OnDeathEvents();
        }
    }


    private void OnParticleCollision(GameObject laser)
    {
        if (laser.gameObject.tag == "Enemy Laser")
        {
            ShieldStrength_ -= 5;

            SetShieldStatusUI();


            beingHit_ = true;//This bool will control if the shield can regen. Needs to revert to false.

            if (ShieldStrength_ <= 0)
            {
                gameObject.GetComponent<PlayerFlight_DeathHandler>().OnDeathEvents();
            }

        }
        else
        {
            beingHit_ = false;
        }

    }

}
