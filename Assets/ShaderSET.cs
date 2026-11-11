using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderSET : MonoBehaviour
{
    public Material shader;
    public float r, g, b;
    void Start()
    {
        shader.color = new Color(r, g, b);
    }

}
