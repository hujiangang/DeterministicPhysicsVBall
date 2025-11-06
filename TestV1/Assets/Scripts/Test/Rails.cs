using Palmmedia.ReportGenerator.Core.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rails : PhyBoxEntity
{
    protected override void Start()
    {
        base.Start();

        AddTestMaterial();
    }

    public void AddTestMaterial()
    {
        if (this.phyEntity != null)
        {
            int paramIndex = TestParamater.TestIndex;
            PhyMaterialTestParamater phyMaterialTest = TestParamater.BallMatParamaterList[paramIndex].cushionMatParamater;

            this.phyEntity.Material = phyMaterialTest.material;
            this.phyEntity.LinearDamping = phyMaterialTest.linearDamping;
            this.phyEntity.AngularDamping = phyMaterialTest.angularDamping;
        }
    }

    float logTime = 0;

    public void Update()
    {
        if (this.phyEntity != null)
        {
            logTime += Time.deltaTime;

            if (logTime > 1)
            {
                //Debug.Log($"Rails Material {JsonSerializer.ToJsonString(this.phyEntity.Material)}");
                //Debug.Log($"rail IsDynamic={this.phyEntity.IsDynamic}");

                logTime = 0;
            }
        }
    }

}
