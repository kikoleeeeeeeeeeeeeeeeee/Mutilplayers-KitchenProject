using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private MeshRenderer bodyMeshRenderer;
    [SerializeField] private MeshRenderer headmeshRenderer;

    private Material material;
    private void Awake()
    {
        material = new Material(headmeshRenderer.material);
        headmeshRenderer.material = material;
        bodyMeshRenderer.material = material;
    }
    public void  SetPlayerColor( Color color)
    {
        material.color = color;
    }
}
