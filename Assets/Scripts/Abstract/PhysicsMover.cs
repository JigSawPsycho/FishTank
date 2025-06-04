using UnityEngine;

public abstract class PhysicsMover
{
    protected Rigidbody _rigidbody;
    protected EPhysicsMoverState _state = EPhysicsMoverState.Inactive;
    public EPhysicsMoverState State => _state;

    public PhysicsMover(Rigidbody rigidbody)
    {
        _rigidbody = rigidbody;
    }

    public virtual void EnterState(EPhysicsMoverState state)
    {
        _state = state;
        ResetVelocities();
    }

    public virtual void ResetVelocities()
    {
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }
    // To be called in FixedUpdate
    public abstract void MoveRigidbody();
}
