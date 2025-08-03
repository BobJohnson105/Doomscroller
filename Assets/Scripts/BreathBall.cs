using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Direction
{
    UP, DOWN, LEFT, RIGHT
}

public class BreathBall : MonoBehaviour
{
    public float MoveSpeed = 1.0f;
    Direction m_iDir;
    RectTransform m_pTransform;

    void OnEnable()
    {
        m_pTransform = GetComponent<RectTransform>();
        m_pTransform.anchorMin = new Vector2( 0.08f, 0.1f );
        m_pTransform.anchorMax = new Vector2( 0.33f, 0.3f );
        m_iDir = Direction.UP;
    }

    // Update is called once per frame
    void Update()
    {
        float fBaseMoveDist = MoveSpeed * Time.deltaTime;
        switch ( m_iDir )
        {
            case Direction.UP:
            {
                float fMoveDist = Mathf.Min( fBaseMoveDist, 0.9f - m_pTransform.anchorMax.y );
                m_pTransform.anchorMin += fMoveDist * Vector2.up;
                m_pTransform.anchorMax += fMoveDist * Vector2.up;

                if ( fMoveDist != fBaseMoveDist )
                    m_iDir = Direction.RIGHT;

                break;
            }
            case Direction.RIGHT:
            {
                float fMoveDist = Mathf.Min( fBaseMoveDist, 0.92f - m_pTransform.anchorMax.x );
                m_pTransform.anchorMin += fMoveDist * Vector2.right;
                m_pTransform.anchorMax += fMoveDist * Vector2.right;

                if ( fMoveDist != fBaseMoveDist )
                    m_iDir = Direction.DOWN;

                break;
            }
            case Direction.DOWN:
            {
                float fMoveDist = Mathf.Min( fBaseMoveDist, m_pTransform.anchorMin.y - 0.1f );
                m_pTransform.anchorMin += fMoveDist * Vector2.down;
                m_pTransform.anchorMax += fMoveDist * Vector2.down;

                if ( fMoveDist != fBaseMoveDist )
                    m_iDir = Direction.LEFT;

                break;
            }
            case Direction.LEFT:
            {
                float fMoveDist = Mathf.Min( fBaseMoveDist, m_pTransform.anchorMin.x - 0.08f );
                m_pTransform.anchorMin += fMoveDist * Vector2.left;
                m_pTransform.anchorMax += fMoveDist * Vector2.left;

                if ( fMoveDist != fBaseMoveDist )
                    m_iDir = Direction.UP;

                break;
            }
        }
    }
}
