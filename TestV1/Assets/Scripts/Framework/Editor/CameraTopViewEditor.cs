using UnityEngine;
using UnityEditor;

/// <summary>
/// 摄像机顶视角设置编辑器工具
/// </summary>
public class CameraTopViewEditor
{
    /// <summary>
    /// 桌子标签
    /// </summary>
    private const string TABLE_TAG = "Table";
    
    /// <summary>
    /// 默认摄像机高度
    /// </summary>
    private const float DEFAULT_CAMERA_HEIGHT = 10f;
    
    /// <summary>
    /// 默认摄像机距离
    /// </summary>
    private const float DEFAULT_CAMERA_DISTANCE = 15f;
    
    /// <summary>
    /// 设置主摄像机为顶视角的菜单项
    /// </summary>
    [MenuItem("Tools/BEPU Physics/Camera/Set Main Camera to Top View")]
    public static void SetMainCameraToTopView()
    {
        // 获取主摄像机
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogWarning("Main camera not found. Creating a new camera.");
            
            // 创建新摄像机
            GameObject cameraObj = new GameObject("Main Camera");
            mainCamera = cameraObj.AddComponent<Camera>();
            mainCamera.tag = "MainCamera";
        }
        
        // 查找桌子对象
        GameObject table = GameObject.FindWithTag(TABLE_TAG);
        Vector3 tableCenter = Vector3.zero;
        
        // 如果找到桌子，使用桌子的中心
        if (table != null)
        {
            tableCenter = table.transform.position;
            Debug.Log("Found table at position: " + tableCenter);
        }
        else
        {
            Debug.LogWarning("Table not found with tag: " + TABLE_TAG + ", using default position (0,0,0)");
            
            // 尝试查找 BasicTable 预制体实例
            GameObject basicTable = GameObject.Find("BasicTable");
            if (basicTable != null)
            {
                tableCenter = basicTable.transform.position;
                Debug.Log("Found BasicTable at position: " + tableCenter);
            }
        }
        
        // 设置摄像机位置和旋转
        SetCameraToTopView(mainCamera, tableCenter);
        
        Debug.Log("Main camera set to top view successfully!");
        EditorUtility.DisplayDialog("Set Camera to Top View", "Main camera has been set to top view.", "OK");
    }
    
    /// <summary>
    /// 设置指定摄像机为顶视角
    /// </summary>
    /// <param name="camera">要设置的摄像机</param>
    /// <param name="targetCenter">目标中心位置</param>
    private static void SetCameraToTopView(Camera camera, Vector3 targetCenter)
    {
        if (camera == null)
            return;
        
        // 摄像机位置：从目标中心正上方俯视
        camera.transform.position = new Vector3(targetCenter.x, DEFAULT_CAMERA_HEIGHT, targetCenter.z);
        
        // 摄像机旋转：向下俯视，保持水平
        camera.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        
        // 设置摄像机视野，确保能看到整个桌子
        camera.fieldOfView = 60f;
        
        // 确保摄像机清除标志正确
        camera.clearFlags = CameraClearFlags.Skybox;
        
        Debug.Log("Camera set to top view: position=" + camera.transform.position + ", rotation=" + camera.transform.rotation);
    }
    
    /// <summary>
    /// 调整摄像机位置以更好地看到桌子的菜单项
    /// </summary>
    [MenuItem("Tools/BEPU Physics/Camera/Adjust Camera to See Table")]
    public static void AdjustCameraToSeeTable()
    {
        // 获取主摄像机
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogWarning("Main camera not found.");
            return;
        }
        
        // 查找桌子对象
        GameObject table = GameObject.FindWithTag(TABLE_TAG);
        if (table == null)
        {
            table = GameObject.Find("BasicTable");
        }
        
        if (table == null)
        {
            Debug.LogWarning("Table not found, cannot adjust camera.");
            return;
        }
        
        // 获取桌子的渲染器，计算桌子的大小
        Renderer tableRenderer = table.GetComponentInChildren<Renderer>();
        if (tableRenderer == null)
        {
            Debug.LogWarning("Table renderer not found, using default camera settings.");
            SetCameraToTopView(mainCamera, table.transform.position);
            return;
        }
        
        // 计算桌子的边界
        Bounds tableBounds = tableRenderer.bounds;
        float tableSize = Mathf.Max(tableBounds.size.x, tableBounds.size.z);
        
        // 根据桌子大小调整摄像机位置
        float cameraHeight = tableSize * 1.5f;
        float cameraDistance = tableSize * 0.5f;
        
        // 设置摄像机位置
        mainCamera.transform.position = new Vector3(
            tableBounds.center.x, 
            cameraHeight, 
            tableBounds.center.z
        );
        
        // 设置摄像机旋转
        mainCamera.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        
        // 调整摄像机视野，确保能看到整个桌子
        float requiredFOV = 2 * Mathf.Atan2(tableSize * 0.5f, cameraHeight) * Mathf.Rad2Deg;
        mainCamera.fieldOfView = Mathf.Clamp(requiredFOV, 30f, 90f);
        
        Debug.Log(string.Format("Camera adjusted to see table: size={0}, height={1}, fov={2}", 
            tableSize, cameraHeight, mainCamera.fieldOfView));
        
        EditorUtility.DisplayDialog("Adjust Camera to See Table", "Camera has been adjusted to see the table.", "OK");
    }
}