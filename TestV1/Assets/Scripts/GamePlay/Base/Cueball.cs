using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cueball : Ball
{
    protected override void Start()
    {
        base.Start();
    }

    public void OnEnable()
    {
        GameEvents.RegisterEvent<Vector3, Vector3>(GameBasicEvent.Strike, OnStrike);
    }

    public void OnDisable()
    {
        GameEvents.UnregisterEvent<Vector3, Vector3>(GameBasicEvent.Strike, OnStrike);
    }

    public void OnStrike(Vector3 pos, Vector3 force)
    {
        if (phyEntity == null)
        {
            Debug.LogWarning("Cueball Strike failed: phyEntity is null");
            return;
        }
        Debug.Log($"Cueball Strike at pos:{pos}, impulse:{force}");
        BEPUutilities.Vector3 bepuImpulse = ConversionHelper.MathConverter.Convert(force);
        BEPUutilities.Vector3 bepuPos = ConversionHelper.MathConverter.Convert(pos);
        phyEntity.ApplyImpulse(bepuPos, bepuImpulse);
    }
}
