using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateItem : MonoBehaviour
{
    public float rotSpeed = 50f; // ȸ�� �ӵ�

    void Update()
    {        
        transform.Rotate(Vector3.up, rotSpeed * Time.deltaTime);
    }
}
