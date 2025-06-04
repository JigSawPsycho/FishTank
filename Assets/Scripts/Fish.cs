using UnityEngine;
using UnityEngine.EventSystems;

public class Fish : IFixedUpdate, IUpdate
{
    private const float TARGET_DISTANCE_THRESHOLD = 0.1f;
    private FishComponentContainer _componentContainer;
    private UnityLifecycleEventRunner _unityEventsRunner;
    private Color _color = Color.white;
    
    private float _moveSpeed = 0.5f;
    private PointerEventSubject _pointerEventSubject;
    private bool _grabbed;
    private PhysicsMover _grabbedPhysicsHandler;
    private PhysicsMover _releasedPhysicsMover;
    private PhysicsMover _fishTankPhysicsMover;

    private int _partToCut = 0;
    private int _bodyPartsCount = 0;

    private bool _dead = false;

    private PhysicsMover GetActivePhysicsMover()
    {
        if(_grabbedPhysicsHandler.State == EPhysicsMoverState.Active) return _grabbedPhysicsHandler;
        if(_releasedPhysicsMover.State == EPhysicsMoverState.Active) return _releasedPhysicsMover;
        if(_fishTankPhysicsMover.State == EPhysicsMoverState.Active) return _fishTankPhysicsMover;
        return null;
    }

    public Fish(UnityLifecycleEventRunner unityEventsRunner, FishComponentContainer componentContainer, PhysicsMover releasedPhysicsMover, PhysicsMover grabbedPhysicsMover, PhysicsMover fishTankPhysicsMover)
    {
        _componentContainer = componentContainer;
        _unityEventsRunner = unityEventsRunner;
        _releasedPhysicsMover = releasedPhysicsMover;
        _grabbedPhysicsHandler = grabbedPhysicsMover;
        _fishTankPhysicsMover = fishTankPhysicsMover;
        _componentContainer.RigidBody.maxLinearVelocity = _moveSpeed;
        _componentContainer.RigidBody.maxAngularVelocity = _moveSpeed;
        SetClickHandler(_componentContainer.PointerEventSubject);
        _releasedPhysicsMover.EnterState(EPhysicsMoverState.Inactive);
        _grabbedPhysicsHandler.EnterState(EPhysicsMoverState.Inactive);
        _fishTankPhysicsMover.EnterState(EPhysicsMoverState.Active);
        _bodyPartsCount = _componentContainer.RigidBody.transform.childCount;
    }

    public void ToggleActive(bool state)
    {
        if(state)
        {
            _unityEventsRunner.RegisterFixedUpdate(this);
            _unityEventsRunner.RegisterUpdate(this);
            _componentContainer.ColliderTriggerEventSubject.RegisterOnTriggerEnterListener(ColliderTriggerEventSubject_OnTriggerEnter);
            _releasedPhysicsMover.EnterState(EPhysicsMoverState.Inactive);
            _grabbedPhysicsHandler.EnterState(EPhysicsMoverState.Inactive);
            _fishTankPhysicsMover.EnterState(EPhysicsMoverState.Active);
            for(int i = 0; i < _bodyPartsCount; i++)
            {
                _componentContainer.RigidBody.transform.GetChild(i).gameObject.SetActive(true);
            }
            _dead = false;
            _componentContainer.gameObject.SetActive(true);
        }
        else
        {
            _componentContainer.ColliderTriggerEventSubject.DeregisterOnTriggerEnterListener(ColliderTriggerEventSubject_OnTriggerEnter);
            _unityEventsRunner.DeregisterFixedUpdate(this);
            _unityEventsRunner.DeregisterUpdate(this);
            _componentContainer.gameObject.SetActive(false);
        }
    }

    public Fish SetMoveSpeed(float moveSpeed)
    {
        _moveSpeed = moveSpeed;
        return this;
    }

    public Fish SetHead(Mesh headMesh, Material headMaterial)
    {
        _componentContainer.FishHead.MeshFilter.sharedMesh = headMesh;
        _componentContainer.FishHead.MeshRenderer.material = headMaterial;
        return this;
    }

    public Fish SetMiddle(Mesh middleMesh, Material middleMaterial)
    {
        _componentContainer.FishMid.MeshFilter.sharedMesh = middleMesh;
        _componentContainer.FishMid.MeshRenderer.material = middleMaterial;
        return this;
    }

    public Fish SetTail(Mesh tailMesh, Material tailMaterial)
    {
        _componentContainer.FishTail.MeshFilter.sharedMesh = tailMesh;
        _componentContainer.FishTail.MeshRenderer.material = tailMaterial;
        return this;
    }

    public Fish SetClickHandler(PointerEventSubject pointerEventSubject)
    {
        if(_pointerEventSubject != null)
        {
            _pointerEventSubject.DeregisterOnPointerDownListener(PointerEventSubject_OnPointerDown);
            _pointerEventSubject.DeregisterOnPointerUpListener(PointerEventSubject_OnPointerUp);
        }
        _pointerEventSubject = pointerEventSubject;
        _pointerEventSubject.RegisterOnPointerDownListener(PointerEventSubject_OnPointerDown);
        _pointerEventSubject.RegisterOnPointerUpListener(PointerEventSubject_OnPointerUp);
        return this;
    }

    public Fish SetGrabbedPhysicsHandler(PhysicsMover grabbedPhysicsHandler)
    {
        _grabbedPhysicsHandler = grabbedPhysicsHandler;
        return this;
    }

    public Fish SetReleasedPhysicsHandler(PhysicsMover releasedPhysicsMover)
    {
        _releasedPhysicsMover = releasedPhysicsMover;
        return this;
    }

    public Fish SetFishtankPhysicsHandler(PhysicsMover fishtankPhysicsMover)
    {
        _fishTankPhysicsMover = fishtankPhysicsMover;
        return this;
    }

    private void PointerEventSubject_OnPointerDown(PointerEventData eventData)
    {
        _grabbed = true;
        _fishTankPhysicsMover.EnterState(EPhysicsMoverState.Inactive);
        _releasedPhysicsMover.EnterState(EPhysicsMoverState.Inactive);
        _grabbedPhysicsHandler.EnterState(EPhysicsMoverState.Active);
    }

    private void PointerEventSubject_OnPointerUp(PointerEventData eventData)
    {
        _grabbed = false;
        _fishTankPhysicsMover.EnterState(EPhysicsMoverState.Inactive);
        _grabbedPhysicsHandler.EnterState(EPhysicsMoverState.Inactive);
        _releasedPhysicsMover.EnterState(EPhysicsMoverState.Active);
    }

    private void ColliderTriggerEventSubject_OnTriggerEnter(Collider other)
    {
        // When we have other triggers, check the Collider
        if(!_dead)
        {
            _grabbedPhysicsHandler.EnterState(EPhysicsMoverState.Inactive);
            _releasedPhysicsMover.EnterState(EPhysicsMoverState.Inactive);
            _fishTankPhysicsMover.EnterState(EPhysicsMoverState.Active);
        }
    }

    public void Update()
    {
        PhysicsMover state = GetActivePhysicsMover();
        if(state == _grabbedPhysicsHandler && Input.GetKeyDown(KeyCode.Space) && _partToCut < 3)
        {
            _componentContainer.RigidBody.transform.GetChild(_partToCut).gameObject.SetActive(false);
            _partToCut++;
            _dead = true;
        }
    }

    public void FixedUpdate()
    {
        GetActivePhysicsMover().MoveRigidbody();
    }
}
