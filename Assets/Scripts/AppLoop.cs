using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Globals;

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
    public float AttentionSpan = 5.0f;
    public float NewReelBoredomDecrease = 0.05f;
    public float OldReelBoredomIncrease = 0.05f;
    Vector2 m_vStartDragPos;
    RectTransform m_pRectTransform;
    float m_fTimeOnReel;
    bool m_bInteractionEnabled = false;
    public void OnDrag( PointerEventData eventData )
    {
        if ( !m_bInteractionEnabled ) return;
        RectTransformUtility.ScreenPointToLocalPointInRectangle( m_pRectTransform, eventData.position, eventData.pressEventCamera, out Vector2 pos );
        float fDeltaY = ( pos.y - m_vStartDragPos.y ) / m_pRectTransform.rect.height;
        m_fScrollProgress = Mathf.Clamp( fDeltaY + m_fDragStartScroll, 0.0f, 1.0f );
        m_pTopReel.Item1.Pos = m_fScrollProgress;
        m_pBotReel.Item1.Pos = m_fScrollProgress - 1.0f;
    }
    public void OnBeginDrag( PointerEventData eventData )
    {
        if ( !m_bInteractionEnabled ) return;
        RectTransformUtility.ScreenPointToLocalPointInRectangle( m_pRectTransform, eventData.position, eventData.pressEventCamera, out m_vStartDragPos );
        m_iAnimState = AnimState.NONE;
        m_fDragStartScroll = m_fScrollProgress;
        OnDrag( eventData );
    }

    public void OnEndDrag( PointerEventData eventData )
    {
        if ( !m_bInteractionEnabled ) return;
        m_iAnimState = m_fScrollProgress >= 0.5f ? AnimState.UP : AnimState.DOWN;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        m_bInteractionEnabled = false;
        m_fScrollProgress = 0.0f;
        Reel[] pReels = transform.GetComponentsInChildren<Reel>( true );
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

        StartCoroutine( EnableReels() );
    }

    IEnumerator EnableReels()
    {
        yield return new WaitForSeconds( 1.0f );
        m_bInteractionEnabled = true;
        m_fTimeOnReel = 0.0f;
        m_pTopReel.Item1.gameObject.SetActive( true );
        m_pBotReel.Item1.gameObject.SetActive( true );
    }

    void SelectNextVideo( ref (Reel, Type) pReel )
    {
        System.Random r = new();
        int iTotalWeightCount = 0;
        foreach ( (Type, int) pReelType in s_pReelTypes )
            iTotalWeightCount += pReelType.Item2;
        float fRandomVal = r.Next( iTotalWeightCount );
        int iCurrentWeightCount = 0;
        for ( int i = 0; i < s_pReelTypes.Length; ++i )
        {
            Type pReelType = s_pReelTypes[ i ].Item1;
            int iReelWeight = s_pReelTypes[ i ].Item2;
            iCurrentWeightCount += iReelWeight;
            if ( iCurrentWeightCount > fRandomVal )
            {
                LoopVideo[] pVideos = s_mapReelVideos[ pReelType ];
                LoopVideo pVideo = pVideos[ r.Next( pVideos.Length ) ];
                pReel.Item1.SetFrames( pVideo.GetComponentsInChildren<RawImage>().Select( i => i.texture ) );
                pReel.Item2 = pReelType;

                for ( int j = 0; j < s_pReelTypes.Length; ++j )
                    if ( s_pReelTypes[ j ].Item2 == 0 )
                        s_pReelTypes[ j ].Item2 = 10;
                s_pReelTypes[ i ].Item2 = 0;

                break;
            }
        }
    }

    void SetDeltas( (Reel, Type) pReel )
    {
        const float MassiveIncrease = 0.05f;
        const float Increase = 0.02f;
        /// HERE is where we set what each type of video does
        if ( pReel.Item2 == typeof( Cooking ) )
            SetDeltas( 
                (StatType.HUNGER, Increase),
                (StatType.ANGER, -Increase),
                (StatType.STRESS, -Increase)
            );
        else if ( pReel.Item2 == typeof( News ) )
            SetDeltas( 
                (StatType.DEPRESSION, MassiveIncrease),
                (StatType.ANGER, MassiveIncrease),
                (StatType.STRESS, MassiveIncrease)
            );
        else if ( pReel.Item2 == typeof( Cat ) )
            SetDeltas( 
                (StatType.ANGER, -Increase),
                (StatType.STRESS, -Increase),
                (StatType.DEPRESSION, Increase)
            );
        else if ( pReel.Item2 == typeof( Crosspost ) )
            SetDeltas( 
                (StatType.DEPRESSION, Increase)
            );
        else if ( pReel.Item2 == typeof( Repost ) )
            SetDeltas( 
                (StatType.DEPRESSION, Increase),
                (StatType.ANGER, Increase)
            );
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        m_fTimeOnReel += Time.deltaTime;

        if ( m_fTimeOnReel >= AttentionSpan )
            m_pStats[ 4 ].Item2 = OldReelBoredomIncrease;
        else
            m_pStats[ 4 ].Item2 = -NewReelBoredomDecrease;

        if ( m_iAnimState != AnimState.NONE ) //update called frequently, save us some work
        {
            float fAnimDist = AnimSpeed * Time.deltaTime;
            float fMaxDist = 0.0f;
            if ( m_iAnimState == AnimState.UP )
                fMaxDist = 1.0f - m_fScrollProgress;
            else if ( m_iAnimState == AnimState.DOWN )
                fMaxDist = m_fScrollProgress;


            float fRealAnimDist = Mathf.Min( fAnimDist, fMaxDist ) * (int)m_iAnimState;

            m_fScrollProgress += fRealAnimDist;
            m_pTopReel.Item1.Pos += fRealAnimDist;
            m_pBotReel.Item1.Pos += fRealAnimDist;

            if ( m_fScrollProgress >= 1.0f || m_fScrollProgress <= -1.0f )
            {
                if ( m_iAnimState == AnimState.UP )
                {
                    m_pTopReel.Item1.Pos -= 2;
                    m_fScrollProgress = 0.0f;
                    (m_pBotReel, m_pTopReel) = (m_pTopReel, m_pBotReel);
                    SelectNextVideo( ref m_pBotReel );
                    SetDeltas( m_pTopReel );

                    m_pBotReel.Item1.StopAllCoroutines();
                    m_pBotReel.Item1.StartCoroutine( m_pBotReel.Item1.PlayVideo() );
                    m_fTimeOnReel = 0.0f;
                }
                m_iAnimState = AnimState.NONE;
                return;
            }
        }
        if ( InputEnabled && Input.GetKeyDown( KeyCode.DownArrow ) )
            m_iAnimState = AnimState.UP;
    }

    void OnDisable()
    {
        ZeroAllDeltas();
        m_pTopReel.Item1.SetFrames( null );
        m_pBotReel.Item1.SetFrames( null );
        m_pTopReel.Item1.GetComponent<RawImage>().texture = null;
        m_pBotReel.Item1.GetComponent<RawImage>().texture = null;
        m_pTopReel.Item1.gameObject.SetActive( false );
        m_pBotReel.Item1.gameObject.SetActive( false );
    }

}
