using FixMath.NET;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PhySphereEntity : PhyBaseEntity
{
    public static float radius;

    protected static SphereCollider col;

    // Start is called before the first frame update
    protected virtual void Start()
    {

        col = this.gameObject.GetComponent<SphereCollider>();

        radius = col.radius;

        this.center = col.center;
        this.phyMat = col.material;
        this.isTrigger = col.isTrigger;
        
        if (gameObject.TryGetComponent<Rigidbody>(out var rigidbody))
        {
            if (rigidbody.mass > 0){
                this.mass = rigidbody.mass;
            }
        }

        if (this.isStatic)
        {
            this.phyEntity = new BEPUphysics.Entities.Prefabs.Sphere(BEPUutilities.Vector3.Zero, (Fix64)radius);
        }
        else
        {
            this.phyEntity = new BEPUphysics.Entities.Prefabs.Sphere(BEPUutilities.Vector3.Zero, (Fix64)radius, (FixMath.NET.Fix64)this.mass);
        }

        if (gameObject.TryGetComponent(out rigidbody))
        {
            phyEntity.LinearDamping = (Fix64)rigidbody.drag;
            phyEntity.AngularDamping = (Fix64)rigidbody.angularDrag;
        }

        if (phyMat != null) {
            phyEntity.material = new BEPUphysics.Materials.Material()
            {
                KineticFriction = (Fix64)phyMat.dynamicFriction,
                StaticFriction = (Fix64)phyMat.staticFriction,
                Bounciness = (Fix64)phyMat.bounciness,
            };
        }

        Debug.Log($"{name} PhySphereEntity Start: radius={radius}, isStatic={this.isStatic}, mass={this.mass}, LinearDamping:{phyEntity.LinearDamping}, AngularDamping : {phyEntity.AngularDamping}");

        this.AddSelfToPhyWorld();
        this.SyncPhyTransformWithUnityTransform();
    }
}
