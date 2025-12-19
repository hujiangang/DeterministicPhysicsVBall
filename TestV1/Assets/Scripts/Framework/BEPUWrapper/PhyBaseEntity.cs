using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhyBaseEntity : MonoBehaviour
{

    public BEPUphysics.Entities.Entity phyEntity = null;
    public Vector3 center = Vector3.zero;
    [SerializeField]
    protected float mass = 1;

    public bool isTrigger = false;
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

        // 位置 - 修复：如果是触发器，不需要加center，直接使用模型位置
        // 触发器需要与模型完全对齐，避免提前碰撞
        transform.GetPositionAndRotation(out Vector3 currentPos, out Quaternion currentRot);

        // 对于非触发器，添加center偏移；对于触发器，直接使用模型位置
        Vector3 physicsPos = currentPos;
        if (!this.isTrigger && center != Vector3.zero)
        {
            physicsPos += currentRot * center;
        }
        
        this.phyEntity.position = ConversionHelper.MathConverter.Convert(physicsPos);
        this.phyEntity.orientation = ConversionHelper.MathConverter.Convert(currentRot);
    }

    public void SyncUnityTransformWithPhyTransform()
    {
        if (this.phyEntity == null) return;


        Vector3 physicsPos = ConversionHelper.MathConverter.Convert(phyEntity.position);
        Quaternion physicsRot = ConversionHelper.MathConverter.Convert(phyEntity.orientation);

        Vector3 unityPos = physicsPos;
        if (!isTrigger && center != Vector3.zero)
        {
            unityPos -= physicsRot * center;
        }
        transform.SetPositionAndRotation(unityPos, physicsRot);
    }

    public void LateUpdate()
    {
        if (this.phyEntity == null || this.isStatic) return;

        this.SyncUnityTransformWithPhyTransform();
    }
}
