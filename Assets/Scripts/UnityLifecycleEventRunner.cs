using System;

public class UnityLifecycleEventRunner : IFixedUpdate, IDrawGizmos
{
    private Action _onFixedUpdate = delegate { };
    private Action _onUpdate = delegate { };
    private Action _onDrawGizmos = delegate { };

    public void RegisterFixedUpdate(IFixedUpdate eventListener)
    {
        _onFixedUpdate += eventListener.FixedUpdate;
    }

    public void RegisterUpdate(IUpdate eventListener)
    {
        _onUpdate += eventListener.Update;
    }

    public void RegisterDrawGizmos(IDrawGizmos eventListener)
    {
        _onDrawGizmos += eventListener.OnDrawGizmos;
    }
    
    public void DeregisterFixedUpdate(IFixedUpdate eventListener)
    {
        _onFixedUpdate -= eventListener.FixedUpdate;
    }

    public void DeregisterUpdate(IUpdate eventListener)
    {
        _onUpdate -= eventListener.Update;
    }


    public void DeregisterDrawGizmos(IDrawGizmos eventListener)
    {
        _onDrawGizmos -= eventListener.OnDrawGizmos;
    }

    public void FixedUpdate()
    {
        _onFixedUpdate();
    }

    public void Update()
    {
        _onUpdate();
    }

    public void OnDrawGizmos()
    {
        _onDrawGizmos();
    }

    public void OnDestroy()
    {
        _onFixedUpdate = null;
        _onUpdate = null;
        _onDrawGizmos = null;
    }
}
