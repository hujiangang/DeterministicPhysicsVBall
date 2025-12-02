using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

/// <summary>
/// 物理实体管理器编辑器工具
/// </summary>
public class PhyEntityManagerEditor
{
    /// <summary>
    /// 扫描场景物理对象的菜单项
    /// </summary>
    [MenuItem("Tools/BEPU Physics/Physics Entity/Scan Scene for Physics Objects")]
    public static void ScanSceneForPhysicsObjects()
    {
        // 确保BEPUPhyMgr存在
        EnsureBEPUPhyMgrExists();
        
        // 确保PhyEntityManager存在
        EnsurePhyEntityManagerExists();
        
        // 扫描场景
        PhyEntityManager.Instance.ScanSceneForPhysicsObjects();
        
        EditorUtility.DisplayDialog("Scan Physics Objects", "Scene scan completed!", "OK");
    }
    
    /// <summary>
    /// 清除所有物理实体的菜单项
    /// </summary>
    [MenuItem("Tools/BEPU Physics/Physics Entity/Clear All Physics Entities")]
    public static void ClearAllPhysicsEntities()
    {
        if (PhyEntityManager.Instance != null)
        {
            PhyEntityManager.Instance.ClearAllPhyEntities();
            EditorUtility.DisplayDialog("Clear Physics Entities", "All physics entities cleared!", "OK");
        }
        else
        {
            EditorUtility.DisplayDialog("Clear Physics Entities", "PhyEntityManager not found!", "OK");
        }
    }
    
    /// <summary>
    /// 切换物理管理器的菜单项
    /// </summary>
    [MenuItem("Tools/BEPU Physics/Physics Entity/Toggle Physics Manager")]
    public static void TogglePhysicsManager()
    {
        // 查找或创建PhyEntityManager
        GameObject managerObj = GameObject.Find("PhyEntityManager");
        if (managerObj == null)
        {
            // 创建新的管理器对象
            managerObj = new GameObject("PhyEntityManager");
            managerObj.AddComponent<PhyEntityManager>();
            EditorUtility.DisplayDialog("Toggle Physics Manager", "PhyEntityManager created!", "OK");
        }
        else
        {
            // 切换激活状态
            bool isActive = managerObj.activeSelf;
            managerObj.SetActive(!isActive);
            EditorUtility.DisplayDialog("Toggle Physics Manager", 
                "PhyEntityManager " + (!isActive ? "activated" : "deactivated") + "!", "OK");
        }
    }
    
    /// <summary>
    /// 确保BEPUPhyMgr存在
    /// </summary>
    private static void EnsureBEPUPhyMgrExists()
    {
        if (BEPUPhyMgr.Instance == null)
        {
            GameObject managerObj = new GameObject("BEPUPhyMgr");
            BEPUPhyMgr manager = managerObj.AddComponent<BEPUPhyMgr>();
            // 手动调用Initialize方法，确保Instance属性被正确设置
            manager.Initialize();
            Debug.Log("BEPUPhyMgr created!");
        }
    }
    
    /// <summary>
    /// 确保PhyEntityManager存在
    /// </summary>
    private static void EnsurePhyEntityManagerExists()
    {
        if (PhyEntityManager.Instance == null)
        {
            GameObject managerObj = new GameObject("PhyEntityManager");
            PhyEntityManager manager = managerObj.AddComponent<PhyEntityManager>();
            // 手动调用Initialize方法，确保Instance属性被正确设置
            manager.Initialize();
            Debug.Log("PhyEntityManager created!");
        }
    }
    
    /// <summary>
    /// 为选中对象创建物理实体的菜单项
    /// </summary>
    [MenuItem("GameObject/BEPU Physics/Create Physics Entity", false, 10)]
    public static void CreatePhysicsEntityForSelected()
    {
        // 确保PhyEntityManager存在
        EnsurePhyEntityManagerExists();
        
        // 处理选中的对象
        foreach (GameObject obj in Selection.gameObjects)
        {
            PhyEntityManager.Instance.CreatePhyEntity(obj);
        }
        
        EditorUtility.DisplayDialog("Create Physics Entity", 
            "Physics entities created for selected objects!", "OK");
    }
    
    /// <summary>
    /// 为选中对象移除物理实体的菜单项
    /// </summary>
    [MenuItem("GameObject/BEPU Physics/Remove Physics Entity", false, 11)]
    public static void RemovePhysicsEntityForSelected()
    {
        if (PhyEntityManager.Instance != null)
        {
            // 处理选中的对象
            foreach (GameObject obj in Selection.gameObjects)
            {
                PhyEntityManager.Instance.RemovePhyEntity(obj);
            }
            
            EditorUtility.DisplayDialog("Remove Physics Entity", 
                "Physics entities removed for selected objects!", "OK");
        }
        else
        {
            EditorUtility.DisplayDialog("Remove Physics Entity", 
                "PhyEntityManager not found!", "OK");
        }
    }
    
    /// <summary>
    /// 验证菜单项是否可用
    /// </summary>
    [MenuItem("GameObject/BEPU Physics/Create Physics Entity", true)]
    [MenuItem("GameObject/BEPU Physics/Remove Physics Entity", true)]
    public static bool ValidatePhysicsEntityMenu()
    {
        // 只有选中了对象时菜单项才可用
        return Selection.gameObjects.Length > 0;
    }
}