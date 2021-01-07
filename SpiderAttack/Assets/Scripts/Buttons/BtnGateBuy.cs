using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BtnGateBuy : Button
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        EventManager.Instance.PostNotification(EVENT_TYPE.BuyGate, this);
    }

}
