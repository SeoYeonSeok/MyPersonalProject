using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateItem : MonoBehaviour
{
    public float rotSpeed = 50f; // 회전 속도

    void Update()
    {        
        transform.Rotate(Vector3.up, rotSpeed * Time.deltaTime);
    }
}
