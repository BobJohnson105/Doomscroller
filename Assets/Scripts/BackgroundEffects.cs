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
    float m_fLoadTime;

    void OnEnable()
    {
        m_fLoadTime = Time.time;
        m_pAttention = GetComponentInChildren<Attention>();
        m_pHunger = GetComponentInChildren<Hunger>();
        m_pStress = GetComponentInChildren<Stress>();
        m_pMessageAlertNotif = FindObjectOfType<MessageAlertNotif>( true );
    }

    void Update()
    {
        m_pHunger.GetComponent<Slider>().value += HungerCreepVal * Time.deltaTime;

        float fDifficulty = ( Time.time - m_fLoadTime - BoredomIncreaseStartTime ) / BoredomIncreaseDoublePeriod;
        float fRealBoredomCreep = BoredomCreepVal * ( 1 + Mathf.Max( fDifficulty, 0 ) );

        m_pAttention.GetComponent<Slider>().value += fRealBoredomCreep * Time.deltaTime;

        if ( m_pMessageAlertNotif.gameObject.activeSelf )
            m_pStress.GetComponent<Slider>().value += StressMessageCreepVal * Time.deltaTime;

    }
}
