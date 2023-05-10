using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParralexCamera : MonoBehaviour
{
    //1
    public Renderer background;
    public Renderer foreground;
    //2
    public float backgroundSpeed = 0.02f;
    public float foregroundSpeed = 0.06f;
    //3
    public float offset = 0.0f;
    public void Update()
    {
        background.material.mainTextureOffset = new Vector2(offset * backgroundSpeed, 0);
        foreground.material.mainTextureOffset = new Vector2(offset * foregroundSpeed, 0);
    }
}
