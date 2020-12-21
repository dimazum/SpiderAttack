using UnityEngine;

public interface IListener
{
    void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null);
}

public enum EVENT_TYPE
{
    GameOver,
    EnableAllButtons,
    DisableAllButtons,
    BuyPick, //buy Pick Button
    PickUp, //upgrade Pick
    BuyGate, //buy Pick Button
    GateUp, //buy Pick Button

    OpenShop, //click Open Shop Button
    CloseShop, //click Close Shop
    StartDay, //Start New Day
    StartNight, //Start New Night

    //Trebuchet
    TrebFireButtonUp,
    TrebFireButtonDown,
    TrebSpoonUpPointerUp, 
    TrebSpoonUpPointerDown,
    TrebSpoonDownPointerUp,
    TrebSpoonDownPointerDown,
    TrebSpoonLimit,
    TrebShot,
    TrebCharge,
    TrebSetup,// end of spoon move

    //Spider
    SpiderMeleeAttackGate,
    SpiderRangeAttackGate,
    SpiderStartRangeAttack,
    SpiderSpitRangeAttack,// начало плевка
    SpiderWebReachedWall,
    SpiderHurt,
    SpiderDie,
    SpiderWebHitCharacter,
    SpiderWebHitMainHouse,
    SpiderMeleeHitCharacter,

    //Gate
    GateDestroy,

    //MainHouse,
    MainHouseDestroy,

    //Character
    CharacterEnterFirstFloor,
    CharacterExitFirstFloor,
    CharacterEnterSecondFloor,
    CharacterExitSecondFloor,
    CharInCity,

    BallistaShot, //start
    BallistaIsCharged,//заряжена
    BallistaFireButtonUp,
    BallistaFireButtonDown,
    BallistaCharge,



}
