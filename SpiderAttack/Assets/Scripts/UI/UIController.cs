using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UIController : MonoBehaviour, IListener
    {
        private WaitForSeconds _youWinTextDelay = new WaitForSeconds(2f);
        private Animator _animator;
        public GameObject panelGameOver;
        public GameObject timerText;
        public TextMeshProUGUI roundText;
        public TextMeshProUGUI winText;
        public TextMeshProUGUI raitingText;
        public GameObject spiderSmallImage;
        public Image trebSliderCharge; //call from treb
        public Image ballistaSliderCharge; //call from ballista
        public GameObject teleportPanel;
        public Slider teleportSlider;
        void Start()
        {
            roundText.text = $"Round: {GameStates.Instance.round+1}";
            raitingText.text = GameStates.Instance.rating.ToString();
            EventManager.Instance.AddListener(EVENT_TYPE.GameOver, this);
            EventManager.Instance.AddListener(EVENT_TYPE.StartDay, this);
            EventManager.Instance.AddListener(EVENT_TYPE.StartNight, this);
            EventManager.Instance.AddListener(EVENT_TYPE.CharacterEnterFirstFloor, this);
            EventManager.Instance.AddListener(EVENT_TYPE.CharacterExitFirstFloor, this);
            EventManager.Instance.AddListener(EVENT_TYPE.CharacterEnterSecondFloor, this);
            EventManager.Instance.AddListener(EVENT_TYPE.CharacterExitSecondFloor, this);
            EventManager.Instance.AddListener(EVENT_TYPE.StartTeleport, this);
            EventManager.Instance.AddListener(EVENT_TYPE.FinishTeleport, this);
            EventManager.Instance.AddListener(EVENT_TYPE.ChangeRating, this);
            _animator = GetComponent<Animator>();
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

                case EVENT_TYPE.StartTeleport:
                    {
                        if (Param == null) return;

                        teleportPanel.SetActive(true);
                        StartCoroutine(AnimateSliderOverTime((float)Param));
                        break;
                    }

                case EVENT_TYPE.FinishTeleport:
                    teleportPanel.SetActive(false);
                    break;

                case EVENT_TYPE.ChangeRating:
                    {
                        raitingText.text = GameStates.Instance.rating.ToString();
                        break;
                    }

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

        IEnumerator AnimateSliderOverTime(float seconds)
        {
            float animationTime = 0f;
            while (animationTime < seconds)
            {
                animationTime += Time.deltaTime;
                float lerpValue = animationTime / seconds;
                teleportSlider.value = Mathf.Lerp(0f, 1f, lerpValue);
                yield return null;
            }
        }
    }
}