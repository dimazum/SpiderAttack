
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TrebSpoonDown : Button
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        EventManager.Instance.PostNotification(EVENT_TYPE.TrebSpoonDownPointerDown, this);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        EventManager.Instance.PostNotification(EVENT_TYPE.TrebSpoonDownPointerUp, this);
    }
}
