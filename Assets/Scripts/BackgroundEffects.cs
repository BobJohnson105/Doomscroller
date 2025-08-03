using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundEffects : MonoBehaviour
{
    public float HungerCreepVal = 0.02f;
    public float BoredomCreepVal = 0.02f;
    public float StressMessageCreepVal = 0.02f;
    public float BoredomIncreaseStartTime = 120.0f;
    public float BoredomIncreaseDoublePeriod = 120.0f;
    Attention m_pAttention;
    Hunger m_pHunger;
    Stress m_pStress;
    MessageAlertNotif m_pMessageAlertNotif;
    public Canvas badend;
    public Canvas goodend;
    public Canvas mainUI;
    public Canvas screentime;
    public Animation cameraAnim;
    public GameObject phone;

    Slider[] m_pAllSliders;
    float m_fLoadTime;
    bool hasPlayed;

    void OnEnable()
    {
        badend.enabled = false;
        goodend.enabled = false;
        hasPlayed = false;
        screentime.enabled = false;

        m_fLoadTime = Time.time;
        m_pAttention = GetComponentInChildren<Attention>();
        m_pHunger = GetComponentInChildren<Hunger>();
        m_pStress = GetComponentInChildren<Stress>();
        m_pMessageAlertNotif = FindObjectOfType<MessageAlertNotif>( true );
        m_pAllSliders = GetComponentsInChildren<Slider>();
    }

    void Update()
    {
        m_pHunger.GetComponent<Slider>().value += HungerCreepVal * Time.deltaTime;

        float fDifficulty = ( Time.time - m_fLoadTime - BoredomIncreaseStartTime ) / BoredomIncreaseDoublePeriod;
        float fRealBoredomCreep = BoredomCreepVal * ( 1 + Mathf.Max( fDifficulty, 0 ) );

        m_pAttention.GetComponent<Slider>().value += fRealBoredomCreep * Time.deltaTime;

        if ( m_pMessageAlertNotif.gameObject.activeSelf )
            m_pStress.GetComponent<Slider>().value += StressMessageCreepVal * Time.deltaTime;

        foreach ( var slider in m_pAllSliders )
        {
            if ( slider.value == slider.maxValue )
            {
                if (!hasPlayed)
                {
                    badend.enabled = true;
                    mainUI.enabled = false;
                    phone.SetActive(false);
                    cameraAnim.Play();
                    hasPlayed = true;
                }
                
            }
        }
        print(Time.time - m_fLoadTime);

        if ( Time.time - m_fLoadTime > 60 * 4 )
        {
            if (!hasPlayed)
            {
                screentime.enabled = true;
                hasPlayed = true;
            }
        }
    }
}
