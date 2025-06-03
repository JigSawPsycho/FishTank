using System.Collections.Generic;
using UnityEngine;

public class FishComponentContainer : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    public Rigidbody RigidBody => _rigidbody;

    [SerializeField] private FishBodyPart _fishHead;
    public FishBodyPart FishHead => _fishHead;

    [SerializeField] private FishBodyPart _fishMid;
    public FishBodyPart FishMid => _fishMid;

    [SerializeField] private FishBodyPart _fishTail;
    public FishBodyPart FishTail => _fishTail;
}
