using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Boss_trackHealth : MonoBehaviour
{
    [Header("Health and Health UI")]
    public float m_startingHealth = 100f;
    public Color m_FullHealthColor = Color.green;
    public Color m_ZeroHealthColor = Color.red;


    [Header("UI Variables")]
    public GameObject objectWithMaxValue_;
    public Slider m_slider;
    public Image m_fillimage;


    Slider maxValueSlider_;
    private float m_currentHealth;
    private bool m_dead;

    Boss_ManageDeath sendDeathEnable;

    void Start()
    {
        SetHealthUI();

        sendDeathEnable = GetComponentInParent<Boss_ManageDeath>();
    }

    private void OnEnable()
    {
        m_currentHealth = m_startingHealth;
        m_dead = false;

        SetHealthUI();
    }


    private void OnParticleCollision(GameObject sourceOfLaser)
    {
        if (sourceOfLaser.gameObject.tag == "Player Laser")
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(float Amount)
    {
        m_currentHealth -= Amount;

        SetHealthUI();

        if (m_currentHealth <= 0f && !m_dead)
        {
            OnDeath();
        }
    }

    public void SetHealthUI()
    {
        m_slider.value = m_currentHealth;

        m_fillimage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_currentHealth / m_startingHealth);

        maxValueSlider_ = objectWithMaxValue_.GetComponent<Slider>();

        maxValueSlider_.maxValue = m_startingHealth;
    }

    public void OnDeath()
    {
        m_dead = true;

        sendDeathEnable.beenKilled();

        SceneManager.LoadScene("Win Screen");
    }
}
