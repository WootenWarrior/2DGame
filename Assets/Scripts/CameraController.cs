using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float smoothing;
    void Start()
    {
        
    }

    void LateUpdate()
    {
        Vector3 targetPos = new Vector3(target.position.x,target.position.y,-10);
        transform.position = targetPos;
    }
}
