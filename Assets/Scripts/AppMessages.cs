using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppMessages : App
{
    MessageAlert m_pAlert;
    MessageAlertNotif m_pNotif;

    NoNewMessages m_pNoNewMessages;
    NewMessages m_pNewMessages;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        m_pAlert = transform.parent.GetComponentInChildren<MessageAlert>( true );
        m_pNotif = transform.parent.GetComponentInChildren<MessageAlertNotif>( true );
        m_pNoNewMessages = GetComponentInChildren<NoNewMessages>( true );
        m_pNewMessages = GetComponentInChildren<NewMessages>( true );

        if ( !m_pNotif.gameObject.activeSelf )
        {
            m_pNoNewMessages.gameObject.SetActive( true );
            m_pNewMessages.gameObject.SetActive( false );
        }
        else
        {
            m_pNewMessages.gameObject.SetActive( true );
            m_pNoNewMessages.gameObject.SetActive( false );
        }

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    void OnDisable()
    {
        m_pAlert.StopAllCoroutines();
        m_pAlert.StartCoroutine( m_pAlert.NewMessage() );
    }
}
