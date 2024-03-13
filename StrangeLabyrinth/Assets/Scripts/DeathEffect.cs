using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    public Material material;
    public float strength;
    
    void OnRenderImage (RenderTexture source, RenderTexture destination)
    {
        material.SetFloat("strength", strength);
        Graphics.Blit(source, destination, material);
    }
}
