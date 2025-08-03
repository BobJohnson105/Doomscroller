using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class Globals
{
    public static bool InputEnabled = true;
}


public class MouseMonitor : MonoBehaviour
{
    public float StaminaRegainSpeed = 0.1f;
    public float ClickCost = 0.2f;
    Stamina m_pStamina;
    Blocker m_pBlocker;

    void OnEnable()
    {
        m_pStamina = GetComponentInChildren<Stamina>( true );
        m_pBlocker = GetComponentInChildren<Blocker>( true );
    }

    // Update is called once per frame
    void Update()
    {
        Slider pSlider = m_pStamina.GetComponent<Slider>();
        pSlider.value += StaminaRegainSpeed * Time.deltaTime;

        if ( m_pBlocker.gameObject.activeSelf )
        {
            if ( pSlider.value >= ClickCost )
                m_pBlocker.gameObject.SetActive( false );
            Globals.InputEnabled = false;
            return;
        }

        if ( Input.GetMouseButtonUp( 0 ) || Input.GetKeyDown( KeyCode.DownArrow ) )
        {
            pSlider.value -= ClickCost;
            if ( pSlider.value < ClickCost )
                m_pBlocker.gameObject.SetActive( true );
            Globals.InputEnabled = true;
        }

        
    }
}
