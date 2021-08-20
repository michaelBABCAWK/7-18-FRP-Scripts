using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Boss_turretDeactivate : MonoBehaviour
{
    public Material[] status;//

    Renderer genColor;//

    public float m_genHealth = 25f;//

    Boss_TurretSystem deactivateTurret;//

    public GameObject connectedTurret;//


    public Slider m_slider;
    public Image m_fillimage;
    public Color m_FullHealthColor = Color.green;
    public Color m_ZeroHealthColor = Color.red;
    // public GameObject m_ExplosionPregab;


    private float m_currentHealth;
    private bool m_dead;

    Boss_ManageDeath sendDeathEnable;

    void Start()
    {
        SetHealthUI();

        sendDeathEnable = GetComponentInParent<Boss_ManageDeath>();

        deactivateTurret = connectedTurret.GetComponent<Boss_TurretSystem>();
        
        genColor = GetComponent<Renderer>();
        
        genColor.material = status[0];
    }

    private void OnEnable()
    {
        m_currentHealth = m_genHealth;
        m_dead = false;

        SetHealthUI();
    }

    public void TakeDamage(float Amount)
    {
        m_currentHealth -= Amount;

        SetHealthUI();
    }

    public void SetHealthUI()
    {
        m_slider.value = m_currentHealth;

        m_fillimage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_currentHealth / m_genHealth);
    }

    // Start is called before the first frame update

    private void OnParticleCollision(GameObject sourceOfLaser)
    {
        if (sourceOfLaser.gameObject.tag == "Player Laser")
        {
            TakeDamage(1);
        }

        if (sourceOfLaser.gameObject.tag == "Player Laser")
        {
            m_genHealth -= 1f;

            if (m_genHealth <= 0)
            {
                deactivateTurret.turretOff();

                genColor.material = status[1];
            }
        }
    } 
}

