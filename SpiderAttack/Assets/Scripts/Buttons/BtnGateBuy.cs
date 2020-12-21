using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BtnGateBuy : Button
{
    // Start is called before the first frame update
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        EventManager.Instance.PostNotification(EVENT_TYPE.BuyGate, this);
    }

}
