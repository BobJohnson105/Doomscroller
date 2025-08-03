using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TreeEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

enum AnimState : int
{
    NONE = 0,
    UP = 1,
    DOWN = -1
}
public class AppLoop : App, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    // types and weights
    static readonly (Type, int)[] s_pReelTypes = new (Type, int)[] {
        ( typeof( Cooking ), 10 ),
        ( typeof( News ), 10 ),
        ( typeof( Cat ), 10 ),
        ( typeof( Crosspost ), 10 ),
        ( typeof( Repost ), 10 ),
    };
    static readonly Dictionary<Type, LoopVideo[]> s_mapReelVideos = new();
    float m_fScrollProgress;
    float m_fDragStartScroll;
    (Reel, Type) m_pTopReel;
    (Reel, Type) m_pBotReel;
    AnimState m_iAnimState;
    public float AnimSpeed = 2.0f;
    Vector2 m_vStartDragPos;
    RectTransform m_pRectTransform;
    public void OnDrag( PointerEventData eventData )
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle( m_pRectTransform, eventData.position, eventData.pressEventCamera, out Vector2 pos );
        float fDeltaY = ( pos.y - m_vStartDragPos.y ) / m_pRectTransform.rect.height;
        m_fScrollProgress = Mathf.Clamp( fDeltaY + m_fDragStartScroll, 0.0f, 1.0f );
        m_pTopReel.Item1.Pos = m_fScrollProgress;
        m_pBotReel.Item1.Pos = m_fScrollProgress - 1.0f;
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

    protected override void OnEnable()
    {
        base.OnEnable();
        m_fScrollProgress = 0.0f;
        Reel[] pReels = transform.GetComponentsInChildren<Reel>();
        m_pRectTransform = GetComponent<RectTransform>();

        m_pTopReel.Item1 = pReels[ 0 ];
        m_pBotReel.Item1 = pReels[ 1 ];
        m_pTopReel.Item1.Pos = 0.0f;
        m_pBotReel.Item1.Pos = -1.0f;

        s_mapReelVideos.Clear();
        foreach ( (Type, int) pReelType in s_pReelTypes )
            s_mapReelVideos.Add( pReelType.Item1, GetComponentsInChildren( pReelType.Item1, true ).Select( e => (LoopVideo)e ).ToArray() );

        SelectNextVideo( ref m_pTopReel );
        SelectNextVideo( ref m_pBotReel );

        SetDeltas( m_pTopReel );

        GetComponentsInChildren<LoopVideo>();
    }

    void SelectNextVideo( ref (Reel, Type) pReel )
    {
        System.Random r = new();
        int iTotalWeightCount = 0;
        foreach ( (Type, int) pReelType in s_pReelTypes )
            iTotalWeightCount += pReelType.Item2;
        float fRandomVal = r.Next( iTotalWeightCount );
        int iCurrentWeightCount = 0;
        foreach ( (Type, int) pReelType in s_pReelTypes )
        {
            iCurrentWeightCount += pReelType.Item2;
            if ( iCurrentWeightCount > fRandomVal )
            {
                LoopVideo[] pVideos = s_mapReelVideos[ pReelType.Item1 ];
                LoopVideo pVideo = pVideos[ r.Next( pVideos.Length ) ];
                pReel.Item1.SetFrames( pVideo.GetComponentsInChildren<RawImage>().Select( i => i.texture ) );
                pReel.Item2 = pReelType.Item1;
                break;
            }
        }
    }

    void SetDeltas( (Reel, Type) pReel)
    {
        /// HERE is where we set what each type of video does
        if ( pReel.Item2 == typeof( Cooking ) )
            SetDeltas( 
                (StatType.HUNGER, 0.05f)
            );
        else if ( pReel.Item2 == typeof( News ) )
            SetDeltas( 
                (StatType.DEPRESSION, 0.05f)
            );
        else if ( pReel.Item2 == typeof( Cat ) )
            SetDeltas( 
                (StatType.FULFILLMENT, 0.05f)
            );
        else if ( pReel.Item2 == typeof( Crosspost ) )
            SetDeltas( 
                (StatType.STRESS, 0.05f)
            );
        else if ( pReel.Item2 == typeof( Repost ) )
            SetDeltas( 
                (StatType.ATTENTION, 0.05f)
            );
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
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
                    m_pTopReel.Item1.Pos -= 2;
                    m_fScrollProgress = 0.0f;
                    (m_pBotReel, m_pTopReel) = (m_pTopReel, m_pBotReel);
                    SelectNextVideo( ref m_pBotReel );
                    SetDeltas( m_pTopReel );
                }
                m_iAnimState = AnimState.NONE;
                return;
            }

            fAnimDist = Mathf.Min( fAnimDist, fMaxDist ) * (int)m_iAnimState;

            m_fScrollProgress += fAnimDist;
            m_pTopReel.Item1.Pos += fAnimDist;
            m_pBotReel.Item1.Pos += fAnimDist;
        }
    }

    void OnDisable()
    {
        ZeroAllDeltas();
        m_pTopReel.Item1.SetFrames( null );
        m_pBotReel.Item1.SetFrames( null );
        m_pTopReel.Item1.GetComponent<RawImage>().texture = null;
        m_pBotReel.Item1.GetComponent<RawImage>().texture = null;
    }

}
