using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 集中式物理实体管理器
/// 替代将物理脚本挂载在每个对象上的方式
/// </summary>
public class PhyEntityManager : MonoBehaviour
{
    /// <summary>
    /// 单例实例
    /// </summary>
    public static PhyEntityManager Instance { get; private set; }
    
    /// <summary>
    /// 物理实体映射表：Unity对象 -> 物理实体数据
    /// </summary>
    private Dictionary<GameObject, PhyEntityData> phyEntityMap = new();
    
    /// <summary>
    /// 物理实体数据类
    /// </summary>
    private class PhyEntityData
    {
        /// <summary>
        /// BEPU物理实体
        /// </summary>
        public BEPUphysics.Entities.Entity PhyEntity { get; set; }
        
        /// <summary>
        /// 中心偏移
        /// </summary>
        public Vector3 Center { get; set; }
        
        /// <summary>
        /// 是否为静态
        /// </summary>
        public bool IsStatic { get; set; }
    }
    
    /// <summary>
    /// 初始化单例实例
    /// </summary>
    public void Initialize()
    {
        // 初始化单例
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        // 只有在运行时才调用 DontDestroyOnLoad
        if (Application.isPlaying)
        {
            // 确保不被销毁
            DontDestroyOnLoad(gameObject);
        }
    }
    
    private void Awake()
    {
        Initialize();
    }
    
    private void Start()
    {
        // 初始化时扫描场景中的物理对象
        ScanSceneForPhysicsObjects();
    }
    
    private void Update()
    {
        // 同步Unity变换到物理变换
        SyncUnityToPhysics();
    }
    
    private void LateUpdate()
    {
        // 同步物理变换到Unity变换
        SyncPhysicsToUnity();
    }
    
    /// <summary>
    /// 扫描场景中的物理对象
    /// </summary>
    public void ScanSceneForPhysicsObjects()
    {
        // 清除现有映射
        ClearAllPhyEntities();
        
        // 获取场景中所有游戏对象
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        
        // 遍历所有对象
        foreach (GameObject obj in allObjects)
        {
            // 跳过已经挂载了PhyBaseEntity的对象
            if (obj.GetComponent<PhyBaseEntity>() != null)
                continue;
            
            // 尝试创建物理实体
            CreatePhyEntityForObject(obj);
        }
        
        Debug.Log("Scanned scene for physics objects. Created " + phyEntityMap.Count + " physics entities.");
    }
    
    /// <summary>
    /// 为对象创建物理实体
    /// </summary>
    /// <param name="obj">游戏对象</param>
    private void CreatePhyEntityForObject(GameObject obj)
    {
        BEPUphysics.Entities.Entity phyEntity = null;
        Vector3 center = Vector3.zero;
        bool isStatic = true; // 默认静态
        
        // 检查BoxCollider
        BoxCollider[] boxColliders = obj.GetComponents<BoxCollider>();
        PhysicMaterial mat = null;
        if (boxColliders.Length > 0)
        {
            if (boxColliders.Length == 1)
            {
                // 单个BoxCollider
                BoxCollider boxCollider = boxColliders[0];
                float width = boxCollider.size.x;
                float height = boxCollider.size.y;
                float length = boxCollider.size.z;
                center = boxCollider.center;

                if (boxCollider.material != null)
                {
                    mat = boxCollider.material;
                }
                // 创建Box实体
                if (isStatic)
                {
                    phyEntity = new BEPUphysics.Entities.Prefabs.Box(
                        BEPUutilities.Vector3.Zero,
                        (FixMath.NET.Fix64)width, (FixMath.NET.Fix64)height, (FixMath.NET.Fix64)length);
                }
                else
                {
                    float mass = 1f; // 默认质量
                    phyEntity = new BEPUphysics.Entities.Prefabs.Box(
                        BEPUutilities.Vector3.Zero,
                        (FixMath.NET.Fix64)width, (FixMath.NET.Fix64)height, (FixMath.NET.Fix64)length, 
                        (FixMath.NET.Fix64)mass);
                }
            }
            else
            {
                // 多个BoxCollider，创建复合实体
                List<BEPUphysics.CollisionShapes.CompoundShapeEntry> shapeEntries = new();
                
                foreach (BoxCollider boxCollider in boxColliders)
                {
                    float width = boxCollider.size.x;
                    float height = boxCollider.size.y;
                    float length = boxCollider.size.z;

                    if (boxCollider.material != null && mat == null)
                    {
                        mat = boxCollider.material;
                    }
                    
                    // 创建BoxShape
                    BEPUphysics.CollisionShapes.ConvexShapes.BoxShape boxShape = new(
                        (FixMath.NET.Fix64)width, (FixMath.NET.Fix64)height, (FixMath.NET.Fix64)length);
                    
                    // 创建CompoundShapeEntry
                    BEPUutilities.Vector3 localPosition = new(
                        (FixMath.NET.Fix64)boxCollider.center.x, 
                        (FixMath.NET.Fix64)boxCollider.center.y, 
                        (FixMath.NET.Fix64)boxCollider.center.z);
                    
                    BEPUutilities.RigidTransform localTransform = new(localPosition);
                    BEPUphysics.CollisionShapes.CompoundShapeEntry shapeEntry = new(boxShape, localTransform);
                    shapeEntries.Add(shapeEntry);
                }
                
                // 创建复合实体
                if (isStatic)
                {
                    phyEntity = new BEPUphysics.Entities.Prefabs.CompoundBody(shapeEntries);
                }
                else
                {
                    float mass = 1f; // 默认质量
                    phyEntity = new BEPUphysics.Entities.Prefabs.CompoundBody(shapeEntries, (FixMath.NET.Fix64)mass);
                }
            }
        }
        // 检查SphereCollider
        else if (obj.GetComponent<SphereCollider>() != null)
        {
            SphereCollider sphereCollider = obj.GetComponent<SphereCollider>();
            float radius = sphereCollider.radius;
            center = sphereCollider.center;

            if (sphereCollider.material != null)
            {
                mat = sphereCollider.material;
            }
            
            // 创建Sphere实体
            if (isStatic)
            {
                phyEntity = new BEPUphysics.Entities.Prefabs.Sphere(
                    BEPUutilities.Vector3.Zero, (FixMath.NET.Fix64)radius);
            }
            else
            {
                float mass = 1f; // 默认质量
                phyEntity = new BEPUphysics.Entities.Prefabs.Sphere(
                    BEPUutilities.Vector3.Zero, (FixMath.NET.Fix64)radius, (FixMath.NET.Fix64)mass);
            }
        }
        
        // 如果创建了物理实体，添加到物理世界
        if (phyEntity != null)
        {
            // 添加到物理世界
            BEPUPhyMgr.Instance.space.Add(phyEntity);
            
            // 创建物理实体数据
            PhyEntityData data = new()
            {
                PhyEntity = phyEntity,
                Center = center,
                IsStatic = isStatic
            };


            if (mat != null){
                phyEntity.Material = new BEPUphysics.Materials.Material()
                {
                    KineticFriction = (FixMath.NET.Fix64)mat.dynamicFriction,
                    StaticFriction = (FixMath.NET.Fix64)mat.staticFriction,
                    Bounciness = (FixMath.NET.Fix64)mat.bounciness,
                };
            }
            
            // 添加到映射表
            phyEntityMap.Add(obj, data);
            
            // 同步初始变换
            SyncTransformToPhysics(obj, data);
        }
    }
    
    /// <summary>
    /// 同步Unity变换到物理变换
    /// </summary>
    private void SyncUnityToPhysics()
    {
        foreach (KeyValuePair<GameObject, PhyEntityData> pair in phyEntityMap)
        {
            GameObject obj = pair.Key;
            PhyEntityData data = pair.Value;
            
            if (data.IsStatic) continue;
            
            SyncTransformToPhysics(obj, data);
        }
    }
    
    /// <summary>
    /// 同步物理变换到Unity变换
    /// </summary>
    private void SyncPhysicsToUnity()
    {
        foreach (KeyValuePair<GameObject, PhyEntityData> pair in phyEntityMap)
        {
            GameObject obj = pair.Key;
            PhyEntityData data = pair.Value;
            
            if (data.IsStatic) continue;
            
            SyncTransformToUnity(obj, data);
        }
    }
    
    /// <summary>
    /// 将Unity变换同步到物理实体
    /// </summary>
    /// <param name="obj">游戏对象</param>
    /// <param name="data">物理实体数据</param>
    private void SyncTransformToPhysics(GameObject obj, PhyEntityData data)
    {
        if (data.PhyEntity == null) return;
        
        // 同步位置
        Vector3 unityPos = obj.transform.position;
        unityPos += data.Center;
        data.PhyEntity.position = ConversionHelper.MathConverter.Convert(unityPos);
        
        // 同步旋转
        Quaternion unityRot = obj.transform.rotation;
        data.PhyEntity.orientation = ConversionHelper.MathConverter.Convert(unityRot);
    }
    
    /// <summary>
    /// 将物理变换同步到Unity变换
    /// </summary>
    /// <param name="obj">游戏对象</param>
    /// <param name="data">物理实体数据</param>
    private void SyncTransformToUnity(GameObject obj, PhyEntityData data)
    {
        if (data.PhyEntity == null) return;
        
        // 同步位置
        BEPUutilities.Vector3 phyPos = data.PhyEntity.position;
        Vector3 unityPos = ConversionHelper.MathConverter.Convert(phyPos);
        unityPos -= data.Center;
        obj.transform.position = unityPos;
        
        // 同步旋转
        BEPUutilities.Quaternion phyRot = data.PhyEntity.orientation;
        Quaternion unityRot = ConversionHelper.MathConverter.Convert(phyRot);
        obj.transform.rotation = unityRot;
    }
    
    /// <summary>
    /// 清除所有物理实体
    /// </summary>
    public void ClearAllPhyEntities()
    {
        // 从物理世界中移除所有实体
        foreach (PhyEntityData data in phyEntityMap.Values)
        {
            if (data.PhyEntity != null)
            {
                BEPUPhyMgr.Instance.space.Remove(data.PhyEntity);
            }
        }
        
        // 清除映射表
        phyEntityMap.Clear();
    }
    
    /// <summary>
    /// 为指定对象创建物理实体
    /// </summary>
    /// <param name="obj">游戏对象</param>
    public void CreatePhyEntity(GameObject obj)
    {
        // 如果已经存在，先移除
        if (phyEntityMap.ContainsKey(obj))
        {
            RemovePhyEntity(obj);
        }
        
        // 创建新的物理实体
        CreatePhyEntityForObject(obj);
    }
    
    /// <summary>
    /// 移除指定对象的物理实体
    /// </summary>
    /// <param name="obj">游戏对象</param>
    public void RemovePhyEntity(GameObject obj)
    {
        if (phyEntityMap.ContainsKey(obj))
        {
            PhyEntityData data = phyEntityMap[obj];
            if (data.PhyEntity != null)
            {
                BEPUPhyMgr.Instance.space.Remove(data.PhyEntity);
            }
            phyEntityMap.Remove(obj);
        }
    }
    
    /// <summary>
    /// 检查对象是否有物理实体
    /// </summary>
    /// <param name="obj">游戏对象</param>
    /// <returns>是否有物理实体</returns>
    public bool HasPhyEntity(GameObject obj)
    {
        return phyEntityMap.ContainsKey(obj);
    }
    
    /// <summary>
    /// 获取对象的物理实体
    /// </summary>
    /// <param name="obj">游戏对象</param>
    /// <returns>物理实体</returns>
    public BEPUphysics.Entities.Entity GetPhyEntity(GameObject obj)
    {
        if (phyEntityMap.ContainsKey(obj))
        {
            return phyEntityMap[obj].PhyEntity;
        }
        return null;
    }
}