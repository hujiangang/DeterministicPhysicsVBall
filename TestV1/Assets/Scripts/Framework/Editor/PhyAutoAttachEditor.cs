using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// 自动挂载物理脚本的编辑器工具
/// </summary>
public class PhyAutoAttachEditor
{
    /// <summary>
    /// 自动挂载物理脚本的菜单项
    /// </summary>
    [MenuItem("Tools/BEPU Physics/Physics Entity/Auto Attach Physics Scripts")]
    public static void AutoAttachPhysicsScripts()
    {
        // 获取场景中所有游戏对象
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        
        // 统计信息
        int boxCount = 0;
        int compoundBoxCount = 0;
        int sphereCount = 0;
        int fixedCount = 0;
        
        // 遍历所有对象
        foreach (GameObject obj in allObjects)
        {
            // 检查碰撞体类型
            BoxCollider[] boxColliders = obj.GetComponents<BoxCollider>();
            SphereCollider sphereCollider = obj.GetComponent<SphereCollider>();
            
            // 确定应该挂载的脚本类型
            System.Type expectedScriptType = null;
            if (boxColliders.Length > 0)
            {
                expectedScriptType = boxColliders.Length == 1 ? typeof(PhyBoxEntity) : typeof(PhyCompoundBoxEntity);
            }
            else if (sphereCollider != null)
            {
                expectedScriptType = typeof(PhySphereEntity);
            }
            
            // 如果没有碰撞体,跳过
            if (expectedScriptType == null)
                continue;
            
            // 获取当前挂载的物理脚本
            PhyBaseEntity currentScript = obj.GetComponent<PhyBaseEntity>();
            
            // 如果当前脚本存在且类型正确
            if (currentScript != null && currentScript.GetType() == expectedScriptType)
            {
                // 设置 IsStatic 为 true
                SetIsStaticToTrue(currentScript);
                continue;
            }
            
            // 如果当前脚本存在但类型错误，移除它
            if (currentScript != null)
            {
                Object.DestroyImmediate(currentScript);
                fixedCount++;
            }
            
            // 挂载正确的脚本
            PhyBaseEntity newScript = obj.AddComponent(expectedScriptType) as PhyBaseEntity;
            
            // 设置 IsStatic 为 true
            SetIsStaticToTrue(newScript);
            
            // 更新统计信息
            if (expectedScriptType == typeof(PhyBoxEntity))
            {
                boxCount++;
            }
            else if (expectedScriptType == typeof(PhyCompoundBoxEntity))
            {
                compoundBoxCount++;
            }
            else if (expectedScriptType == typeof(PhySphereEntity))
            {
                sphereCount++;
            }
        }
        
        // 显示结果
        string message = string.Format("Auto attach physics scripts completed!\n" +
                                       "- Single BoxCollider: {0} objects\n" +
                                       "- Multiple BoxCollider: {1} objects\n" +
                                       "- SphereCollider: {2} objects\n" +
                                       "- Fixed wrong scripts: {3} objects\n" +
                                       "Total: {4} objects processed",
                                       boxCount, compoundBoxCount, sphereCount, fixedCount,
                                       boxCount + compoundBoxCount + sphereCount);
        
        Debug.Log(message);
        EditorUtility.DisplayDialog("Auto Attach Physics Scripts", message, "OK");
    }
    
    /// <summary>
    /// 设置物理脚本的 IsStatic 属性为 true
    /// </summary>
    /// <param name="script">物理脚本</param>
    private static void SetIsStaticToTrue(PhyBaseEntity script)
    {
        if (script != null)
        {
            // 使用反射设置 IsStatic 属性
            System.Reflection.FieldInfo isStaticField = typeof(PhyBaseEntity).GetField("isStatic", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (isStaticField != null)
            {
                isStaticField.SetValue(script, true);
            }
        }
    }
    
    /// <summary>
    /// 清理所有物理脚本的菜单项
    /// </summary>
    [MenuItem("Tools/BEPU Physics/Physics Entity/Clear All Physics Scripts")]
    public static void ClearAllPhysicsScripts()
    {
        // 获取场景中所有游戏对象
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        
        // 统计信息
        int removedCount = 0;
        
        // 遍历所有对象
        foreach (GameObject obj in allObjects)
        {
            // 查找并移除所有物理脚本
            PhyBaseEntity[] phyEntities = obj.GetComponents<PhyBaseEntity>();
            foreach (PhyBaseEntity phyEntity in phyEntities)
            {
                Object.DestroyImmediate(phyEntity);
                removedCount++;
            }
        }
        
        // 显示结果
        string message = string.Format("Clear all physics scripts completed!\n" +
                                       "- Removed: {0} scripts",
                                       removedCount);
        
        Debug.Log(message);
        EditorUtility.DisplayDialog("Clear All Physics Scripts", message, "OK");
    }
}