using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : PhySphereEntity
{
    // Start is called before the first frame update
     protected override void Start()
    {
        base.Start();

        Table.Instance.RegisterBall(this, transform.position);
    }

    public void ResetPos(Vector3 pos)
    {
        transform.position = pos;
        SyncPhyTransformWithUnityTransform();
    }


    /// <summary>
    /// 获取表面最近的点.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static Vector3 GetSurfacePoint(Vector3 pos)
    {
        return col.ClosestPoint(pos);
    }

    /// <summary>
    /// 获取击打球的偏移.
    /// </summary>
    /// <param name="cueHitType"></param>
    /// <returns></returns>
    public static Vector3 GetCueHitPosOffSet(CueHitType cueHitType)
    {
        Vector3 offset = Vector3.zero;
        float max = 0.5f;
        float middle = 0.3f;

        switch (cueHitType)
        {
            case CueHitType.Center:
                break;
            case CueHitType.TopSpin:
                offset = new Vector3(0, radius * max, 0);
                break;
            case CueHitType.BackSpin:
                offset = new Vector3(0, -radius * max, 0);
                break;
            case CueHitType.LeftSpin:
                offset = new Vector3(-radius * max, 0, 0);
                break;
            case CueHitType.RightSpin:
                offset = new Vector3(radius * max, 0, 0);
                break;
            case CueHitType.TopLeft:
                offset = new Vector3(-radius * middle, radius * middle, 0);
                break;
            case CueHitType.TopRight:
                offset = new Vector3(radius * middle, radius * middle, 0);
                break;
            case CueHitType.BottomLeft:
                offset = new Vector3(-radius * middle, -radius * middle, 0);
                break;
            case CueHitType.BottomRight:
                offset = new Vector3(radius * middle, -radius * middle, 0);
                break;
        }

        return offset;
    }
}
