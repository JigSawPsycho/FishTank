using UnityEngine;

public class ColliderTriggerEventSubject : MonoBehaviour
{
    public delegate void ColliderTriggerEvent(Collider other);
    private ColliderTriggerEvent _onColliderEnter;

    public void RegisterOnTriggerEnterListener(ColliderTriggerEvent action)
    {
        _onColliderEnter += action;
    }

    public void DeregisterOnTriggerEnterListener(ColliderTriggerEvent action)
    {
        _onColliderEnter -= action;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("1");
        _onColliderEnter(other);
    }
}
