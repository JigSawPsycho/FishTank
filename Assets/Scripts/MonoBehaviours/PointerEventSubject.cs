using UnityEngine;
using UnityEngine.EventSystems;

public class PointerEventSubject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public delegate void OnPointerEvent(PointerEventData pointerEventData);
    private OnPointerEvent _onPointerDown;
    private OnPointerEvent _onPointerUp;

    public void RegisterOnPointerDownListener(OnPointerEvent action)
    {
        _onPointerDown += action;
    }

    public void RegisterOnPointerUpListener(OnPointerEvent action)
    {
        _onPointerUp += action;
    }

    public void DeregisterOnPointerUpListener(OnPointerEvent action)
    {
        _onPointerUp -= action;
    }
    
    public void DeregisterOnPointerDownListener(OnPointerEvent action)
    {
        _onPointerDown -= action;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("pointerdown");
        _onPointerDown(eventData);
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("pointerup");
        _onPointerUp(eventData);
    }

    private void OnDestroy()
    {
        _onPointerUp = null;
        _onPointerDown = null;
    }
}
