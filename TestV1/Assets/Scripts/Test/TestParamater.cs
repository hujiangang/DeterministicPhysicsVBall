using FixMath.NET;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PhyMaterialTestParamater
{

    // 材质.
    public BEPUphysics.Materials.Material material;

    //线速度每秒 损失 5%.
    public Fix64 linearDamping = 0.02m;
    //角速度每秒 损失 5%.
    public Fix64 angularDamping = 0.02m;
}

public class TestMatGroup
{
    public PhyMaterialTestParamater ballMatParamater;
    public PhyMaterialTestParamater tableMatParamater;
}

public static class TestParamater
{
    public static List<TestMatGroup> BallMatParamaterList = new()
    {
        new TestMatGroup{
            ballMatParamater = new PhyMaterialTestParamater{
                 // 测试效果-停止突兀.
                material = new BEPUphysics.Materials.Material(
                    staticFriction: 0.30m,
                    kineticFriction: 0.25m,
                    bounciness: 0.92m),
                linearDamping = 0.05m,
                angularDamping = 0.05m
            },
            tableMatParamater = new PhyMaterialTestParamater{
                 // 测试效果-停止突兀.
                material = new BEPUphysics.Materials.Material(
                     staticFriction: 0.30m,
                     kineticFriction: 0.25m,
                     bounciness: 0.85m)
            }
        },


         new TestMatGroup{
            ballMatParamater = new PhyMaterialTestParamater{
                 // 测试效果-停止突兀.
                material = new BEPUphysics.Materials.Material(
                    staticFriction: 0.28m,
                    kineticFriction: 0.25m,
                    bounciness: 0.92m),
                linearDamping = 0.02m,
                angularDamping = 0.02m
            },
            tableMatParamater = new PhyMaterialTestParamater{
                 // 测试效果-停止突兀.
                material = new BEPUphysics.Materials.Material(
                     kineticFriction: 0.20m,
                     staticFriction: 0.20m,
                     bounciness: 0.85m)
            }
        },

         new TestMatGroup{
            ballMatParamater = new PhyMaterialTestParamater{
                 // 测试效果-停止突兀.
                material = new BEPUphysics.Materials.Material(
                    staticFriction: 0.28m,
                    kineticFriction: 0.25m,
                    bounciness: 0.92m),
                linearDamping = 0.01m,
                angularDamping = 0.01m
            },
            tableMatParamater = new PhyMaterialTestParamater{
                 // 测试效果-停止突兀.
                material = new BEPUphysics.Materials.Material(
                     kineticFriction: 0.15m,
                     staticFriction: 0.15m,
                     bounciness: 0.85m)
            }
        }
    };
}
