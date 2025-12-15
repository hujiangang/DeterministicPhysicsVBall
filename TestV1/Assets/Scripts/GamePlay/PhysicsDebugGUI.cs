using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 运行时物理参数调节GUI
/// 在游戏运行过程中调节物理参数
/// 支持多个面板切换，每个面板包含完整的物理参数
/// 底部有应用按钮
/// </summary>
public class PhysicsDebugGUI : MonoBehaviour
{
    // GUI开关
    private bool _showGUI = false;
    
    // 当前选中的面板索引
    private int _currentPanel = 0;
    
    // 面板数量
    private int _panelCount = 3;
    
    // 面板标题
    private readonly string[] _panelTitles = { "球物理参数", "桌子物理参数", "库边物理参数" };

    public Dictionary<GameLayers, int> _physicsParamsDict = new(){
        {GameLayers.Balls, 0},
        {GameLayers.Table, 1},
        {GameLayers.Rails, 2},
    };
    
    // 物理参数数组，每个面板对应一组参数
    private PhysicsParams[] _physicsParams;
    
    // GUI位置和大小 - 大幅增大窗口尺寸以适应超大字体
    private Rect _windowRect = new(20, 20, 550, 700);
    
    // 滚动位置
    private Vector2 _scrollPosition = Vector2.zero;
    
    private void Awake()
    {
        // 初始化物理参数数组
        _physicsParams = new PhysicsParams[_panelCount];
        
        // 给每个面板设置默认参数
        _physicsParams[0] = new PhysicsParams(0.5f, 0.3f, 0.8f, 0.05f, 0.1f);   // 面板1：默认设置
        _physicsParams[1] = new PhysicsParams(0.7f, 0.4f, 0.6f, 0.1f, 0.15f);    // 面板2：进阶设置
        _physicsParams[2] = new PhysicsParams(0.3f, 0.2f, 0.9f, 0.02f, 0.05f);   // 面板3：高级设置
    }
    
    /// <summary>
    /// 初始化物理参数，从物理实体管理器获取当前参数并更新面板.
    /// </summary>
    private void InitializePhysicsParams()
    {
        foreach (var item in _physicsParamsDict)
        {
            if (!PhyEntityManager.Instance.GetPhysicsParams(item.Key, out PhysicsParams physicsParams)) continue;
            
            // 更新面板参数
            _physicsParams[item.Value] = physicsParams;

            Debug.Log($"层: {item.Key}");
            Debug.Log($"静态摩擦力: {physicsParams.staticFriction:F2}");
            Debug.Log($"动摩擦力: {physicsParams.dynamicFriction:F2}");
            Debug.Log($"弹性系数: {physicsParams.restitution:F2}");
            Debug.Log($"线速度衰减: {physicsParams.linearDamping:F2}");
            Debug.Log($"角速度衰减: {physicsParams.angularDamping:F2}");
        }
    }


    private void Update()
    {
        // 按G键开关GUI
        if (Input.GetKeyDown(KeyCode.G))
        {
            _showGUI = !_showGUI;
            if (_showGUI)
            {
                InitializePhysicsParams();
            }
        }
    }
    
    private void OnGUI()
    {
        if (!_showGUI)
            return;
        
        // 设置GUI样式 - 大幅增大字体，确保清晰可见
        GUI.skin.window.fontSize = 24;     // 窗口标题字体大小
        GUI.skin.label.fontSize = 22;       // 标签字体大小
        GUI.skin.button.fontSize = 20;      // 按钮字体大小
        GUI.skin.toggle.fontSize = 20;      // 开关字体大小
        GUI.skin.textField.fontSize = 22;    // 输入框字体大小
        
        // 设置字体样式，确保行高足够
        GUI.skin.label.fontStyle = FontStyle.Bold;
        GUI.skin.button.fontStyle = FontStyle.Bold;
        
        // 保存原始文本样式，避免影响其他GUI元素
        GUIStyle originalTextFieldStyle = new GUIStyle(GUI.skin.textField);
        
        // 设置输入框样式 - 优化交互体验
        GUI.skin.textField = new GUIStyle(originalTextFieldStyle)
        {
            alignment = TextAnchor.MiddleCenter,    // 文本居中
            fixedHeight = 50,                      // 合适的高度
            fontStyle = FontStyle.Bold,            // 加粗字体
            padding = new RectOffset(10, 10, 10, 10), // 适当内边距
            margin = new RectOffset(5, 5, 5, 5),    // 适当外边距
            // 保持默认背景和边框，确保交互正常
            normal = originalTextFieldStyle.normal,
            active = originalTextFieldStyle.active,
            focused = originalTextFieldStyle.focused,
            hover = originalTextFieldStyle.hover,
            onNormal = originalTextFieldStyle.onNormal,
            onActive = originalTextFieldStyle.onActive,
            onFocused = originalTextFieldStyle.onFocused,
            onHover = originalTextFieldStyle.onHover
        };
        
        // 绘制主窗口 - 大幅增大窗口尺寸以适应大字体
        _windowRect = GUI.Window(0, _windowRect, DrawWindow, "物理参数调节", GUI.skin.window);
    }
    
    /// <summary>
    /// 绘制窗口内容 - 改用GUILayout自动处理布局，确保文字完整显示
    /// </summary>
    private void DrawWindow(int windowID)
    {
        // 允许窗口拖动
        GUI.DragWindow(new Rect(0, 0, _windowRect.width, 20));
        
        // 使用垂直布局开始
        GUILayout.BeginVertical();
        
        // 绘制面板切换按钮 - 改用GUILayout
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        for (int i = 0; i < _panelCount; i++)
        {
            // 自动宽度的面板按钮
            if (GUILayout.Button(_panelTitles[i], GUILayout.Height(60), GUILayout.ExpandWidth(true)))
            {
                _currentPanel = i;
            }
        }
        GUILayout.EndHorizontal();
        
        // 绘制面板标题
        //GUILayout.Space(20);
        //GUILayout.Label(_panelTitles[_currentPanel], GUILayout.Height(40));
        //GUILayout.Box(GUIContent.none, GUILayout.ExpandWidth(true), GUILayout.Height(3)); // 分割线
        //GUILayout.Space(30);
        
        // 开始滚动视图
        //_scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.Height(400));
        
        // 绘制当前面板的参数 - 改用GUILayout
        DrawCurrentPanelParamsWithGUILayout();
        
        // 结束滚动视图
        //GUILayout.EndScrollView();
        
        // 绘制应用按钮 - 改用GUILayout
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("应用参数", GUILayout.Width(200), GUILayout.Height(60)))
        {
            ApplyParameters();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        
        // 绘制GUI开关提示
        GUILayout.Space(20);
        GUILayout.Label("按G键开关GUI | 按左右箭头切换面板", GUILayout.Height(40));
        
        // 结束垂直布局
        GUILayout.EndVertical();
    }
    
    /// <summary>
    /// 绘制当前面板的物理参数 - 使用GUILayout自动布局，确保文字完整显示
    /// </summary>
    private void DrawCurrentPanelParamsWithGUILayout()
    {
        // 获取当前面板的参数
        ref PhysicsParams currentParams = ref _physicsParams[_currentPanel];
        
        // 使用垂直布局
        GUILayout.BeginVertical();
        
        // 设置滑块样式
        GUIStyle sliderStyle = new GUIStyle(GUI.skin.horizontalSlider)
        {
            fixedHeight = 30 // 增大滑块高度
        };
        
        GUIStyle sliderThumbStyle = new GUIStyle(GUI.skin.horizontalSliderThumb)
        {
            fixedHeight = 30, // 增大滑块拇指高度
            fixedWidth = 30   // 增大滑块拇指宽度
        };
        
        // 保存原始样式
        GUIStyle originalSliderStyle = GUI.skin.horizontalSlider;
        GUIStyle originalSliderThumbStyle = GUI.skin.horizontalSliderThumb;
        
        // 应用新样式
        GUI.skin.horizontalSlider = sliderStyle;
        GUI.skin.horizontalSliderThumb = sliderThumbStyle;
        
        // 静态摩擦力 - 使用水平布局，添加可输入文本框
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.Label("静态摩擦力:", GUILayout.Width(150), GUILayout.Height(50));
        
        // 滑块控制
        float tempStaticFriction = GUILayout.HorizontalSlider(currentParams.staticFriction, 0f, 1f, GUILayout.Height(30));
        if (Mathf.Abs(tempStaticFriction - currentParams.staticFriction) > 0.001f)
        {
            currentParams.staticFriction = tempStaticFriction;
        }
        
        // 可输入文本框 - 支持精确输入
        string staticFrictionStr = GUILayout.TextField(currentParams.staticFriction.ToString("F3"), GUILayout.Width(100), GUILayout.Height(50));
        if (float.TryParse(staticFrictionStr, out float staticFrictionValue))
        {
            // 限制数值范围 0-1
            staticFrictionValue = Mathf.Clamp(staticFrictionValue, 0f, 1f);
            if (Mathf.Abs(staticFrictionValue - currentParams.staticFriction) > 0.001f)
            {
                currentParams.staticFriction = staticFrictionValue;
            }
        }
        GUILayout.EndHorizontal();
        
        // 动摩擦力
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Label("动摩擦力:", GUILayout.Width(150), GUILayout.Height(50));
        
        // 滑块控制
        float tempDynamicFriction = GUILayout.HorizontalSlider(currentParams.dynamicFriction, 0f, 1f, GUILayout.Height(30));
        if (Mathf.Abs(tempDynamicFriction - currentParams.dynamicFriction) > 0.001f)
        {
            currentParams.dynamicFriction = tempDynamicFriction;
        }
        
        // 可输入文本框
        string dynamicFrictionStr = GUILayout.TextField(currentParams.dynamicFriction.ToString("F3"), GUILayout.Width(100), GUILayout.Height(50));
        if (float.TryParse(dynamicFrictionStr, out float dynamicFrictionValue))
        {
            dynamicFrictionValue = Mathf.Clamp(dynamicFrictionValue, 0f, 1f);
            if (Mathf.Abs(dynamicFrictionValue - currentParams.dynamicFriction) > 0.001f)
            {
                currentParams.dynamicFriction = dynamicFrictionValue;
            }
        }
        GUILayout.EndHorizontal();
        
        // 弹性系数
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Label("弹性系数:", GUILayout.Width(150), GUILayout.Height(50));
        
        // 滑块控制
        float tempRestitution = GUILayout.HorizontalSlider(currentParams.restitution, 0f, 1f, GUILayout.Height(30));
        if (Mathf.Abs(tempRestitution - currentParams.restitution) > 0.001f)
        {
            currentParams.restitution = tempRestitution;
        }
        
        // 可输入文本框
        string restitutionStr = GUILayout.TextField(currentParams.restitution.ToString("F3"), GUILayout.Width(100), GUILayout.Height(50));
        if (float.TryParse(restitutionStr, out float restitutionValue))
        {
            restitutionValue = Mathf.Clamp(restitutionValue, 0f, 1f);
            if (Mathf.Abs(restitutionValue - currentParams.restitution) > 0.001f)
            {
                currentParams.restitution = restitutionValue;
            }
        }
        GUILayout.EndHorizontal();
        
        // 线速度衰减系数
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Label("线速度衰减:", GUILayout.Width(150), GUILayout.Height(50));
        
        // 滑块控制
        float tempLinearDamping = GUILayout.HorizontalSlider(currentParams.linearDamping, 0f, 1f, GUILayout.Height(30));
        if (Mathf.Abs(tempLinearDamping - currentParams.linearDamping) > 0.001f)
        {
            currentParams.linearDamping = tempLinearDamping;
        }
        
        // 可输入文本框
        string linearDampingStr = GUILayout.TextField(currentParams.linearDamping.ToString("F3"), GUILayout.Width(100), GUILayout.Height(50));
        if (float.TryParse(linearDampingStr, out float linearDampingValue))
        {
            linearDampingValue = Mathf.Clamp(linearDampingValue, 0f, 1f);
            if (Mathf.Abs(linearDampingValue - currentParams.linearDamping) > 0.001f)
            {
                currentParams.linearDamping = linearDampingValue;
            }
        }
        GUILayout.EndHorizontal();
        
        // 角速度衰减系数
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Label("角速度衰减:", GUILayout.Width(150), GUILayout.Height(50));
        
        // 滑块控制
        float tempAngularDamping = GUILayout.HorizontalSlider(currentParams.angularDamping, 0f, 1f, GUILayout.Height(30));
        if (Mathf.Abs(tempAngularDamping - currentParams.angularDamping) > 0.001f)
        {
            currentParams.angularDamping = tempAngularDamping;
        }
        
        // 可输入文本框
        string angularDampingStr = GUILayout.TextField(currentParams.angularDamping.ToString("F3"), GUILayout.Width(100), GUILayout.Height(50));
        if (float.TryParse(angularDampingStr, out float angularDampingValue))
        {
            angularDampingValue = Mathf.Clamp(angularDampingValue, 0f, 1f);
            if (Mathf.Abs(angularDampingValue - currentParams.angularDamping) > 0.001f)
            {
                currentParams.angularDamping = angularDampingValue;
            }
        }
        GUILayout.EndHorizontal();
        
        // 恢复原始样式
        GUI.skin.horizontalSlider = originalSliderStyle;
        GUI.skin.horizontalSliderThumb = originalSliderThumbStyle;
        
        GUILayout.Label($"当前面板: {_currentPanel + 1}/{_panelCount}", GUILayout.Height(50));
        
        // 结束垂直布局
        GUILayout.Space(50);
        GUILayout.EndVertical();
    }

    
    /// <summary>
    /// 应用当前面板的物理参数
    /// 实际项目中应将参数应用到真实的物理配置
    /// </summary>
    private void ApplyParameters()
    {
        // 获取当前面板的参数
        PhysicsParams currentParams = _physicsParams[_currentPanel];
        
        // 示例：打印参数到控制台
        Debug.Log($"应用面板 {_currentPanel + 1} 的物理参数:");
        Debug.Log($"静态摩擦力: {currentParams.staticFriction:F2}");
        Debug.Log($"动摩擦力: {currentParams.dynamicFriction:F2}");
        Debug.Log($"弹性系数: {currentParams.restitution:F2}");
        Debug.Log($"线速度衰减: {currentParams.linearDamping:F2}");
        Debug.Log($"角速度衰减: {currentParams.angularDamping:F2}");
        Debug.Log("参数应用成功！");


        foreach (var (layer, index) in _physicsParamsDict){
            if (index == _currentPanel){
                PhyEntityManager.Instance.SetPhysicsParams(layer, currentParams);
                break;
            }
        }
        
        // 在实际项目中，这里应该将参数应用到真实的物理系统
        // 例如：
        // PhysicsMaterial mat = GetTargetPhysicsMaterial();
        // mat.staticFriction = currentParams.staticFriction;
        // mat.dynamicFriction = currentParams.dynamicFriction;
        // mat.bounciness = currentParams.restitution;
        // 
        // Rigidbody[] rigidbodies = FindObjectsOfType<Rigidbody>();
        // foreach (Rigidbody rb in rigidbodies)
        // {
        //     rb.drag = currentParams.linearDamping;
        //     rb.angularDrag = currentParams.angularDamping;
        // }
    }
    
    /// <summary>
    /// 切换GUI显示状态
    /// </summary>
    public void ToggleGUI()
    {
        _showGUI = !_showGUI;
    }
    
    /// <summary>
    /// 设置当前面板
    /// </summary>
    public void SetCurrentPanel(int panelIndex)
    {
        _currentPanel = Mathf.Clamp(panelIndex, 0, _panelCount - 1);
    }
}
