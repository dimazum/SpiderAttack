using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FireBallistaButton : Button
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        EventManager.Instance.PostNotification(EVENT_TYPE.BallistaFireButtonDown, this);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        EventManager.Instance.PostNotification(EVENT_TYPE.BallistaFireButtonUp, this);
    }
}
