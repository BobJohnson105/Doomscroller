using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Uber : App
{
    public Canvas order;
    public Canvas pay;
    public Canvas complete;

    protected override void OnEnable()
    {
        base.OnEnable();
        order.enabled = true;
        pay.enabled = false;
        complete.enabled = false;
    }

    public void Order()
    {
        order.enabled = false;
        pay.enabled = true;
    }
    public void Pay()
    {
        pay.enabled = false;
        complete.enabled = true;
        StartCoroutine(delay());

        m_pStats[ 2 ].Item1.value -= 0.5f;
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(5);
        order.enabled = true;
        pay.enabled = false;
        complete.enabled = false;
    }
}
