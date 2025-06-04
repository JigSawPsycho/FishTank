using UnityEngine;

public class SetPositionToMousePhysics : PhysicsMover
{
    public const float ABOVE_TABLE_Z = 0.4f;

    private Vector3 _lastCamPos = Vector3.zero;

    public SetPositionToMousePhysics(Rigidbody rigidbody) : base(rigidbody) { }

    public override void MoveRigidbody()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = ABOVE_TABLE_Z;
        _lastCamPos = Camera.main.ScreenToWorldPoint(mousePos);
        _rigidbody.MovePosition(_lastCamPos);
    }

    public override void EnterState(EPhysicsMoverState state)
    {
        base.EnterState(state);
        if(state == EPhysicsMoverState.Active)
        {
            _rigidbody.useGravity = false;
        }
    }
}
