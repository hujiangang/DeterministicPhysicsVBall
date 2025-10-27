using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : PhySphereEntity
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        AddTestMaterial();
    }

    public void AddTestMaterial()
    {
        var ballMat = new BEPUphysics.Materials.Material(
            staticFriction: 0.30m,
            kineticFriction: 0.25m,
            bounciness: 0.92m);

        if (this.phyEntity != null)
        {
            int paramIndex = 2;
            PhyMaterialTestParamater phyMaterialTest = TestParamater.BallMatParamaterList[paramIndex].ballMatParamater;

            phyMaterialTest.material = ballMat;
            this.phyEntity.LinearDamping = phyMaterialTest.linearDamping;
            this.phyEntity.AngularDamping = phyMaterialTest.angularDamping;
        }
    }
}
