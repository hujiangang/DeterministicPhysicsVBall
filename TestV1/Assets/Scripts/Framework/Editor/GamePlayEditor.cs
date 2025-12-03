using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

/// <summary>
/// 球杆管理器编辑器工具
/// </summary>
public class GamePlayEditor
{
    /// <summary>
    /// 创建球杆管理器的菜单项
    /// </summary>
    [MenuItem("Tools/Basic/Create EightBallGamePlay")]
    public static void CreateEightBallGamePlay()
    {
        // 查找或创建球杆管理器
        GameObject gameplay = GameObject.Find("EightBallGamePlay");
        if (gameplay == null)
        {
            // 创建新的管理器对象
            gameplay = new GameObject("EightBallGamePlay");
            GameController controller = gameplay.AddComponent<GameController>();
            controller.Init();
            
            EditorUtility.DisplayDialog("Create EightBallGamePlay", "GameController created and configured!", "OK");
        }
        else
        {
            EditorUtility.DisplayDialog("Create EightBallGamePlay", "GameController already exists!", "OK");
        }
    }
}