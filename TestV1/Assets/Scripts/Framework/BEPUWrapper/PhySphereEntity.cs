using FixMath.NET;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PhySphereEntity : PhyBaseEntity
{
    public float radius;

    protected SphereCollider col;

    // Start is called before the first frame update
    protected virtual void Start()
    {

        col = this.gameObject.GetComponent<SphereCollider>();

        radius = col.radius;

        this.center = col.center;
        this.phyMat = col.material;
        this.isTrigger = col.isTrigger;

        if (this.isStatic)
        {
            this.phyEntity = new BEPUphysics.Entities.Prefabs.Sphere(BEPUutilities.Vector3.Zero, (Fix64)radius);
        }
        else
        {
            this.phyEntity = new BEPUphysics.Entities.Prefabs.Sphere(BEPUutilities.Vector3.Zero, (Fix64)radius, (FixMath.NET.Fix64)this.mass);
        }

        Debug.Log($"{name} PhySphereEntity Start: radius={radius}, isStatic={this.isStatic}, mass={this.mass}");

        this.AddSelfToPhyWorld();
        this.SyncPhyTransformWithUnityTransform();
    }
}
