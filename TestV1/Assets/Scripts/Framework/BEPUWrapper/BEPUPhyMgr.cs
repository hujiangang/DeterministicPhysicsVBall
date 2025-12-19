using FixMath.NET;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BEPUPhyMgr : MonoBehaviour
{
    public BEPUphysics.Space space;
    public static BEPUPhyMgr Instance;


    /// <summary>
    /// 初始化BEPU物理管理器
    /// </summary>
    public void Initialize()
    {
        if (BEPUPhyMgr.Instance != null)
        {
            return;
        }
        Physics.simulationMode = SimulationMode.Script; // 关闭原来物理引擎迭代;
        // Physics.autoSyncTransforms = false; // 关闭射线检测功能
        BEPUPhyMgr.Instance = this; // 初始化单例
        this.space = new BEPUphysics.Space(); // 创建物理世界
        this.space.ForceUpdater.gravity = new BEPUutilities.Vector3(0, -9.81m, 0); // 配置重力
        this.space.TimeStepSettings.TimeStepDuration = (Fix64)(1 / 120m); // 设置迭代时间间隔

        // 物体速度低于这个值，开始考虑休眠.
        this.space.DeactivationManager.VelocityLowerLimit = (Fix64)0.05m;

        //速度持续低于上限的时间超过这个值，才真正休眠.
        this.space.DeactivationManager.LowVelocityTimeMinimum = (Fix64)0.8m;

        // 如果撞击速度小于这个阈值，就直接忽略弹性.
        BEPUphysics.Settings.CollisionResponseSettings.BouncinessVelocityThreshold = (Fix64)0.01;

        //this.space.DeactivationManager.Enabled = false; // 先关闭休眠功能，测试时方便观察效果.;
    }
    
    public void Awake()
    {
        Initialize();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.space.Update();
    }

    private void DrawVisualModel()
    {
        // 绘制视觉模型边界用于比较
        Gizmos.color = Color.blue;
        
        // 绘制所有BoxCollider
        BoxCollider[] boxColliders = FindObjectsOfType<BoxCollider>();
        foreach (var boxCollider in boxColliders)
        {
            // 获取世界位置（考虑center偏移）
            Vector3 worldPosition = boxCollider.transform.position + 
                                boxCollider.transform.rotation * boxCollider.center;
            
            // 获取世界旋转
            Quaternion worldRotation = boxCollider.transform.rotation;
            
            // 保存当前矩阵
            Matrix4x4 originalMatrix = Gizmos.matrix;
            
            // 设置变换矩阵
            Gizmos.matrix = Matrix4x4.TRS(
                worldPosition,
                worldRotation,
                Vector3.one
            );
            
            // 绘制立方体（不考虑缩放）
            Gizmos.DrawWireCube(Vector3.zero, boxCollider.size);
            
            // 恢复矩阵
            Gizmos.matrix = originalMatrix;
        }
        
        // 绘制所有SphereCollider
        SphereCollider[] sphereColliders = FindObjectsOfType<SphereCollider>();
        foreach (var sphereCollider in sphereColliders)
        {
            // 获取世界位置（考虑center偏移）
            Vector3 worldPosition = sphereCollider.transform.position + 
                                sphereCollider.transform.rotation * sphereCollider.center;
            
            // 绘制球体（不考虑缩放）
            Gizmos.DrawWireSphere(worldPosition, sphereCollider.radius);
        }
    }

    private void DrawEntityBounds()
    {
        // 设置调试绘制颜色
        Gizmos.color = Color.yellow;
        
        // 遍历所有物理实体
        foreach (var entity in this.space.Entities)
        {
            if (entity != null)
            {
                // 转换物理位置到Unity位置
                Vector3 unityPos = ConversionHelper.MathConverter.Convert(entity.position);
                
                // 绘制碰撞体位置指示器
                Gizmos.DrawWireSphere(unityPos, 0.1f);
                
                // 如果是球体，绘制球体碰撞体
                if (entity is BEPUphysics.Entities.Prefabs.Sphere sphereEntity)
                {
                    Gizmos.DrawWireSphere(unityPos, (float)sphereEntity.Radius);
                }
                // 如果是立方体，绘制立方体碰撞体
                else if (entity is BEPUphysics.Entities.Prefabs.Box boxEntity)
                {
                    // 保存当前矩阵
                    Matrix4x4 originalMatrix = Gizmos.matrix;
                    
                    // 设置变换矩阵（位置和旋转，缩放为1）
                    Gizmos.matrix = Matrix4x4.TRS(
                        unityPos,
                        ConversionHelper.MathConverter.Convert(entity.orientation),
                        Vector3.one); // 注意：这里应该是Vector3.one
                    
                    // 绘制立方体碰撞体（传入实际的box尺寸）
                    Vector3 boxSize = new(
                        (float)boxEntity.Width, 
                        (float)boxEntity.Height, 
                        (float)boxEntity.Length);
                    Gizmos.DrawWireCube(Vector3.zero, boxSize);
                    
                    // 恢复矩阵
                    Gizmos.matrix = originalMatrix;
                }
            }
        }
    }


    /// <summary>
    /// 绘制调试信息，可视化物理碰撞体位置
    /// </summary>
    private void OnDrawGizmos()
    {
        // 只在编辑器模式下显示
#if UNITY_EDITOR
        if (this.space != null)
        {
            //DrawVisualModel();
            DrawEntityBounds();
        }
#endif
    }
}
