using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Buttons
{
    public class FireTrebButton : Button
    {
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            EventManager.Instance.PostNotification(EVENT_TYPE.TrebFireButtonDown, this);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            EventManager.Instance.PostNotification(EVENT_TYPE.TrebFireButtonUp, this);
        }
    }
}
