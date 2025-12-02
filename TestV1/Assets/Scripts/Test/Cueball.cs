using FixMath.NET;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public void TestLeftSpinForcefully(Vector3 aimDir)
    {
        // 强制施加强烈的左旋转
        BEPUutilities.Vector3 strongTorque = new(0, 0, (Fix64)(-2f));
        phyEntity.ApplyAngularImpulse(ref strongTorque);

        // 同时给一个向前的力
        BEPUutilities.Vector3 aimDirVec = ConversionHelper.MathConverter.Convert(aimDir);
        BEPUutilities.Vector3 forwardImpulse =
            ConversionHelper.MathConverter.Convert(aimDir * 0.3f);
        phyEntity.ApplyLinearImpulse(ref forwardImpulse);
    }

}
