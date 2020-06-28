using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class gameState
{
    public static PlayerClass currentPlayer = ScriptableObject.CreateInstance<PlayerClass>();

    //флаг о том, что мы в магазине
    public static bool isInShop = false;
    //флаг о том, что мы в инвентаре
    public static bool isInInventory = false;
    //флаг о том, что мы смотрим карту
    public static bool isInMap = false;
    

    //разрешение экрана
    public static float screenHieght;
    public static float screenWidth;
    //кол-во места в верхнем инвентаре
    //public static int itemCounterAllUp;
    public static int maxItemCounterAllUp=5;
    //кол-во места в нижнем инвентаре
    //public static int itemCounterAllDown;
    //словарь ресурсов
    public static Dictionary<string, int> resursDictionary = new Dictionary<string, int>
    {
        { "blockAmethyst",1 },
        { "blockBlackopal",2 },
        { "blockBluegarnet",3 },
        { "blockCopper",4 },
        { "blockDemantoid",5 },
        { "blockDiamond",6 },
        { "blockEmerald",7 },
        { "blockGold",8 },
        { "blockLazurit",9 },
        { "blockMaaffe",10 },
        { "blockMusgravit",11 },
        { "blockNefrit",12 },
        { "blockPlatinum",13 },
        { "blockPudrettite",14 },
        { "blockRuby",15 },
        { "blockSapphire",16 },
        { "blockSilver",17 },
        { "blockGround",0 },
        { "blockTopaz",18 }
    };

    //массив спрайтов для дестроя
    public static Sprite[] destroySp = { Resources.Load<Sprite>("destroy1"), Resources.Load<Sprite>("destroy2"), Resources.Load<Sprite>("destroy1") };

}
