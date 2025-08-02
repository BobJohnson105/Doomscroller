using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reel : MonoBehaviour
{
    public float FPS = 2.0f;
    public float Pos
    {
        get => m_fPos;
        set
        {
            m_fPos = value;
            if ( !m_pRectTransform )
                m_pRectTransform = GetComponent<RectTransform>();
            m_pRectTransform.anchorMin = new( m_pRectTransform.anchorMin.x, m_fPos );
            m_pRectTransform.anchorMax = new( m_pRectTransform.anchorMax.x, m_fPos + 1 );
        }
    }
    [SerializeField]
    float m_fPos;
    RawImage m_pImage;

    readonly List<Texture> m_pFrames = new();
    int m_iAnimFrame;

    RectTransform m_pRectTransform;
    void OnEnable()
    {
        m_iAnimFrame = 0;
        m_pRectTransform = GetComponent<RectTransform>();
        m_fPos = m_pRectTransform.anchorMin.y;

        m_pImage = GetComponent<RawImage>();

        foreach ( RawImage i in transform.parent.GetComponentInChildren<cooking1>( true ).GetComponentsInChildren<RawImage>() )
            m_pFrames.Add( i.texture );

        StartCoroutine( PlayVideo() );
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator PlayVideo()
    {
        while ( true )
        {
            yield return new WaitForSeconds( 1.0f / FPS );
            m_iAnimFrame++;
            m_iAnimFrame %= m_pFrames.Count;
            m_pImage.texture = m_pFrames[ m_iAnimFrame ];
        }
    }
}
