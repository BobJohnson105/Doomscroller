using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reel : MonoBehaviour
{
    public float Pos
    {
        get => m_fPos;
        set
        {
            m_fPos = value;
            m_pRectTransform.anchorMin = new( m_pRectTransform.anchorMin.x, m_fPos );
            m_pRectTransform.anchorMax = new( m_pRectTransform.anchorMax.x, m_fPos + 1 );
        }
    }
    public bool TopReel => m_fPos >= 0.0f;
    [SerializeField]
    float m_fPos;
    RectTransform m_pRectTransform;
    void OnEnable()
    {
        m_pRectTransform = GetComponent<RectTransform>();
        m_fPos = m_pRectTransform.anchorMin.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
