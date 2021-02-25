using Assets.Scripts.Models.Upgrates;
using UnityEngine;

namespace Assets.Scripts.UpgradeController
{
    public class BackpackUpgradeController : BaseUpgradeController
    {
        [SerializeField]
        private SpriteRenderer _characterBackpack;

       private void Start()
        {
            RenderMainImages(GameStates.BackpackLvl);
            RenderCharBackpack(GameStates.BackpackLvl);
            RenderPrice(GameStates.BackpackLvl);
            RenderAttributeText(GameStates.BackpackLvl);
        }

        public override void Upgrade()
        {
            GameStates.BackpackLvl++;
            RenderMainImages(GameStates.BackpackLvl);
            RenderCharBackpack(GameStates.BackpackLvl);
            RenderPrice(GameStates.BackpackLvl);
            RenderAttributeText(GameStates.BackpackLvl);
        }

        protected override void RenderPrice(int lvl)
        {
            if (GameStates.BackpackLvl < 4)
            {
                priceText.text = models.GetChild(lvl + 1).GetComponent<BackpackUpgradeModel>().price.ToString();
            }
        }

        protected override void RenderMainImages(int lvl)
        {
            firstShopImage.sprite = models.GetChild(lvl).GetComponent<BackpackUpgradeModel>().uiSprite;
            if (GameStates.BackpackLvl < 4)
            {
                secondShopImage.sprite = models.GetChild(lvl + 1).GetComponent<BackpackUpgradeModel>().uiSprite;
            }
            else
            {
                secondPanel.SetActive(false);
                firstPanel.localPosition = new Vector3(0, 18, 0);
                pricePanel.SetActive(false);
            }
        }

        private void RenderCharBackpack(int lvl)
        {
            _characterBackpack.sprite = models.GetChild(lvl).GetComponent<BackpackUpgradeModel>().charSprite;
        }

        void RenderAttributeText(int lvl)
        {
            firstAttributeText.text = models.GetChild(lvl).GetComponent<BackpackUpgradeModel>().capacity.ToString();
            if (GameStates.BackpackLvl < 4)
            {
                secondAttributeText.text = models.GetChild(lvl + 1).GetComponent<BackpackUpgradeModel>().capacity.ToString();
            }
        }
    }
}