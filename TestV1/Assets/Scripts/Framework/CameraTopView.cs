using UnityEngine;

/// <summary>
/// 设置摄像机为顶视角的脚本
/// </summary>
public class CameraTopView : MonoBehaviour
{
    /// <summary>
    /// 桌子预制体标签
    /// </summary>
    public string tableTag = "Table";
    
    /// <summary>
    /// 摄像机高度
    /// </summary>
    public float cameraHeight = 10f;
    
    /// <summary>
    /// 摄像机距离桌子中心的水平距离
    /// </summary>
    public float cameraDistance = 15f;
    
    private void Start()
    {
        // 查找桌子对象
        GameObject table = GameObject.FindWithTag(tableTag);
        Vector3 tableCenter = Vector3.zero;
        
        // 如果找到桌子，使用桌子的中心
        if (table != null)
        {
            tableCenter = table.transform.position;
            Debug.Log("Found table at position: " + tableCenter);
        }
        else
        {
            Debug.LogWarning("Table not found with tag: " + tableTag + ", using default position (0,0,0)");
        }
        
        // 设置摄像机位置和旋转
        SetTopView(tableCenter);
    }
    
    /// <summary>
    /// 设置摄像机为顶视角
    /// </summary>
    /// <param name="targetCenter">目标中心位置</param>
    private void SetTopView(Vector3 targetCenter)
    {
        // 摄像机位置：从目标中心上方俯视
        
        // 摄像机旋转：向下俯视，保持水平
        transform.SetPositionAndRotation(new Vector3(targetCenter.x, cameraHeight, targetCenter.z - cameraDistance), Quaternion.Euler(90f, 0f, 0f));
        Debug.Log("Camera set to top view: position=" + transform.position + ", rotation=" + transform.rotation);
    }
    
    /// <summary>
    /// 在编辑器中实时更新摄像机位置
    /// </summary>
    private void OnValidate()
    {
        // 只在编辑器中运行
        if (!Application.isPlaying)
        {
            GameObject table = GameObject.FindWithTag(tableTag);
            Vector3 tableCenter = table != null ? table.transform.position : Vector3.zero;
            SetTopView(tableCenter);
        }
    }
}