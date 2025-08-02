using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageAlert : MonoBehaviour
{
    public float m_fMaxTime;
    public float m_fMinTime;
    MessageAlertNotif m_pNotif;
    void OnEnable()
    {
        m_pNotif = GetComponentInChildren<MessageAlertNotif>( true );
        StartCoroutine( NewMessage() );
    }

    public IEnumerator NewMessage()
    {
        yield return new WaitForSeconds( Random.value * ( m_fMaxTime - m_fMinTime ) + m_fMinTime );
        m_pNotif.gameObject.SetActive( true );
    }

    public void DisableAlert()
    {
        m_pNotif.gameObject.SetActive( false );
    }
}
