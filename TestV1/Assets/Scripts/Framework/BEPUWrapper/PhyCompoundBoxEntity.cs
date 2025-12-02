using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 处理多个 BoxCollider 的复合物理实体类
/// </summary>
public class PhyCompoundBoxEntity : PhyBaseEntity
{
    protected virtual void Start()
    {
        // 获取所有 BoxCollider 组件
        BoxCollider[] boxColliders = this.GetComponents<BoxCollider>();
        
        // 检查是否有 BoxCollider
        if (boxColliders.Length == 0)
        {
            Debug.LogWarning("PhyCompoundBoxEntity: No BoxCollider components found on " + gameObject.name);
            return;
        }
        
        // 创建形状条目列表
        List<BEPUphysics.CollisionShapes.CompoundShapeEntry> shapeEntries = new();

        // 为每个 BoxCollider 创建一个 CompoundShapeEntry
        foreach (BoxCollider boxCollider in boxColliders)
        {
            float width = boxCollider.size.x;
            float height = boxCollider.size.y;
            float length = boxCollider.size.z;

            // 使用第一个 collider 的属性作为实体的整体属性
            if (shapeEntries.Count == 0)
            {
                this.phyMat = boxCollider.material;
                this.isTrigger = boxCollider.isTrigger;
            }

            // 创建 BoxShape
            BEPUphysics.CollisionShapes.ConvexShapes.BoxShape boxShape = new(
                (FixMath.NET.Fix64)width, 
                (FixMath.NET.Fix64)height, 
                (FixMath.NET.Fix64)length);

            // 创建 CompoundShapeEntry，包含形状和本地变换
            BEPUutilities.Vector3 localPosition = new(
                (FixMath.NET.Fix64)boxCollider.center.x, 
                (FixMath.NET.Fix64)boxCollider.center.y, 
                (FixMath.NET.Fix64)boxCollider.center.z);

            BEPUutilities.RigidTransform localTransform = new(localPosition);
            BEPUphysics.CollisionShapes.CompoundShapeEntry shapeEntry = new(boxShape, localTransform);
            shapeEntries.Add(shapeEntry);
        }

        // 对于复合实体，center 应该为 Vector3.zero，因为 CompoundBody 会自动计算质心
        this.center = Vector3.zero;

        // 创建复合实体
        if (isStatic)
        {
            this.phyEntity = new BEPUphysics.Entities.Prefabs.CompoundBody(shapeEntries);
        }
        else
        {
            this.phyEntity = new BEPUphysics.Entities.Prefabs.CompoundBody(shapeEntries, (FixMath.NET.Fix64)this.mass);
        }

        this.AddSelfToPhyWorld();
        this.SyncPhyTransformWithUnityTransform();
    }
}