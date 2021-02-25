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
    SuperBomb,
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
    TrebSetup,
    BallHitTarget,

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
    CharMoveToTarget,
    CharCanvasLeftArrow,

    BallistaShot, //start
    BallistaIsCharged,//заряжена
    BallistaFireButtonUp,
    BallistaFireButtonDown,
    BallistaCharge,
    ArrowHitTarget,
    ArrowMissedTarget,

    //UI
    ChangeRating,
    RatingAdditionUp,
    MaxRatingAddition,
    ChangeMoney,
    Buy,
    NotEnoughMoney,

    //Thougts
    NeedNextPick,
    NeedNextBackpack,

    DynamiteHurtChar
}
