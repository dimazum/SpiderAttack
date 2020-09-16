using UnityEngine;

public interface IListener
{
    void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null);
}

public enum EVENT_TYPE
{
    BuyPick,
    PickUp//вошел в пещеру

}
