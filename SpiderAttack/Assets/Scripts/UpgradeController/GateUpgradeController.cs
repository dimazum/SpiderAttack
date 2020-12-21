using System.Linq;
using UnityEngine;

namespace Assets.Scripts.UpgradeController
{
    public class GateUpgradeController : MonoBehaviour, IListener
    {
        public int currentLvl;
        public GateSpriteLevel[] gateLevels;
        public SpriteRenderer topTowerFront;
        public SpriteRenderer middleTowerFront;
        public SpriteRenderer bottomTowerFront;
        public SpriteRenderer topTowerBack;
        public SpriteRenderer middleTowerBack;
        public SpriteRenderer bottomTowerBack;
        public SpriteRenderer door;

        public bool plusOneLvl; //for 2nd image of gate shop

        void Start()
        {
            EventManager.Instance.AddListener(EVENT_TYPE.GateUp, this);
            SetSprites(GameStates.Instance.GateLvl);
        }

        public void SetSprites(int gateLvl)
        {
            if (plusOneLvl)
                gateLvl +=1;
            door.sprite = gateLevels.ElementAtOrDefault(gateLvl).door;
            topTowerFront.sprite = gateLevels.ElementAtOrDefault(gateLvl).topTower;
            middleTowerFront.sprite = gateLevels.ElementAtOrDefault(gateLvl).middleTower;
            bottomTowerFront.sprite = gateLevels.ElementAtOrDefault(gateLvl).bottomTower;
            topTowerBack.sprite = gateLevels.ElementAtOrDefault(gateLvl).topTower;
            middleTowerBack.sprite = gateLevels.ElementAtOrDefault(gateLvl).middleTower;
            bottomTowerBack.sprite = gateLevels.ElementAtOrDefault(gateLvl).bottomTower;
        }

        public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
        {
            switch (Event_Type)
            {
                case EVENT_TYPE.GateUp:
                    SetSprites(GameStates.Instance.GateLvl);
                    break;
            }
        }
    }

    [System.Serializable]
    public struct GateSpriteLevel
    {
        public Sprite topTower;
        public Sprite middleTower;
        public Sprite bottomTower;
        public Sprite door;
    }
}
