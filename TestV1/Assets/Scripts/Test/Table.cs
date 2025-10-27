using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : PhyBoxEntity
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        AddTestMaterial();
    }

    public void AddTestMaterial()
    {
        if (this.phyEntity != null)
        {
            int paramIndex = 2;
            PhyMaterialTestParamater phyMaterialTest = TestParamater.BallMatParamaterList[paramIndex].tableMatParamater;

            this.phyEntity.Material = phyMaterialTest.material;
        }
    }
}
