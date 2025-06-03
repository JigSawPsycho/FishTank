using System;
using UnityEngine;

[Serializable]
public class FishBodyPart
{
    [SerializeField] private MeshRenderer _meshRenderer;
    public MeshRenderer MeshRenderer => _meshRenderer;

    [SerializeField] private MeshCollider _meshCollider;
    public MeshCollider MeshCollider => _meshCollider;
}
