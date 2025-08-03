using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppBreaths : App
{
    protected override void OnEnable()
    {
        base.OnEnable();
        SetDeltas( (StatType.DEPRESSION, -0.05f) );
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
