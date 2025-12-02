using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

/// <summary>
/// 球杆管理器编辑器工具
/// </summary>
public class CuestickManagerEditor
{
    /// <summary>
    /// 创建球杆管理器的菜单项
    /// </summary>
    [MenuItem("Tools/BEPU Physics/Cuestick/Create Cuestick Manager")]
    public static void CreateCuestickManager()
    {
        // 查找或创建球杆管理器
        GameObject managerObj = GameObject.Find("CuestickManager");
        if (managerObj == null)
        {
            // 创建新的管理器对象
            managerObj = new GameObject("CuestickManager");
            CuestickManager manager = managerObj.AddComponent<CuestickManager>();
            
            // 尝试自动查找并设置球杆和白球
            AutoSetCuestickAndWhiteBall(manager);
            
            EditorUtility.DisplayDialog("Create Cuestick Manager", "CuestickManager created and configured!", "OK");
        }
        else
        {
            EditorUtility.DisplayDialog("Create Cuestick Manager", "CuestickManager already exists!", "OK");
        }
    }
    
    /// <summary>
    /// 切换球杆管理器的菜单项
    /// </summary>
    [MenuItem("Tools/BEPU Physics/Cuestick/Toggle Cuestick Manager")]
    public static void ToggleCuestickManager()
    {
        GameObject managerObj = GameObject.Find("CuestickManager");
        if (managerObj != null)
        {
            bool isActive = managerObj.activeSelf;
            managerObj.SetActive(!isActive);
            EditorUtility.DisplayDialog("Toggle Cuestick Manager", 
                "CuestickManager " + (!isActive ? "activated" : "deactivated") + "!", "OK");
        }
        else
        {
            EditorUtility.DisplayDialog("Toggle Cuestick Manager", "CuestickManager not found!", "OK");
        }
    }
    
    /// <summary>
    /// 自动设置球杆和白球
    /// </summary>
    /// <param name="manager">球杆管理器</param>
    private static void AutoSetCuestickAndWhiteBall(CuestickManager manager)
    {
        if (manager == null)
            return;
        
        // 查找球杆
        GameObject cuestick = GameObject.FindWithTag("Cuestick");
        if (cuestick == null)
        {
            cuestick = GameObject.Find("Cuestick");
        }
        
        // 查找白球
        GameObject whiteBall = GameObject.FindWithTag("WhiteBall");
        if (whiteBall == null)
        {
            whiteBall = GameObject.Find("WhiteBall");
        }
        
        // 查找瞄准线材质
        Material aimLineMaterial = FindAimLineMaterial();
        
        // 使用反射设置属性
        SetPrivateField(manager, "cuestick", cuestick);
        SetPrivateField(manager, "whiteBall", whiteBall);
        SetPrivateField(manager, "aimLineMaterial", aimLineMaterial);
    }
    
    /// <summary>
    /// 查找瞄准线材质
    /// </summary>
    /// <returns>瞄准线材质</returns>
    private static Material FindAimLineMaterial()
    {
        // 在项目中查找瞄准线材质
        string[] materialPaths = AssetDatabase.FindAssets("AimLine t:Material");
        if (materialPaths.Length > 0)
        {
            string materialPath = AssetDatabase.GUIDToAssetPath(materialPaths[0]);
            return AssetDatabase.LoadAssetAtPath<Material>(materialPath);
        }
        
        // 如果没有找到，创建一个简单的红色材质
        Debug.LogWarning("AimLine material not found, creating a simple red material.");
        Material material = new Material(Shader.Find("Unlit/Color"));
        material.color = Color.red;
        return material;
    }
    
    /// <summary>
    /// 设置私有字段
    /// </summary>
    /// <param name="obj">对象</param>
    /// <param name="fieldName">字段名</param>
    /// <param name="value">值</param>
    private static void SetPrivateField(Object obj, string fieldName, Object value)
    {
        System.Reflection.FieldInfo field = obj.GetType().GetField(fieldName, 
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
        if (field != null)
        {
            field.SetValue(obj, value);
        }
    }
    
    /// <summary>
    /// 为球杆添加标签的菜单项
    /// </summary>
    [MenuItem("GameObject/BEPU Physics/Set as Cuestick", false, 12)]
    public static void SetAsCuestick()
    {
        SetObjectTag(Selection.gameObjects, "Cuestick");
    }
    
    /// <summary>
    /// 为白球添加标签的菜单项
    /// </summary>
    [MenuItem("GameObject/BEPU Physics/Set as White Ball", false, 13)]
    public static void SetAsWhiteBall()
    {
        SetObjectTag(Selection.gameObjects, "WhiteBall");
    }
    
    /// <summary>
    /// 设置对象标签
    /// </summary>
    /// <param name="objects">对象数组</param>
    /// <param name="tag">标签</param>
    private static void SetObjectTag(GameObject[] objects, string tag)
    {
        foreach (GameObject obj in objects)
        {
            obj.tag = tag;
        }
        EditorUtility.DisplayDialog("Set Object Tag", "Tag set to: " + tag, "OK");
    }
    
    /// <summary>
    /// 验证菜单项是否可用
    /// </summary>
    [MenuItem("GameObject/BEPU Physics/Set as Cuestick", true)]
    [MenuItem("GameObject/BEPU Physics/Set as White Ball", true)]
    public static bool ValidateSetObjectTag()
    {
        return Selection.gameObjects.Length > 0;
    }
}