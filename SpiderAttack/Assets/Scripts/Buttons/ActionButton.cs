using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : Button
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (!CharackterHelper.Instance.CheckIfCharIsInCave())
        {
            EventManager.Instance.PostNotification(EVENT_TYPE.CharCanvasLeftArrow, this);
            return;
        }
        base.OnPointerClick(eventData);
    }
}
