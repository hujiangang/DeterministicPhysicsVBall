using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 集中式球杆管理器
/// 替代将脚本挂载在球杆对象上的方式
/// </summary>
public class CuestickManager : MonoBehaviour
{
    /// <summary>
    /// 单例实例
    /// </summary>
    public static CuestickManager Instance { get; private set; }
    
    /// <summary>
    /// 球杆对象
    /// </summary>
    [SerializeField]
    private GameObject cuestick;
    
    /// <summary>
    /// 白球对象
    /// </summary>
    [SerializeField]
    private GameObject whiteBall;
    
    /// <summary>
    /// 瞄准线材质
    /// </summary>
    [SerializeField]
    private Material aimLineMaterial;
    
    /// <summary>
    /// 球杆标签
    /// </summary>
    private const string CUESTICK_TAG = "Cuestick";
    
    /// <summary>
    /// 白球标签
    /// </summary>
    private const string WHITE_BALL_TAG = "Cueball";
    
    /// <summary>
    /// 球杆与白球的距离
    /// </summary>
    private float cuestickDistance = 0.5f;
    
    /// <summary>
    /// 最大击打力度
    /// </summary>
    private float maxPower = 100f;
    
    /// <summary>
    /// 当前击打力度
    /// </summary>
    private float currentPower = 0f;
    
    /// <summary>
    /// 是否正在瞄准
    /// </summary>
    private bool isAiming = true;
    
    /// <summary>
    /// 瞄准方向
    /// </summary>
    private Vector3 aimDirection = Vector3.forward;
    
    /// <summary>
    /// 瞄准线长度
    /// </summary>
    private float aimLineLength = 5f;
    
    private void Awake()
    {
        // 初始化单例
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        
        // 确保不被销毁
        DontDestroyOnLoad(gameObject);
        
        // 自动查找球杆和白球
        FindCuestickAndWhiteBall();
    }
    
    private void Start()
    {
        // 初始化球杆位置
        UpdateCuestickPosition();
    }
    
    private void Update()
    {
        if (isAiming)
        {
            // 处理瞄准输入
            HandleAimingInput();
            
            // 更新球杆位置
            UpdateCuestickPosition();
        }
        
        // 处理击打输入
        HandleHitInput();
    }
    
    private void OnRenderObject()
    {
        // 绘制瞄准线
        DrawAimLine();
    }
    
    /// <summary>
    /// 自动查找球杆和白球
    /// </summary>
    private void FindCuestickAndWhiteBall()
    {
        // 查找球杆
        if (cuestick == null)
        {
            cuestick = GameObject.FindWithTag(CUESTICK_TAG);
            if (cuestick == null)
            {
                // 尝试通过名称查找
                cuestick = GameObject.Find(CUESTICK_TAG);
            }
        }
        
        // 查找白球
        if (whiteBall == null)
        {
            whiteBall = GameObject.FindWithTag(WHITE_BALL_TAG);
            if (whiteBall == null)
            {
                // 尝试通过名称查找
                whiteBall = GameObject.Find(WHITE_BALL_TAG);
            }
        }
        
        Debug.Log("CuestickManager: Found cuestick=" + (cuestick != null) + ", whiteBall=" + (whiteBall != null));
    }
    
    /// <summary>
    /// 处理瞄准输入
    /// </summary>
    private void HandleAimingInput()
    {
        // 简单的鼠标瞄准实现
        if (Input.GetMouseButton(1)) // 右键瞄准
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            
            // 旋转瞄准方向
            Quaternion rotation = Quaternion.Euler(mouseY * 5f, mouseX * 5f, 0f);
            aimDirection = rotation * aimDirection;
            aimDirection.Normalize();
        }
        
        // 键盘控制力度
        if (Input.GetKey(KeyCode.Space))
        {
            currentPower = Mathf.Min(currentPower + Time.deltaTime * 20f, maxPower);
        }
        else if (currentPower > 0f)
        {
            currentPower = Mathf.Max(currentPower - Time.deltaTime * 30f, 0f);
        }
    }
    
    /// <summary>
    /// 处理击打输入
    /// </summary>
    private void HandleHitInput()
    {
        if (Input.GetMouseButtonUp(0) && currentPower > 0f) // 左键击打
        {
            HitWhiteBall();
        }
    }
    
    /// <summary>
    /// 更新球杆位置
    /// </summary>
    private void UpdateCuestickPosition()
    {
        if (cuestick == null || whiteBall == null)
            return;
        
        // 计算球杆位置：白球位置 + 瞄准方向 * 距离
        Vector3 cuestickPos = whiteBall.transform.position + aimDirection * cuestickDistance;
        cuestick.transform.position = cuestickPos;
        
        // 计算球杆旋转：朝向白球
        cuestick.transform.LookAt(whiteBall.transform);
        
        // 调整球杆旋转，使其正确指向
        cuestick.transform.Rotate(0f, 90f, 0f);
    }
    
    /// <summary>
    /// 击打白球
    /// </summary>
    private void HitWhiteBall()
    {
        if (whiteBall == null)
            return;
        
        // 获取白球的物理实体
        BEPUphysics.Entities.Entity whiteBallPhy = null;
        
        // 检查是否使用集中式物理管理器
        if (PhyEntityManager.Instance != null && PhyEntityManager.Instance.HasPhyEntity(whiteBall))
        {
            whiteBallPhy = PhyEntityManager.Instance.GetPhyEntity(whiteBall);
        }
        else
        {
            // 检查是否挂载了传统物理脚本
            PhyBaseEntity phyBaseEntity = whiteBall.GetComponent<PhyBaseEntity>();
            if (phyBaseEntity != null)
            {
                // 使用反射获取物理实体
                System.Reflection.FieldInfo phyEntityField = typeof(PhyBaseEntity).GetField("phyEntity", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                if (phyEntityField != null)
                {
                    whiteBallPhy = phyEntityField.GetValue(phyBaseEntity) as BEPUphysics.Entities.Entity;
                }
            }
        }
        
        if (whiteBallPhy != null)
        {
            // 计算击打力度
            BEPUutilities.Vector3 force = ConversionHelper.MathConverter.Convert(aimDirection * currentPower);
            
            // 应用冲击力
            whiteBallPhy.ApplyLinearImpulse(ref force);
            
            Debug.Log("Hit white ball with power: " + currentPower + ", force: " + force);
        }
        
        // 重置力度
        currentPower = 0f;
    }
    
    /// <summary>
    /// 绘制瞄准线
    /// </summary>
    private void DrawAimLine()
    {
        if (whiteBall == null || aimLineMaterial == null)
            return;
        
        // 激活材质
        aimLineMaterial.SetPass(0);
        
        // 设置线宽
        GL.Begin(GL.LINES);
        GL.Color(Color.red);
        
        // 绘制瞄准线
        Vector3 startPos = whiteBall.transform.position;
        Vector3 endPos = startPos + aimDirection * aimLineLength;
        
        GL.Vertex(startPos);
        GL.Vertex(endPos);
        
        GL.End();
    }
    
    /// <summary>
    /// 设置球杆对象
    /// </summary>
    /// <param name="newCuestick">新的球杆对象</param>
    public void SetCuestick(GameObject newCuestick)
    {
        cuestick = newCuestick;
    }
    
    /// <summary>
    /// 设置白球对象
    /// </summary>
    /// <param name="newWhiteBall">新的白球对象</param>
    public void SetWhiteBall(GameObject newWhiteBall)
    {
        whiteBall = newWhiteBall;
    }
    
    /// <summary>
    /// 设置瞄准方向
    /// </summary>
    /// <param name="direction">瞄准方向</param>
    public void SetAimDirection(Vector3 direction)
    {
        aimDirection = direction.normalized;
    }
    
    /// <summary>
    /// 设置球杆与白球的距离
    /// </summary>
    /// <param name="distance">距离</param>
    public void SetCuestickDistance(float distance)
    {
        cuestickDistance = distance;
    }
    
    /// <summary>
    /// 获取当前击打力度
    /// </summary>
    /// <returns>当前力度</returns>
    public float GetCurrentPower()
    {
        return currentPower;
    }
    
    /// <summary>
    /// 获取最大击打力度
    /// </summary>
    /// <returns>最大力度</returns>
    public float GetMaxPower()
    {
        return maxPower;
    }
}