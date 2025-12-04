using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cuestick : MonoBehaviour
{
    private Transform parent;

    private float power = 0f;
    private float maxPullDistance = 0.5f;

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


    public void OnEnable()
    {
        GameEvents.RegisterBasicEvent(GameBasicEvent.PullCuestick, PullCuestick);
        GameEvents.RegisterBasicEvent(GameBasicEvent.ReleaseCuestick, ReleaseCuestick);
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
        power = 0f;
    }

    public void OnDisable()
    {
        GameEvents.UnregisterBasicEvent(GameBasicEvent.PullCuestick, PullCuestick);
        GameEvents.UnregisterBasicEvent(GameBasicEvent.ReleaseCuestick, ReleaseCuestick);
    }
}
