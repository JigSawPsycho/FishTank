using UnityEngine;

public class MoveToRandomPositionInBounds : PhysicsMover, IDrawGizmos
{
    private Vector3 _targetPosition;
    private Bounds _moveableBounds;
    private float _targetDistanceThreshold = 0.1f;
    private float _moveSpeed = 0.5f;

    public MoveToRandomPositionInBounds(Rigidbody rigidbody, Bounds moveableBounds, UnityLifecycleEventRunner unityLifecycleEventRunner = null) : base(rigidbody)
    {
        _moveableBounds = moveableBounds;
        _targetPosition = BoundsUtility.GetRandomPointInBounds(_moveableBounds);
        unityLifecycleEventRunner.RegisterDrawGizmos(this);
    }

    public override void MoveRigidbody()
    {
        if(Vector3.Distance(_rigidbody.position, _targetPosition) < _targetDistanceThreshold || Physics.Raycast(_rigidbody.position, _rigidbody.transform.forward, 0.2f, _rigidbody.gameObject.layer))
        {
            ResetRigidbodyWithNewTargetPosition();
        }
        Vector3 moveDir = (_targetPosition - _rigidbody.position).normalized * _moveSpeed * Time.fixedDeltaTime;

        _rigidbody.AddForce(moveDir, ForceMode.VelocityChange);
    }
    
    private void ResetRigidbodyWithNewTargetPosition()
    {
        ResetVelocities();
        _targetPosition = BoundsUtility.GetRandomPointInBounds(_moveableBounds, 0.05f);
        _rigidbody.transform.LookAt(_targetPosition);
    }

    public MoveToRandomPositionInBounds SetMoveSpeed(float moveSpeed)
    {
        _moveSpeed = moveSpeed;
        return this;
    }

    public MoveToRandomPositionInBounds SetTargetDistanceThreshold(float targetDistanceThreshold)
    {
        _targetDistanceThreshold = targetDistanceThreshold;
        return this;
    }

    public void OnDrawGizmos()
    {
        if(State != EPhysicsMoverState.Active) return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_targetPosition, 0.01f);

        Gizmos.DrawRay(_rigidbody.position, _targetPosition - _rigidbody.position);
    }

    public override void EnterState(EPhysicsMoverState state)
    {
        base.EnterState(state);
        if(state == EPhysicsMoverState.Active)
        {
            ResetRigidbodyWithNewTargetPosition();
            _rigidbody.useGravity = false;
        }
    }
}
