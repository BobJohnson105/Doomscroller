using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

enum AnimState
{
    NONE,
    UP,
    DOWN
}

public class LoopSlider : MonoBehaviour, IPointerUpHandler
{
    Reel m_pTopReel;
    Reel m_pBotReel;
    Slider m_pSlider;
    AnimState m_iAnimState;
    public float m_fAnimSpeed = 2.0f;
    void OnEnable()
    {
        Reel[] pReels = transform.parent.GetComponentsInChildren<Reel>();

        for ( int i = 0; i < 2; ++i )
            if ( pReels[ i ].TopReel )
                m_pTopReel = pReels[ i ];
            else
                m_pBotReel = pReels[ i ];

        m_pSlider = GetComponent<Slider>();
    }

    public void SliderValueChanged()
    {
        if ( m_pSlider.interactable )
        {
            m_pTopReel.Pos = m_pSlider.value;
            m_pBotReel.Pos = m_pSlider.value - 1;
        }
    }

    void Update()
    {
        if ( m_iAnimState != AnimState.NONE )
        {
            float fAnimDist = m_fAnimSpeed * Time.deltaTime;
            float fMaxDist = 0.0f;
            if ( m_iAnimState == AnimState.UP )
                fMaxDist = 1.0f - m_pTopReel.Pos;
            else if ( m_iAnimState == AnimState.DOWN )
                fMaxDist = m_pTopReel.Pos;

            if ( fMaxDist == 0.0f )
            {
                if ( m_iAnimState == AnimState.UP )
                {
                    m_pTopReel.Pos -= 2;
                    (m_pBotReel, m_pTopReel) = (m_pTopReel, m_pBotReel);
                }
                m_iAnimState = AnimState.NONE;
                m_pSlider.interactable = true;
                return;
            }

            fAnimDist = Mathf.Min( fAnimDist, fMaxDist ) * ( m_iAnimState == AnimState.UP ? 1 : -1 );

            m_pTopReel.Pos += fAnimDist;
            m_pBotReel.Pos += fAnimDist;
        }
    }


    public void OnPointerUp( PointerEventData eventData )
    {
        if ( m_pSlider.value == 0.0f )
        {
            m_iAnimState = AnimState.NONE;
            return;
        }
        m_iAnimState = m_pSlider.value > 0.5f ? AnimState.UP : AnimState.DOWN;
        m_pSlider.interactable = false;
        m_pSlider.value = 0.0f;
    }
}
