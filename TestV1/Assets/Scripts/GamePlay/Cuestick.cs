using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cuestick : MonoBehaviour
{

    private Transform parent;

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
}
