using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundEffects : MonoBehaviour
{
    public float HungerCreepVal = 0.02f;
    public float BoredomCreepVal = 0.02f;
    Attention m_pAttention;
    Hunger m_pHunger;

    void OnEnable()
    {
        m_pAttention = GetComponentInChildren<Attention>();
        m_pHunger = GetComponentInChildren<Hunger>();
    }

    void Update()
    {
        m_pHunger.GetComponent<Slider>().value += HungerCreepVal * Time.deltaTime;
        m_pAttention.GetComponent<Slider>().value += BoredomCreepVal * Time.deltaTime;
    }
}
