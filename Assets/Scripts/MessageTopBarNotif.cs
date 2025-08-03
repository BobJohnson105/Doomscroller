using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageTopBarNotif : MonoBehaviour
{
    void OnEnable()
    {
        StartCoroutine( DisableAfterDelay() );
    }

    IEnumerator DisableAfterDelay()
    {
        yield return new WaitForSeconds( 1.0f );
        gameObject.SetActive( false );
    }
}
