using UnityEngine;

public class FallingPhysics : PhysicsMover
{
    public const float ABOVE_FISHTANK_Z = 0f;

    public FallingPhysics(Rigidbody rigidbody) : base(rigidbody) { }

    public override void MoveRigidbody()
    {
        // we let gravity do the work
    }

    public override void EnterState(EPhysicsMoverState state)
    {
        base.EnterState(state);
        if(state == EPhysicsMoverState.Active)
        {
            _rigidbody.MovePosition(new Vector3(_rigidbody.position.x, _rigidbody.position.y, ABOVE_FISHTANK_Z));
            _rigidbody.useGravity = true;
        }
    }
}
