using System;
using UnityEngine;

[Serializable]
public class FishSettings
{
    [SerializeField] private string _name;
    public string Name => _name;

    [SerializeField] private Mesh _headMesh;
    public Mesh HeadMesh => _headMesh;

    [SerializeField] private Mesh _midMesh;
    public Mesh MidMesh => _midMesh;

    [SerializeField] private Mesh _tailMesh;
    public Mesh TailMesh => _tailMesh;

    [SerializeField] private Material _material;
    public Material Material => _material;
    
    [SerializeField] private Color _color;
    public Color Color => _color;

    [SerializeField] private float _moveSpeed;
    public float MoveSpeed => _moveSpeed;
}
