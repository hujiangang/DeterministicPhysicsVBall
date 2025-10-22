using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhyBaseEntity : MonoBehaviour
{

    protected BEPUphysics.Entities.Entity phyEntity = null;
    protected Vector3 center = Vector3.zero;
    [SerializeField]
    protected float mass = 1;

    protected bool isTrigger = false;
    protected PhysicMaterial phyMat = null;

    [SerializeField]
    protected bool isStatic = false;


    /// <summary>
    /// 增加自己到物理世界中.
    /// </summary>
    public void AddSelfToPhyWorld()
    {
        if (this.phyEntity == null)
        {
            return;
        }

        BEPUPhyMgr.Instance.space.Add(this.phyEntity);
    }

    public void SyncPhyTransformWithUnityTransform()
    {
        if (this.phyEntity == null)
        {
            return;
        }


        // 位置
        Vector3 unityPos = this.transform.position;
        unityPos += this.center;
        this.phyEntity.position = ConversionHelper.MathConverter.Convert(unityPos);
        // end

        // 旋转
        Quaternion rot = this.transform.rotation;
        this.phyEntity.orientation = ConversionHelper.MathConverter.Convert(rot);
        // end
    }
}
