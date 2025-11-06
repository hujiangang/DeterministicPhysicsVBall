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
    public PhyMaterialTestParamater cushionMatParamater = new()
    {
        material = new BEPUphysics.Materials.Material(
                    staticFriction: 0.25m,
                    kineticFriction: 0.02m,
                    bounciness: 0.9m),
    };
}

public static class TestParamater
{



    public static int TestIndex = 5;

    public static List<TestMatGroup> BallMatParamaterList = new()
    {
        new TestMatGroup{
            ballMatParamater = new PhyMaterialTestParamater{
                 // 测试效果-力量小同时回滚.
                 /* 原因
                    静摩擦系数 > 动摩擦系数（常见 0.30 vs 0.25）
                    弹性≈1（0.92~1.0）
                    速度进入睡眠阈值（0.26 m/s 或你改的 0.02 m/s）时，
                    引擎为了“让物体尽快睡”，会把剩余 微小切向动能 通过 静摩擦锥 反弹，
                    结果就产生 反向微冲量 → 肉眼看到“回滚”。
                  */
                material = new BEPUphysics.Materials.Material(
                    staticFriction: 0.30m,
                    kineticFriction: 0.25m,
                    bounciness: 0.92m),
                linearDamping = 0.05m,
                angularDamping = 0.05m
            },
            tableMatParamater = new PhyMaterialTestParamater{
                material = new BEPUphysics.Materials.Material(
                     staticFriction: 0.30m,
                     kineticFriction: 0.25m,
                     bounciness: 0.85m)
            }
        },


         new TestMatGroup{
            ballMatParamater = new PhyMaterialTestParamater{
                material = new BEPUphysics.Materials.Material(
                    staticFriction: 0m,
                    kineticFriction: 0.25m,
                    bounciness: 0.92m),
                linearDamping = 0.05m,
                angularDamping = 0.02m
            },
            tableMatParamater = new PhyMaterialTestParamater{
                material = new BEPUphysics.Materials.Material(
                     kineticFriction: 0.20m,
                     staticFriction: 0.20m,
                     bounciness: 0.85m)
            }
        },

         new TestMatGroup{
            ballMatParamater = new PhyMaterialTestParamater{
                material = new BEPUphysics.Materials.Material(
                    staticFriction: 0.28m,
                    kineticFriction: 0.25m,
                    bounciness: 0.92m),
                linearDamping = 0.01m,
                angularDamping = 0.01m
            },
            tableMatParamater = new PhyMaterialTestParamater{
                material = new BEPUphysics.Materials.Material(
                     kineticFriction: 0.15m,
                     staticFriction: 0.15m,
                     bounciness: 0.85m)
            }
        },

         new TestMatGroup{
            ballMatParamater = new PhyMaterialTestParamater{
                material = new BEPUphysics.Materials.Material(
                    staticFriction: 0m,
                    kineticFriction: 0.005m,
                    bounciness: 1m),
                linearDamping = 0.01m,
                angularDamping = 0.01m
            },
            tableMatParamater = new PhyMaterialTestParamater{
                material = new BEPUphysics.Materials.Material(
                     kineticFriction: 0.001m,
                     staticFriction: 0m,
                     bounciness: 0.3m)
            }
        },

         new TestMatGroup{
             // 还行吧.
            ballMatParamater = new PhyMaterialTestParamater{
                material = new BEPUphysics.Materials.Material(
                    staticFriction: 0.05m,
                    kineticFriction: 0.02m,
                    bounciness: 0.93m),
                linearDamping = 0.008m,
                angularDamping = 0.04m
            },
            tableMatParamater = new PhyMaterialTestParamater{
                material = new BEPUphysics.Materials.Material(
                     kineticFriction: 0.02m,
                     staticFriction: 0.05m,
                     bounciness: 0.05m)
            }
        },
         new TestMatGroup{
            ballMatParamater = new PhyMaterialTestParamater{
                material = new BEPUphysics.Materials.Material(
                    staticFriction: (Fix64) 0.25m,
                    kineticFriction: (Fix64) 0.03m,
                    bounciness: (Fix64) 0.93m),
                linearDamping = (Fix64) 0.02m,
                angularDamping = (Fix64) 0.015m
            },
            tableMatParamater = new PhyMaterialTestParamater{
                material = new BEPUphysics.Materials.Material(
                     kineticFriction: (Fix64) 0.03m,
                     staticFriction: (Fix64) 0.3m,
                     bounciness: (Fix64) 0.05m)
            },
            cushionMatParamater = new PhyMaterialTestParamater
            {
                material = new BEPUphysics.Materials.Material(
                    staticFriction: (Fix64) 0.25m,
                    kineticFriction: (Fix64) 0.02m,
                    bounciness: (Fix64) 0.9m
                    )
            }
        },
         new TestMatGroup{
            ballMatParamater = new PhyMaterialTestParamater{
                material = new BEPUphysics.Materials.Material(
                    staticFriction: (Fix64) 0.15m,
                    kineticFriction: (Fix64) 0.1m,
                    bounciness: (Fix64) 0.8m),
                linearDamping = (Fix64) 0.03m,
                angularDamping = (Fix64)0.05m
            },
            tableMatParamater = new PhyMaterialTestParamater{
                material = new BEPUphysics.Materials.Material(
                     staticFriction: (Fix64) 0.15m,
                     kineticFriction: (Fix64)0.1m,
                     bounciness: (Fix64)0.8m
                    )
            },
            cushionMatParamater = new PhyMaterialTestParamater
            {
                material = new BEPUphysics.Materials.Material(
                    staticFriction: (Fix64)0.15m,
                     kineticFriction: (Fix64)0.1m,
                     bounciness: (Fix64)0.8m
                    )
            }
        }
    };
}
