using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    float offsetX;
    [SerializeField] GameObject Mouse;
    void Start()
    {
        offsetX = Mouse.transform.position.x - gameObject.transform.position.x;
    }
    void FixedUpdate()
    {
        gameObject.transform.position = new Vector3(Mouse.transform.position.x - offsetX, gameObject.transform.position.y, gameObject.transform.position.z);
    }
}
