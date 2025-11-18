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
        CueballStrike();
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

        Vector2 offset = cueball.GetCueHitPosOffSet(CueHitType.TopLeft);
       
        Vector3 pos = cueball.transform.TransformPoint(offset.x, offset.y, -cueball.radius);

        Vector3 hitPos = cueball.GetSurfacePoint(pos);

        Debug.DrawLine(hitPos, this.destball.transform.position, Color.red, 5.0f);

        this.cueball.Strike(hitPos, impulse);

    }
}
