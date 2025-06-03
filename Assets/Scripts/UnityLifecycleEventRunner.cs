using System;

public class UnityLifecycleEventRunner : IFixedUpdate, IDrawGizmos
{
    private Action _onFixedUpdate;
    private Action _onDrawGizmos;

    public void RegisterFixedUpdate(IFixedUpdate eventListener)
    {
        _onFixedUpdate += eventListener.FixedUpdate;
    }

    public void RegisterDrawGizmos(IDrawGizmos eventListener)
    {
        _onDrawGizmos += eventListener.OnDrawGizmos;
    }
    
    public void DeregisterFixedUpdate(IFixedUpdate eventListener)
    {
        _onFixedUpdate += eventListener.FixedUpdate;
    }

    public void DeregisterDrawGizmos(IDrawGizmos eventListener)
    {
        _onDrawGizmos += eventListener.OnDrawGizmos;
    }

    public void FixedUpdate()
    {
        _onFixedUpdate();
    }

    public void OnDrawGizmos()
    {
        _onDrawGizmos();
    }

    public void OnDestroy()
    {
        _onFixedUpdate = null;
        _onDrawGizmos = null;
    }
}
