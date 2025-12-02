using SharpDX.DirectWrite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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


public class Test1 : MonoBehaviour
{

    /// <summary>
    /// 母球.
    /// </summary>
    public Cueball cueball;

    /// <summary>
    /// 目标测试球.
    /// </summary>
    public Ball destball;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayedStrike());
    }

    private System.Collections.IEnumerator DelayedStrike()
    {
        yield return new WaitForSeconds(2f);
        Test1CueballStrike();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateMiniCube(Vector3 pos)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // 设置位置
        cube.transform.position = pos;

        // 修改大小
        cube.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
    }


    private void TestHitPosGroup()
    {
        TestHitPoint( CueHitType.Center);

        TestHitPoint(CueHitType.TopSpin);

        TestHitPoint(CueHitType.BackSpin);

        TestHitPoint(CueHitType.LeftSpin);

        TestHitPoint(CueHitType.RightSpin);

        TestHitPoint(CueHitType.TopLeft);

        TestHitPoint(CueHitType.TopRight);

        TestHitPoint(CueHitType.BottomLeft);

        TestHitPoint(CueHitType.BottomRight);

    }

    private void TestHitPoint(CueHitType cueHitType)
    {
        Vector3 dir = (this.destball.transform.position - this.cueball.transform.position).normalized;
        Vector3 impulse = dir * 0.1f;

        Vector2 offset = cueball.GetCueHitPosOffSet(cueHitType);

        Vector3 pos = cueball.transform.TransformPoint(offset.x, offset.y, -cueball.radius);

        Vector3 hitPos = cueball.GetSurfacePoint(pos);

        GenerateMiniCube(hitPos);
    }

    public void CueballStrike()
    {

        Vector3 dir = (this.destball.transform.position - this.cueball.transform.position).normalized;
        Vector3 impulse = dir * 0.1f;

        Vector2 offset = cueball.GetCueHitPosOffSet(CueHitType.BackSpin);
       
        Vector3 pos = cueball.transform.TransformPoint(offset.x, offset.y, -cueball.radius);

        Vector3 hitPos = cueball.GetSurfacePoint(pos);

        Debug.DrawLine(hitPos, this.destball.transform.position, Color.red, 5.0f);

        this.cueball.Strike(hitPos, impulse);

    }


    // 假设 cueballEntity 是你的 BEPU Entity 对象
    // 假设 destball 是目标位置
    public void Test1CueballStrike()
    {
        float r = cueball.radius; // 从 BEPU 获取物理半径

        // 1. 计算偏移 (x:左右, y:高低)
        Vector3 offsetLocal = cueball.GetCueHitPosOffSet(CueHitType.LeftSpin); // 比如打左塞

        // 2. 计算球体表面的 Z 深度 (勾股定理)
        float z = -Mathf.Sqrt(Mathf.Max(0, r * r - offsetLocal.x * offsetLocal.x - offsetLocal.y * offsetLocal.y));
        Vector3 hitPointRelative = new Vector3(offsetLocal.x, offsetLocal.y, z);

        // 3. 构建旋转矩阵，转到世界坐标
        // 注意：BEPU 使用自己的 Vector3 (BEPUutilities.Vector3)，这里假设你做过转换或者用了别名
        Vector3 aimDir = (destball.transform.position - cueball.transform.position).normalized;
        Quaternion shotRotation = Quaternion.LookRotation(aimDir, Vector3.up);

        // 4. 算出世界坐标下的击球点
        Vector3 hitPosWorld = cueball.transform.position + (shotRotation * hitPointRelative);

        // 5. 算出冲量向量 (方向向前)
        Vector3 impulseVec = aimDir * 0.1f; // 力度

        Debug.DrawLine(hitPosWorld, this.destball.transform.position, Color.red, 5.0f);

        // 6. 【关键】BEPU 调用
        // ApplyImpulse 会自动根据 hitPosWorld 和 Position 的差值计算出力矩(Torque)
        cueball.Strike(hitPosWorld, impulseVec);
    }
}
