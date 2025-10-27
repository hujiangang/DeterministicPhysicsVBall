using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cueball : Ball
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    public void Strike(Vector3 pos, Vector3 impulse)
    {
        if (this.phyEntity == null)
        {
            Debug.LogWarning("Cueball Strike failed: phyEntity is null");
            return;
        }

        Debug.Log($"Cueball Strike at pos:{pos}, impulse:{impulse}");

        BEPUutilities.Vector3 bepuImpulse = ConversionHelper.MathConverter.Convert(impulse);
        BEPUutilities.Vector3 bepuPos = ConversionHelper.MathConverter.Convert(pos);
        this.phyEntity.ApplyImpulse(bepuPos, bepuImpulse);
    }

}
