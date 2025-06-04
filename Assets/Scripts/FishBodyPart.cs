using System;
using UnityEngine;

[Serializable]
public class FishBodyPart
{
    [SerializeField] private MeshRenderer _meshRenderer;
    public MeshRenderer MeshRenderer => _meshRenderer;

    [SerializeField] private MeshFilter _meshFilter;
    public MeshFilter MeshFilter => _meshFilter;
}
