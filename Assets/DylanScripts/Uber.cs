using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uber : MonoBehaviour
{
    public Canvas order;
    public Canvas pay;
    public Canvas complete;
    // Start is called before the first frame update
    void Start()
    {
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
    }
}
