using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class Player_SpeedControl : MonoBehaviour
{

    [Header("No Change: Tracking Values")]
    [SerializeField] public float currentBoostLevel_;
    [SerializeField] public static float regulatedSpeed_;//used to determine turn speed in other scripts

    [Header("UI Variables")]
    public Slider m_slider;
    public Image m_fillImage;
    public Color maxSpeedColor_;
    public Color minSpeedColor_;

    [Header("Ship Speed Settings")]
    public float baseSpeed_;
    public float speedAdjustValue_;

    [Header("Ship Speed Min and Max")]
    [SerializeField] float minSpeed_;//will be the slider health
    [SerializeField] float maxSpeed_;//will be the slider health

    [Header("Testing")]
    public bool testingFullPower_;
    public bool testingHalfPower_;

    public float currentSpeed_;
    float availableBoostSpeed_;

    bool boosting_;
    bool slowingDown_;



    // Start is called before the first frame update
    void Start()
    {
        SetBoostPower();

        currentSpeed_ = baseSpeed_;

        m_slider.maxValue = maxSpeed_;
        m_slider.minValue = minSpeed_;

    }


    public void UpdateSlider()//called while boosting and regenerating
    {

        m_slider.value = currentSpeed_;

        m_fillImage.color = Color.Lerp(maxSpeedColor_, maxSpeedColor_, maxSpeed_ / currentSpeed_);


    }


    private void SetBoostPower()
    {
        if (testingFullPower_ == true)
            availableBoostSpeed_ = baseSpeed_ * 4;

        else if (testingHalfPower_ == true)
            availableBoostSpeed_ = baseSpeed_ * 2;

        else
            availableBoostSpeed_ = GameManager_BoostersObstacleActivation.completionFactor * baseSpeed_;
    }



    private void ApplySpeed()
    {
        UpdateSlider();


        regulatedSpeed_ = Mathf.Clamp(currentSpeed_, minSpeed_, maxSpeed_);
        //print(regulatedSpeed_);

        transform.Translate(0f, 0f, regulatedSpeed_, Space.Self);
    }

    private void BoolControl()
    {

        if (CrossPlatformInputManager.GetButtonDown("Boost"))
        {
            boosting_ = true;
        }
        else if (CrossPlatformInputManager.GetButtonUp("Boost"))
        {
            boosting_ = false;
        }
        if (CrossPlatformInputManager.GetButtonDown("Slow"))
            slowingDown_ = true;
        else if (CrossPlatformInputManager.GetButtonUp("Slow"))
            slowingDown_ = false;


    }

    private void SpeedControl()
    {
        if (boosting_ && currentSpeed_ <= maxSpeed_)
        {
            currentSpeed_ += (CrossPlatformInputManager.GetAxis("Boost") * availableBoostSpeed_);
            //UpdateSlider();

        }
        else if (slowingDown_ && currentSpeed_ >= minSpeed_)
        {
            currentSpeed_ -= (CrossPlatformInputManager.GetAxis("Slow") * availableBoostSpeed_);
            //UpdateSlider();
        }
    }

    // Update is called once per frame
    void Update()
    {
        ApplySpeed();
        BoolControl();
        SpeedControl();
    }
}
