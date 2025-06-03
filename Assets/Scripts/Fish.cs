using UnityEngine;

public class Fish : IFixedUpdate, IDrawGizmos
{
    private const float TARGET_DISTANCE_THRESHOLD = 0.1f;
    private FishComponentContainer _componentContainer;
    private UnityLifecycleEventRunner _unityEventsRunner;
    private Bounds _moveableBounds;
    private Color _color = Color.white;
    
    private float _moveSpeed = 0.5f;
    private Vector3 _targetPosition = Vector3.zero;

    public Fish(UnityLifecycleEventRunner unityEventsRunner, FishComponentContainer componentContainer, Bounds moveableBounds)
    {
        _componentContainer = componentContainer;
        _unityEventsRunner = unityEventsRunner;
        _moveableBounds = moveableBounds;
        _componentContainer.RigidBody.maxLinearVelocity = _moveSpeed;
        _componentContainer.RigidBody.maxAngularVelocity = _moveSpeed;
    }

    public void ToggleActive(bool state)
    {
        if(state)
        {
            _unityEventsRunner.RegisterFixedUpdate(this);
            _unityEventsRunner.RegisterDrawGizmos(this);
            ResetRigidbodyWithNewTargetPosition();
            _componentContainer.gameObject.SetActive(true);
        }
        else
        {
            _componentContainer.gameObject.SetActive(false);
        }
    }

    public Fish SetSettings(FishSettings settings)
    {
        _moveSpeed = settings.MoveSpeed;
        _componentContainer.FishHead.MeshCollider.sharedMesh = settings.HeadMesh;
        _componentContainer.FishMid.MeshCollider.sharedMesh = settings.MidMesh;
        _componentContainer.FishTail.MeshCollider.sharedMesh = settings.TailMesh;
        _componentContainer.FishHead.MeshRenderer.material = settings.Material;
        _componentContainer.FishMid.MeshRenderer.material = settings.Material;
        _componentContainer.FishTail.MeshRenderer.material = settings.Material;
        return this;
    }

    public void FixedUpdate()
    {
        if(Vector3.Distance(_componentContainer.RigidBody.position, _targetPosition) < TARGET_DISTANCE_THRESHOLD || Physics.Raycast(_componentContainer.RigidBody.position, _componentContainer.RigidBody.transform.forward, 0.2f, _componentContainer.RigidBody.gameObject.layer))
        {
            ResetRigidbodyWithNewTargetPosition();
        }

        Vector3 moveDir = (_targetPosition - _componentContainer.RigidBody.position).normalized * _moveSpeed * Time.fixedDeltaTime;

        _componentContainer.RigidBody.AddForce(moveDir, ForceMode.VelocityChange);
    }

    private void ResetRigidbodyWithNewTargetPosition()
    {
        _componentContainer.RigidBody.linearVelocity = Vector3.zero;
        _targetPosition = BoundsUtility.GetRandomPointInBounds(_moveableBounds);
        _componentContainer.RigidBody.transform.LookAt(_targetPosition);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_targetPosition, 0.01f);

        Gizmos.DrawRay(_componentContainer.RigidBody.position, _targetPosition - _componentContainer.RigidBody.position);
    }
}
