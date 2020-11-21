using UnityEngine;

public interface IListener
{
    void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null);
}

public enum EVENT_TYPE
{
    BuyPick, //buy Pick Button
    PickUp, //upgrade Pick
    OpenShop, //click Open Shop Button
    CloseShop, //click Close Shop
    StartDay, //Start New Day
    StartNight, //Start New Night
    FireButtonUp,
    FireButtonDown,
    //Trebuchet
    TrebSpoonUpPointerUp, 
    TrebSpoonUpPointerDown,
    TrebSpoonDownPointerUp,
    TrebSpoonDownPointerDown,
    TrebSpoonLimit,
    TrebShot,// end of spoon move

    //Spider
    SpiderMeleeAttackHitGate,
    SpiderRangeAttackHitGate,
    SpiderHurt,
    SpiderDie

 

}
