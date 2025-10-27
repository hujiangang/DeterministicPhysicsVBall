using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PhySphereEntity : PhyBaseEntity
{
    // Start is called before the first frame update
    protected virtual void Start()
    {

        SphereCollider sphere = this.gameObject.GetComponent<SphereCollider>();

        float radius = sphere.radius;

        this.center = sphere.center;
        this.phyMat = sphere.material;
        this.isTrigger = sphere.isTrigger;

        if (this.isStatic)
        {
            this.phyEntity = new BEPUphysics.Entities.Prefabs.Sphere(BEPUutilities.Vector3.Zero, (FixMath.NET.Fix64)radius);
        }
        else
        {
            this.phyEntity = new BEPUphysics.Entities.Prefabs.Sphere(BEPUutilities.Vector3.Zero, (FixMath.NET.Fix64)radius, (FixMath.NET.Fix64)this.mass);
        }

        Debug.Log($"{name} PhySphereEntity Start: radius={radius}, isStatic={this.isStatic}, mass={this.mass}");

        this.AddSelfToPhyWorld();
        this.SyncPhyTransformWithUnityTransform();
    }
}
