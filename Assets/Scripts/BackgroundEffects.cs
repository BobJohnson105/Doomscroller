using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundEffects : MonoBehaviour
{
    public float HungerCreepVal = 0.02f;
    public float BoredomCreepVal = 0.02f;
    public float BoredomIncreaseStartTime = 120.0f;
    public float BoredomIncreaseDoublePeriod = 120.0f;
    Attention m_pAttention;
    Hunger m_pHunger;
    float m_fLoadTime;

    void OnEnable()
    {
        m_fLoadTime = Time.time;
        m_pAttention = GetComponentInChildren<Attention>();
        m_pHunger = GetComponentInChildren<Hunger>();
    }

    void Update()
    {
        m_pHunger.GetComponent<Slider>().value += HungerCreepVal * Time.deltaTime;

        float fDifficulty = ( Time.time - m_fLoadTime - BoredomIncreaseStartTime ) / BoredomIncreaseDoublePeriod;
        float fRealBoredomCreep = BoredomCreepVal * ( 1 + Mathf.Max( fDifficulty, 0 ) );

        m_pAttention.GetComponent<Slider>().value += fRealBoredomCreep * Time.deltaTime;
    }
}
