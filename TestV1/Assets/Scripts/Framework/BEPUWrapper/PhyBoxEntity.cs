using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PhyBoxEntity : PhyBaseEntity
{
    protected virtual void Start()
    {
        BoxCollider boxPhy = this.GetComponent<BoxCollider>();
        float width = boxPhy.size.x;
        float height = boxPhy.size.y;
        float length = boxPhy.size.z;

        this.center = boxPhy.center;
        this.phyMat = boxPhy.material;
        this.isTrigger = boxPhy.isTrigger;

        
        if (isStatic)
        {
            this.phyEntity = new BEPUphysics.Entities.Prefabs.Box(
                BEPUutilities.Vector3.Zero,
                (FixMath.NET.Fix64)width, (FixMath.NET.Fix64)height,
                (FixMath.NET.Fix64)length);
        }
        else
        {
            this.phyEntity = new BEPUphysics.Entities.Prefabs.Box(
                BEPUutilities.Vector3.Zero,
                (FixMath.NET.Fix64)width, (FixMath.NET.Fix64)height,
                (FixMath.NET.Fix64)length, (FixMath.NET.Fix64)this.mass);
        }

        this.AddSelfToPhyWorld();
        this.SyncPhyTransformWithUnityTransform();
    }
}
