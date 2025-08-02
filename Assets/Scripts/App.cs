using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum StatType
{
    FULFILLMENT,
    DEPRESSION,
    HUNGER,
    STRESS,
    ATTENTION
}

public class App : MonoBehaviour
{
    // sliders and their change per second
    // 0: fulfillment
    // 1: depression
    // 2: hunger
    // 3: stress
    // 4: attention
    readonly (Slider, float)[] m_pStats = new (Slider, float)[ 5 ];

    protected virtual void OnEnable()
    {
        m_pStats[ 0 ].Item1 = GameObject.Find( "fulfillment" ).GetComponent<Slider>();
        m_pStats[ 1 ].Item1 = GameObject.Find( "depression" ).GetComponent<Slider>();
        m_pStats[ 2 ].Item1 = GameObject.Find( "hunger" ).GetComponent<Slider>();
        m_pStats[ 3 ].Item1 = GameObject.Find( "stress" ).GetComponent<Slider>();
        m_pStats[ 4 ].Item1 = GameObject.Find( "attention" ).GetComponent<Slider>();

        ZeroAllDeltas();
    }

    protected void ZeroAllDeltas()
    {
        for ( int i = 0; i < m_pStats.Length; ++i )
            m_pStats[ i ].Item2 = 0;
    }

    protected void SetDeltas( params (StatType, float)[] pDeltas )
    {
        ZeroAllDeltas();
        for ( int i = 0; i < pDeltas.Length; ++i )
        {
            switch ( pDeltas[ i ].Item1 )
            {
                case StatType.FULFILLMENT:
                    m_pStats[ 0 ].Item2 = pDeltas[ i ].Item2;
                    break;
                case StatType.DEPRESSION:
                    m_pStats[ 1 ].Item2 = pDeltas[ i ].Item2;
                    break;
                case StatType.HUNGER:
                    m_pStats[ 2 ].Item2 = pDeltas[ i ].Item2;
                    break;
                case StatType.STRESS:
                    m_pStats[ 3 ].Item2 = pDeltas[ i ].Item2;
                    break;
                case StatType.ATTENTION:
                    m_pStats[ 4 ].Item2 = pDeltas[ i ].Item2;
                    break;
            }
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        foreach ( var stat in m_pStats )
            stat.Item1.value += stat.Item2 * Time.deltaTime;
    }
}
