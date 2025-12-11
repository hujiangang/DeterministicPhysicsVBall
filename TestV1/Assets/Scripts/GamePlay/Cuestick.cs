using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 击球杆法类型（球的旋转与击点）
/// </summary>
public enum CueHitType
{
    /// <summary>中杆（击打球心，无旋转）</summary>
    Center = 0,

    /// <summary>高杆（击打球上部，前旋）</summary>
    TopSpin = 1,

    /// <summary>低杆（击打球下部，回旋）</summary>
    BackSpin = 2,

    /// <summary>左侧旋（击打球左侧）</summary>
    LeftSpin = 3,

    /// <summary>右侧旋（击打球右侧）</summary>
    RightSpin = 4,

    /// <summary>高左（上左斜旋）</summary>
    TopLeft = 5,

    /// <summary>高右（上右斜旋）</summary>
    TopRight = 6,

    /// <summary>低左（下左斜旋）</summary>
    BottomLeft = 7,

    /// <summary>低右（下右斜旋）</summary>
    BottomRight = 8
}

public class Cuestick : MonoBehaviour
{
    private Transform parent;

    private float power = 0f;
    private readonly float maxPullDistance = 0.5f;
    private readonly float maxPower = 0.1f;
    private CueHitType cueHitType = CueHitType.Center;

    void Awake()
    {
        parent = transform.parent;
    }


    public void Rotate(float angle)
    {
        Vector3 rotation = angle * Vector3.up;
        parent.Rotate(rotation, Space.World);
        
    }

    public void AimAt(Vector3 pos)
    {
        Rotate(Vector3.SignedAngle(parent.right, pos - parent.position, Vector3.up));
    }

    private void Strike(float power){

        Vector3 rawForce = maxPower * power * parent.right;
        Vector3 offsetLocal = Ball.GetCueHitPosOffSet(cueHitType); 
        Vector3 pos = parent.TransformPoint(-Ball.radius, offsetLocal.y, -offsetLocal.x);
        pos = Ball.GetSurfacePoint(pos);

        Vector3 spinAdjustedForce = rawForce + parent.position - pos;
        spinAdjustedForce = spinAdjustedForce.normalized * rawForce.magnitude;

        Debug.DrawLine(pos, pos + spinAdjustedForce, Color.red, 10);
        GameEvents.InvokeEvent(GameBasicEvent.Strike, pos, spinAdjustedForce);
    }


    public void OnEnable()
    {
        GameEvents.RegisterBasicEvent(GameBasicEvent.PullCuestick, PullCuestick);
        GameEvents.RegisterBasicEvent(GameBasicEvent.ReleaseCuestick, ReleaseCuestick);
        // 使用泛型事件系统注册带参数的事件
        GameEvents.RegisterEvent<CueHitType>(GameBasicEvent.CueHitTypeChanged, CueHitTypeChanged);
    }

    private void CueHitTypeChanged(CueHitType cueHitType)
    {
        // 处理杆法类型变化
        Debug.Log("杆法类型变化: " + cueHitType);
        this.cueHitType = cueHitType;
    }

    private void PullCuestick()
    {
        float pullpower = 0.005f;
        power += pullpower;
        if (power > 1f)
        {
            power = 1f;
            return;
        }
        transform.Translate(maxPullDistance * pullpower * Vector3.left, Space.Self);
    }

    public void ReleaseCuestick()
    {
        transform.Translate(maxPullDistance * power * Vector3.right, Space.Self);
        Strike(power);
        power = 0f;
    }

    public void OnDisable()
    {
        GameEvents.UnregisterBasicEvent(GameBasicEvent.PullCuestick, PullCuestick);
        GameEvents.UnregisterBasicEvent(GameBasicEvent.ReleaseCuestick, ReleaseCuestick);
        // 使用泛型事件系统注销带参数的事件
        GameEvents.UnregisterEvent<CueHitType>(GameBasicEvent.CueHitTypeChanged, CueHitTypeChanged);
    }
}
