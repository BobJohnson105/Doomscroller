using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

enum AnimState : int
{
    NONE =  0,
    UP   =  1,
    DOWN = -1
}
public class AppLoop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    float m_fScrollProgress;
    float m_fDragStartScroll;
    Reel m_pTopReel;
    Reel m_pBotReel;
    AnimState m_iAnimState;
    public float AnimSpeed = 2.0f;
    Vector2 m_vStartDragPos;
    RectTransform m_pRectTransform;
    public void OnDrag( PointerEventData eventData )
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle( m_pRectTransform, eventData.position, eventData.pressEventCamera, out Vector2 pos );
        float fDeltaY = ( pos.y - m_vStartDragPos.y ) / m_pRectTransform.rect.height;
        m_fScrollProgress = Mathf.Clamp( fDeltaY + m_fDragStartScroll, 0.0f, 1.0f );
        m_pTopReel.Pos = m_fScrollProgress;
        m_pBotReel.Pos = m_fScrollProgress - 1.0f;
    }
    public void OnBeginDrag( PointerEventData eventData )
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle( m_pRectTransform, eventData.position, eventData.pressEventCamera, out m_vStartDragPos );
        m_iAnimState = AnimState.NONE;
        m_fDragStartScroll = m_fScrollProgress;
        OnDrag( eventData );
    }

    public void OnEndDrag( PointerEventData eventData )
    {
        m_iAnimState = m_fScrollProgress >= 0.5f ? AnimState.UP : AnimState.DOWN;
    }

    void OnEnable()
    {
        m_fScrollProgress = 0.0f;
        Reel[] pReels = transform.parent.GetComponentsInChildren<Reel>();
        m_pRectTransform = GetComponent<RectTransform>();

        for ( int i = 0; i < 2; ++i )
            if ( pReels[ i ].TopReel )
                m_pTopReel = pReels[ i ];
            else
                m_pBotReel = pReels[ i ];
    }

    // Update is called once per frame
    void Update()
    {
        if ( m_iAnimState != AnimState.NONE ) //update called frequently, save us some work
        {
            float fAnimDist = AnimSpeed * Time.deltaTime;
            float fMaxDist = 0.0f;
            if ( m_iAnimState == AnimState.UP )
                fMaxDist = 1.0f - m_fScrollProgress;
            else if ( m_iAnimState == AnimState.DOWN )
                fMaxDist = m_fScrollProgress;

            if ( fMaxDist == 0.0f )
            {
                if ( m_iAnimState == AnimState.UP )
                {
                    m_pTopReel.Pos -= 2;
                    m_fScrollProgress = 0.0f;
                    (m_pBotReel, m_pTopReel) = (m_pTopReel, m_pBotReel);
                }
                m_iAnimState = AnimState.NONE;
                return;
            }

            fAnimDist = Mathf.Min( fAnimDist, fMaxDist ) * (int)m_iAnimState;

            m_fScrollProgress += fAnimDist;
            m_pTopReel.Pos += fAnimDist;
            m_pBotReel.Pos += fAnimDist;
        }
    }

}
