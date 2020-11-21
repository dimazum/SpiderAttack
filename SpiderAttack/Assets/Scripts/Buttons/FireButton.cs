using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Buttons
{
    public class FireButton : Button
    {
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            EventManager.Instance.PostNotification(EVENT_TYPE.FireButtonDown, this);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            EventManager.Instance.PostNotification(EVENT_TYPE.FireButtonUp, this);
        }
    }
}
