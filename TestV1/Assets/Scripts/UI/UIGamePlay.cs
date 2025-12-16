using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 引入Test命名空间，使用CueHitType枚举
using Test;
using TMPro;

/// <summary>
/// 负责动态生成杆法按钮和处理点击事件
/// </summary>
public class UIGamePlay : MonoBehaviour
{
    // 9种基本杆法（与CueHitType枚举完全对应，索引=枚举值）
    // 重新排列顺序，确保在3x3网格中显示为：
    // 第1行：高左(5) | 高杆(1) | 高右(6)
    // 第2行：左杆(3) | 中杆(0) | 右杆(4)
    // 第3行：低左(7) | 低杆(2) | 低右(8)
    // 数组索引对应网格位置：0(0,0),1(0,1),2(0,2),3(1,0),4(1,1),5(1,2),6(2,0),7(2,1),8(2,2)
    private readonly string[] _cueActionNames = {
        "TopLeft",    // 0: 网格位置(0,0) - 高左
        "TopSpin",    // 1: 网格位置(0,1) - 高杆
        "TopRight",   // 2: 网格位置(0,2) - 高右
        "LeftSpin",   // 3: 网格位置(1,0) - 左杆
        "Center",     // 4: 网格位置(1,1) - 中杆
        "RightSpin",  // 5: 网格位置(1,2) - 右杆
        "BottomLeft", // 6: 网格位置(2,0) - 低左
        "BackSpin",   // 7: 网格位置(2,1) - 低杆
        "BottomRight"  // 8: 网格位置(2,2) - 低右
    };
    
    // 杆法名称到枚举值的映射，确保按钮点击时能正确获取对应的CueHitType
    private readonly CueHitType[] _cueHitTypeMap = {
        CueHitType.TopLeft,    // 0
        CueHitType.TopSpin,    // 1
        CueHitType.TopRight,   // 2
        CueHitType.LeftSpin,   // 3
        CueHitType.Center,     // 4
        CueHitType.RightSpin,  // 5
        CueHitType.BottomLeft, // 6
        CueHitType.BackSpin,   // 7
        CueHitType.BottomRight // 8
    };
    
    // UI元素引用（通过代码动态查找，无需手动挂载）
    private Button _cueActionButton;
    private GameObject _cueActionPanel;
    private GridLayoutGroup _gridLayout;
    private Text _cueActionDesText; // 当前杆法描述文本
    
    // Prefab路径
    private const string CUE_ACTION_PREFAB_PATH = "Assets/Prefab/CueAction.prefab";
    
    // 已生成的杆法按钮
    private List<GameObject> _cueActionButtons = new List<GameObject>();
    
    private void Awake()
        {
            // 动态查找UI元素
            FindUIElements();
            
            // 初始化面板
            InitializePanel();
            
            // 隐藏初始面板（添加null检查）
            if (_cueActionPanel != null)
            {
                _cueActionPanel.SetActive(false);
            }
        }
    
    /// <summary>
    /// 动态查找UI元素，避免手动挂载
    /// 简化查找逻辑，只保留最可靠的方式
    /// </summary>
    private void FindUIElements()
    {
        // 查找主画布
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("❌ 未找到Canvas组件，无法初始化杆法面板管理器");
            Debug.LogError("请确保场景中有一个Canvas对象");
            return;
        }
        Debug.Log("✅ 找到Canvas组件");
        
        // 查找杆法触发按钮
        FindCueActionButton(canvas);
        
        // 查找杆法面板
        FindCueActionPanel(canvas);
        
        // 验证是否找到所有必要组件
        ValidateUIElements();
    }
    
    /// <summary>
    /// 查找杆法相关UI元素
    /// 优先按层级查找，支持两种拼写（修复拼写错误问题）
    /// </summary>
    /// <param name="canvas">主画布</param>
    private void FindCueActionButton(Canvas canvas)
    {
        // 优先按层级查找，支持两种拼写（修复拼写错误）
        string[] possibleParentNames = { "PlayControls", "PlayControlls" };
        foreach (string parentName in possibleParentNames)
        {
            Transform parent = canvas.transform.Find(parentName);
            if (parent != null)
            {
                // 查找CueActionButton
                Transform buttonTransform = parent.Find("CueActionButton");
                if (buttonTransform != null)
                {
                    _cueActionButton = buttonTransform.GetComponent<Button>();
                    if (_cueActionButton != null)
                    {
                        _cueActionButton.onClick.AddListener(ToggleCueActionPanel);
                        Debug.Log($"✅ 找到CueActionButton组件，父节点: {parentName}");
                    }
                }

                Transform buttonReRackTransform = parent.Find("ReRack");
                if (buttonReRackTransform != null)
                {
                    if (buttonReRackTransform.TryGetComponent<Button>(out var rerackButton))
                    {
                        rerackButton.onClick.AddListener(ReRack);
                        Debug.Log($"✅ 找到ReRack组件，父节点: {parentName}");
                    }
                }
                
                // 查找CueActionDes文本组件
                Transform desTransform = parent.Find("CueActionDes");
                if (desTransform != null)
                {
                    _cueActionDesText = desTransform.GetComponent<Text>();
                    if (_cueActionDesText != null)
                    {
                        Debug.Log($"✅ 找到CueActionDes文本组件，父节点: {parentName}");
                        // 初始显示中杆
                        _cueActionDesText.text = "Current Cue: Center";
                    }
                    else
                    {
                        // 尝试查找TextMeshProUGUI组件
                        TextMeshProUGUI tmpTextComponent = desTransform.GetComponent<TextMeshProUGUI>();
                        if (tmpTextComponent != null)
                        {
                            Debug.Log($"✅ 找到CueActionDes TextMeshProUGUI组件，父节点: {parentName}");
                            // 初始显示中杆
                        tmpTextComponent.text = "Current Cue: Center";
                        }
                    }
                }
                
                // 如果找到任何一个组件就返回
                if (_cueActionButton != null || _cueActionDesText != null)
                {
                    return;
                }
            }
        }
        
        // 如果按层级找不到，尝试直接查找CueActionButton
        GameObject buttonObj = GameObject.Find("CueActionButton");
        if (buttonObj != null)
        {
            _cueActionButton = buttonObj.GetComponent<Button>();
            if (_cueActionButton != null)
            {
                _cueActionButton.onClick.AddListener(ToggleCueActionPanel);
                Debug.Log("✅ 直接找到CueActionButton组件");
                return;
            }
        }
        
        // 如果都找不到，记录警告
        if (_cueActionButton == null)
        {
            Debug.LogWarning("⚠️ 未找到CueActionButton组件");
            Debug.LogWarning("请确保场景中有一个名称为CueActionButton的Button对象");
            Debug.LogWarning("或父节点名为PlayControls/PlayControlls");
        }
        
        if (_cueActionDesText == null)
        {
            Debug.LogWarning("⚠️ 未找到CueActionDes文本组件");
            Debug.LogWarning("请确保场景中有一个名称为CueActionDes的Text或TextMeshProUGUI对象");
            Debug.LogWarning("父节点名为PlayControls/PlayControlls");
        }
    }
    
    /// <summary>
    /// 查找杆法面板
    /// 优先按层级查找，找不到则直接查找
    /// </summary>
    /// <param name="canvas">主画布</param>
    private void FindCueActionPanel(Canvas canvas)
    {
        // 优先按层级查找
        Transform panelTransform = canvas.transform.Find("CueActionPanel");
        if (panelTransform != null)
        {
            _cueActionPanel = panelTransform.gameObject;
            _gridLayout = _cueActionPanel.GetComponent<GridLayoutGroup>();
            Debug.Log("✅ 找到CueActionPanel组件");
            if (_gridLayout == null)
            {
                Debug.LogWarning("⚠️ CueActionPanel未挂载GridLayoutGroup组件，将尝试添加");
                _gridLayout = _cueActionPanel.AddComponent<GridLayoutGroup>();
                ConfigureGridLayout();
            }
            return;
        }
        
        // 如果按层级找不到，尝试直接查找
        _cueActionPanel = GameObject.Find("CueActionPanel");
        if (_cueActionPanel != null)
        {
            _gridLayout = _cueActionPanel.GetComponent<GridLayoutGroup>();
            Debug.Log("✅ 直接找到CueActionPanel组件");
            if (_gridLayout == null)
            {
                Debug.LogWarning("⚠️ CueActionPanel未挂载GridLayoutGroup组件，将尝试添加");
                _gridLayout = _cueActionPanel.AddComponent<GridLayoutGroup>();
                ConfigureGridLayout();
            }
            return;
        }
        
        // 如果都找不到，记录警告
        Debug.LogWarning("⚠️ 未找到CueActionPanel组件");
        Debug.LogWarning("请确保场景中有一个名称为CueActionPanel的GameObject对象");
    }
    
    /// <summary>
    /// 配置网格布局组件
    /// 确保杆法按钮按合理顺序排列：
    /// 第1行：高左（5）  | 高杆（1） | 高右（6）
    /// 第2行：左杆（3）  | 中杆（0） | 右杆（4）
    /// 第3行：低左（7）  | 低杆（2） | 低右（8）
    /// 注意：实际显示顺序由_cueActionNames数组决定，这里配置网格为3x3布局
    /// </summary>
    private void ConfigureGridLayout()
    {
        if (_gridLayout == null)
            return;
        
        // 设置3x3网格布局
        _gridLayout.cellSize = new Vector2(100f, 30f); // 按钮大小
        _gridLayout.spacing = new Vector2(10f, 10f); // 按钮间距
        _gridLayout.startCorner = GridLayoutGroup.Corner.UpperLeft; // 从左上角开始
        _gridLayout.startAxis = GridLayoutGroup.Axis.Horizontal; // 水平排列
        _gridLayout.childAlignment = TextAnchor.MiddleCenter; // 居中对齐
        _gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount; // 固定列数
        _gridLayout.constraintCount = 3; // 3列，形成3x3网格
        
        // 调整按钮顺序，确保显示顺序合理
        // 中杆 -> 高杆 -> 低杆 -> 左杆 -> 右杆 -> 高左 -> 高右 -> 低左 -> 低右
        // 在3x3网格中显示为：
        // 高左(5)  | 高杆(1) | 高右(6)
        // 左杆(3)  | 中杆(0) | 右杆(4)
        // 低左(7)  | 低杆(2) | 低右(8)
        Debug.Log("✅ 已配置3x3网格布局，确保杆法按钮按合理顺序排列");
    }
    
    /// <summary>
    /// 验证UI元素是否找到
    /// </summary>
    private void ValidateUIElements()
    {
        if (_cueActionButton == null)
        {
            Debug.LogWarning("⚠️ 杆法面板触发器按钮未找到，面板将无法通过按钮触发");
            Debug.LogWarning("可以通过代码调用ShowPanel()或HidePanel()方法来控制面板显示");
        }
        
        if (_cueActionPanel == null)
        {
            Debug.LogWarning("⚠️ 杆法面板未找到，无法生成杆法按钮");
            Debug.LogWarning("请检查场景中是否存在名称为CueActionPanel的GameObject");
        }
        else if (_gridLayout == null)
        {
            Debug.LogWarning("⚠️ GridLayoutGroup组件未找到或创建失败，无法生成杆法按钮");
        }
        else
        {
            Debug.Log("✅ 所有UI元素查找完成，可以正常生成杆法按钮");
        }
    }
    
    /// <summary>
    /// 初始化面板，生成9个杆法按钮
    /// </summary>
    private void InitializePanel()
    {
        if (_cueActionPanel == null || _gridLayout == null)
            return;
        
        // 加载Prefab
        GameObject cueActionPrefab = Resources.Load<GameObject>("CueAction");
        if (cueActionPrefab == null)
        {
            // 尝试使用AssetDatabase加载（编辑器环境）
#if UNITY_EDITOR
            cueActionPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(CUE_ACTION_PREFAB_PATH);
            if (cueActionPrefab == null)
            {
                Debug.LogError("无法加载Prefab: " + CUE_ACTION_PREFAB_PATH);
                return;
            }
#else
            Debug.LogError("无法加载Prefab: CueAction");
            return;
#endif
        }
        
        // 清空现有按钮
        ClearCueActionButtons();
        
        // 生成9个杆法按钮
        for (int i = 0; i < _cueActionNames.Length; i++)
        {
            // 实例化按钮
            GameObject buttonObj = Instantiate(cueActionPrefab, _cueActionPanel.transform);
            buttonObj.name = "CueActionBtn_" + _cueActionNames[i];
            
            // 设置按钮文本 - 同时支持UGUI Text和TextMeshPro组件
            string actionName = _cueActionNames[i];
            Debug.Log($"🔧 正在设置按钮{buttonObj.name}的文本: {actionName}");
            
            // 查找所有可能的文本组件
            Component[] textComponents = buttonObj.GetComponentsInChildren(typeof(Text), true);
            Component[] textMeshProComponents = buttonObj.GetComponentsInChildren(typeof(TextMeshPro), true);
            Component[] textMeshProUGUIComponents = buttonObj.GetComponentsInChildren(typeof(TextMeshProUGUI), true);
            
            Debug.Log($"🔧 找到{textComponents.Length}个Text组件, {textMeshProComponents.Length}个TextMeshPro组件, {textMeshProUGUIComponents.Length}个TextMeshProUGUI组件");
            
            // 尝试设置Text组件
            if (textComponents.Length > 0)
            {
                Text textComponent = (Text)textComponents[0];
                textComponent.text = actionName;
                Debug.Log($"✅ 设置按钮{buttonObj.name}的Text组件文本为: {actionName}");
            }
            // 尝试设置TextMeshPro组件
            else if (textMeshProComponents.Length > 0)
            {
                TextMeshPro textMeshProComponent = (TextMeshPro)textMeshProComponents[0];
                textMeshProComponent.text = actionName;
                Debug.Log($"✅ 设置按钮{buttonObj.name}的TextMeshPro组件文本为: {actionName}");
            }
            // 尝试设置TextMeshProUGUI组件
            else if (textMeshProUGUIComponents.Length > 0)
            {
                TextMeshProUGUI textMeshProUGUIComponent = (TextMeshProUGUI)textMeshProUGUIComponents[0];
                textMeshProUGUIComponent.text = actionName;
                Debug.Log($"✅ 设置按钮{buttonObj.name}的TextMeshProUGUI组件文本为: {actionName}");
            }
            else
            {
                Debug.LogWarning($"⚠️ 无法找到按钮{buttonObj.name}的任何文本组件，请确保Prefab包含Text、TextMeshPro或TextMeshProUGUI组件");
                
                // 尝试直接创建Text组件
                Text newTextComponent = buttonObj.AddComponent<Text>();
                if (newTextComponent != null)
                {
                    newTextComponent.text = actionName;
                    newTextComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                    newTextComponent.fontSize = 14;
                    newTextComponent.color = Color.black;
                    newTextComponent.alignment = TextAnchor.MiddleCenter;
                    Debug.Log($"✅ 为按钮{buttonObj.name}创建了新的Text组件并设置文本为: {actionName}");
                }
            }
            
            // 添加点击事件
            Button buttonComponent = buttonObj.GetComponent<Button>();
            if (buttonComponent != null)
            {
                int index = i; // 闭包变量
                buttonComponent.onClick.AddListener(() => OnCueActionClicked(index));
            }
            
            // 添加到列表
            _cueActionButtons.Add(buttonObj);
        }
        
        Debug.Log("已生成 " + _cueActionButtons.Count + " 个杆法按钮");
    }
    
    /// <summary>
    /// 切换杆法面板的显示/隐藏状态
    /// </summary>
    private void ToggleCueActionPanel()
    {
        if (_cueActionPanel != null)
        {
            bool isActive = _cueActionPanel.activeSelf;
            _cueActionPanel.SetActive(!isActive);
            Debug.Log("杆法面板已" + (!isActive ? "显示" : "隐藏"));
        }
    }


    private void ReRack()
    {

        Debug.Log("还原球的位置");

        // 触发重新rack事件
        GameEvents.InvokeBasicEvent(GameBasicEvent.ReRack);
    }
    
    /// <summary>
    /// 杆法按钮点击事件处理
    /// </summary>
    /// <param name="index">杆法索引</param>
    private void OnCueActionClicked(int index)
    {
        // 使用映射数组获取正确的CueHitType枚举值
        CueHitType hitType = _cueHitTypeMap[index];
        string actionName = _cueActionNames[index];
        
        Debug.Log("点击了杆法: " + actionName + " (枚举: " + hitType + ", 索引: " + index + ")");
        
        // 更新当前杆法描述
        UpdateCueActionDes(actionName);
        
        // 触发杆法类型变化事件
        GameEvents.InvokeEvent<CueHitType>(GameBasicEvent.CueHitTypeChanged, hitType);
        
        // 隐藏面板
        HidePanel();
        
        // 这里可以添加实际的杆法处理逻辑
        // 例如：设置当前选中的杆法，或者调用其他组件的方法
        // Example: someComponent.SetCueHitType(hitType);
    }
    
    /// <summary>
    /// 更新当前杆法描述
    /// </summary>
    /// <param name="actionName">杆法名称</param>
    private void UpdateCueActionDes(string actionName)
    {
        // 查找画布
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogWarning("⚠️ 未找到Canvas组件，无法更新当前杆法描述");
            return;
        }
        
        // 查找PlayControls/PlayControlls父节点
        string[] possibleParentNames = { "PlayControls", "PlayControlls" };
        foreach (string parentName in possibleParentNames)
        {
            Transform parent = canvas.transform.Find(parentName);
            if (parent != null)
            {
                // 查找CueActionDes文本组件
                Transform desTransform = parent.Find("CueActionDes");
                if (desTransform != null)
                {
                    // 尝试查找Text组件
                    Text textComponent = desTransform.GetComponent<Text>();
                    if (textComponent != null)
                    {
                        textComponent.text = "Current Cue: " + actionName;
                        Debug.Log($"✅ 更新CueActionDes文本组件: {actionName}");
                        return;
                    }
                    
                    // 尝试查找TextMeshProUGUI组件
                    TextMeshProUGUI tmpTextComponent = desTransform.GetComponent<TextMeshProUGUI>();
                    if (tmpTextComponent != null)
                    {
                        tmpTextComponent.text = "Current Cue: " + actionName;
                        Debug.Log($"✅ 更新CueActionDes TextMeshProUGUI组件: {actionName}");
                        return;
                    }
                    
                    // 尝试查找TextMeshPro组件
                    TextMeshPro textMeshProComponent = desTransform.GetComponent<TextMeshPro>();
                    if (textMeshProComponent != null)
                    {
                        textMeshProComponent.text = "Current Cue: " + actionName;
                        Debug.Log($"✅ 更新CueActionDes TextMeshPro组件: {actionName}");
                        return;
                    }
                    
                    Debug.LogWarning($"⚠️ 找到CueActionDes对象，但未找到任何文本组件");
                    return;
                }
            }
        }
        
        Debug.LogWarning("⚠️ 未找到CueActionDes文本组件，无法更新当前杆法描述");
    }
    
    /// <summary>
    /// 清空现有杆法按钮
    /// </summary>
    private void ClearCueActionButtons()
    {
        foreach (GameObject button in _cueActionButtons)
        {
            if (button != null)
            {
                Destroy(button);
            }
        }
        _cueActionButtons.Clear();
    }
    
    /// <summary>
    /// 显示杆法面板
    /// </summary>
    public void ShowPanel()
    {
        if (_cueActionPanel != null)
        {
            _cueActionPanel.SetActive(true);
        }
    }
    
    /// <summary>
    /// 隐藏杆法面板
    /// </summary>
    public void HidePanel()
    {
        if (_cueActionPanel != null)
        {
            _cueActionPanel.SetActive(false);
        }
    }
}