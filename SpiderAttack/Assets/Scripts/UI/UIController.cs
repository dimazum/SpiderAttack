using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UIController : MonoBehaviour, IListener
    {
        public GameObject panelGameOver;
        public GameObject timerText;
        public TextMeshProUGUI roundText;
        public TextMeshProUGUI winText;
        public GameObject spiderSmallImage;
        private WaitForSeconds _youWinTextDelay = new WaitForSeconds(2f);
        private Animator _animator;
        public GameObject eventController;
        public Button buttonFire;
        public Color buttonFireColorCharged;
        public Color buttonFireColorUncharged;
        public Image trebSliderCharge;
        public Image ballistaSliderCharge;
        //public static Image sliderChargeStatic;
        //public static Color buttonFireColorChargedStatic;
        void Start()
        {
            roundText.text = $"Round: {GameStates.Instance.round+1}";
            EventManager.Instance.AddListener(EVENT_TYPE.GameOver, this);
            EventManager.Instance.AddListener(EVENT_TYPE.StartDay, this);
            EventManager.Instance.AddListener(EVENT_TYPE.StartNight, this);
            EventManager.Instance.AddListener(EVENT_TYPE.CharacterEnterFirstFloor, this);
            EventManager.Instance.AddListener(EVENT_TYPE.CharacterExitFirstFloor, this);
            EventManager.Instance.AddListener(EVENT_TYPE.CharacterEnterSecondFloor, this);
            EventManager.Instance.AddListener(EVENT_TYPE.CharacterExitSecondFloor, this);
            EventManager.Instance.AddListener(EVENT_TYPE.EnableAllButtons, this);
            EventManager.Instance.AddListener(EVENT_TYPE.DisableAllButtons, this);
            EventManager.Instance.AddListener(EVENT_TYPE.BallistaIsCharged, this);
            EventManager.Instance.AddListener(EVENT_TYPE.BallistaShot, this);
            _animator = GetComponent<Animator>();
            //buttonFire.GetComponent<Image>().color = buttonFireColorUncharged;

        }

        public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
        {
            switch (Event_Type)
            {
                case EVENT_TYPE.GameOver:
                    panelGameOver.SetActive(true);
                    break;

                case EVENT_TYPE.StartDay:
                    timerText.SetActive(true);
                    spiderSmallImage.SetActive(false);
                    roundText.text = $"Round: {GameStates.Instance.round+1}";

                    StartCoroutine(YouWinText());

                    break;

                case EVENT_TYPE.StartNight:
                    timerText.SetActive(false);
                    spiderSmallImage.SetActive(true);
                    break;

                case EVENT_TYPE.CharacterEnterFirstFloor:
                    _animator.Play("HideButtonsOnBattle");
                    break;

                case EVENT_TYPE.CharacterExitFirstFloor:
                    _animator.Play("ShowButtonsOnBattle");
                    break;

                case EVENT_TYPE.CharacterEnterSecondFloor:
                    _animator.Play("HideButtonsOnBattle");
                    break;

                case EVENT_TYPE.CharacterExitSecondFloor:
                    _animator.Play("ShowButtonsOnBattle");
                    break;
                //case EVENT_TYPE.EnableAllButtons:
                //    eventController.SetActive(true);
                //    break;
                //case EVENT_TYPE.DisableAllButtons:
                //    eventController.SetActive(false);
                //    break;
                case EVENT_TYPE.BallistaIsCharged:
                    buttonFire.GetComponent<Image>().color= buttonFireColorCharged;
 
                    break;
                case EVENT_TYPE.BallistaShot:
                    buttonFire.GetComponent<Image>().color = buttonFireColorUncharged;
                    break;

            }
        }

        public IEnumerator YouWinText()
        {
            winText.enabled = true;
            winText.text = $"Hooray!!!\n" +
                           $"Round: {GameStates.Instance.round + 1}";
            yield return _youWinTextDelay;
            winText.enabled = false;
        }

        //public static void SetButtonState()
        //{
        //    sliderChargeStatic.color = buttonFireColorChargedStatic;
        //}
    }
}