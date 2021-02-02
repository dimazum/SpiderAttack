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
    GateUp, //buy Pick Button
    SuperBomb, //buy Pick Button
    StartTeleport,
    FinishTeleport,
    SetLadder,
    GetResurs,
    HitResurs,
    

    OpenShop, //click Open Shop Button
    CloseShop, //click Close Shop
    StartDay, //start New Day
    StartNight, //start New Night
    ResetTime, // waiting for the night

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
    CheckIfCharInVillage,

    BallistaShot, //start
    BallistaIsCharged,//заряжена
    BallistaFireButtonUp,
    BallistaFireButtonDown,
    BallistaCharge,
    ArrowHitTarget,
    ArrowMissedTarget,

    //UI
    ChangeRating,
    ChangeMoney,
    Buy,
    NotEnoughMoney,

    //Thougts
    FullBackpack,
    NeedNextPick,



}
