using System.Collections;
using System.Text;
using Assets.Scripts.Utils;
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
        [SerializeField]
        private Color _ratingAdditionTextColor;
        [SerializeField]
        private Color _ratingMaxAdditionTextColor;
        [SerializeField]
        private TextMeshProUGUI _ratingAdditionText;
        [SerializeField]
        private TextMeshProUGUI moneyText;
        public GameObject spiderSmallImage;
        public Image trebSliderCharge; //call from treb
        public Image ballistaSliderCharge; //call from ballista
        public GameObject teleportPanel;
        public Slider teleportSlider;
        private Coroutine AnimateSliderOverTimeCo;
        private WaitForSeconds scoredDelay = new WaitForSeconds(.05f);
        private int _oldMoneyValue;
        [SerializeField]
        private Animator _moneyAnimator;

        void Start()
        {
            roundText.text = $"{GameStates.Round + 1}";
            raitingText.text = GameStates.rating.ToString();
            _ratingAdditionText.text = "+ 0";
            moneyText.text = GameStates.Money.ToString();
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
            EventManager.Instance.AddListener(EVENT_TYPE.ChangeMoney, this);
            EventManager.Instance.AddListener(EVENT_TYPE.NotEnoughMoney, this);
            EventManager.Instance.AddListener(EVENT_TYPE.RatingAdditionUp, this);
            EventManager.Instance.AddListener(EVENT_TYPE.MaxRatingAddition, this);
            _animator = GetComponent<Animator>();
            _oldMoneyValue = GameStates.Money;
        }

        public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
        {
            switch (Event_Type)
            {
                case EVENT_TYPE.GameOver:
                    PanelsController.Instance.PanelsDeactivation(panelGameOver);
                    panelGameOver.SetActive(true);
                    break;

                case EVENT_TYPE.StartDay:
                    timerText.SetActive(true);
                    spiderSmallImage.SetActive(false);
                    roundText.text = $"{GameStates.Round + 1}";
                    StartCoroutine(YouWinText());

                    break;

                case EVENT_TYPE.StartNight:
                    timerText.SetActive(false);
                    spiderSmallImage.SetActive(true);
                    break;

                case EVENT_TYPE.CharacterEnterFirstFloor:
                    _animator.Play("HideButtonsOnBattle");
                    //_ratingAdditionText.rectTransform.localPosition = new Vector3(_ratingAdditionText.rectTransform.localPosition.x, _ratingAdditionText.rectTransform.localPosition.y, 0);
                    break;

                case EVENT_TYPE.CharacterExitFirstFloor:
                    _animator.Play("ShowButtonsOnBattle");
                    //_ratingAdditionText.rectTransform.localPosition = new Vector3(_ratingAdditionText.rectTransform.localPosition.x, _ratingAdditionText.rectTransform.localPosition.y, -100000);
                    break;

                case EVENT_TYPE.CharacterEnterSecondFloor:
                    _animator.Play("HideButtonsOnBattle");
                    //_ratingAdditionText.rectTransform.localPosition = new Vector3(_ratingAdditionText.rectTransform.localPosition.x, _ratingAdditionText.rectTransform.localPosition.y, 0);
                    break;

                case EVENT_TYPE.CharacterExitSecondFloor:
                    _animator.Play("ShowButtonsOnBattle");
                    //_ratingAdditionText.rectTransform.localPosition = new Vector3(_ratingAdditionText.rectTransform.localPosition.x, _ratingAdditionText.rectTransform.localPosition.y, -100000);
                    break;

                case EVENT_TYPE.StartTeleport:
                    {
                        if (Param == null) return;

                        teleportPanel.SetActive(true);
                        AnimateSliderOverTimeCo = StartCoroutine(AnimateSliderOverTime((float)Param));
                        break;
                    }

                case EVENT_TYPE.FinishTeleport:
                    teleportPanel.SetActive(false);
                    StopCoroutine(AnimateSliderOverTimeCo);
                    teleportSlider.value = 0;
                    break;

                case EVENT_TYPE.ChangeRating:
                    {
                        raitingText.text = GameStates.rating.ToString();
                        break;
                    }

                case EVENT_TYPE.RatingAdditionUp:
                {
                    if (Param == null) return;
                    _ratingAdditionText.text = $"+ {Param}";
                    _ratingAdditionText.color = _ratingAdditionTextColor;
                    break;
                }

                case EVENT_TYPE.MaxRatingAddition:
                {
                    _ratingAdditionText.color = _ratingMaxAdditionTextColor;
                    break;
                }

                case EVENT_TYPE.ChangeMoney:
                    {
                        // object test = Param;
                        //var obj = Cast(test, new { _money = 0, oldValue = 0 });
                        var currentMoney = (int)Param;
                        StartCoroutine(Scored(_oldMoneyValue, currentMoney));
                        _oldMoneyValue = currentMoney;
                        //moneyText.text = obj._money.ToString();
                        break;
                    }

                case EVENT_TYPE.NotEnoughMoney:
                    {
                        _moneyAnimator.Play("NotEnoughMoney");
                        break;
                    }
            }
        }

        //T Cast<T>(object obj, T type) { return (T)obj; }

        private IEnumerator Scored(int oldVal, int newVal)
        {
           if(oldVal > newVal)
           {
               moneyText.text = newVal.ToString();
           }
           else if(oldVal < newVal)
           {
                _moneyAnimator.Play("GettingMoney");
                while (oldVal < newVal)
                {
                    oldVal += 100;
                    StringBuilder moneyTextSB = new StringBuilder();
                    moneyTextSB.Append(oldVal.ToString());
                    moneyText.text = moneyTextSB.ToString();
                    yield return scoredDelay;
                }
            }          
        }

        public IEnumerator YouWinText()
        {
            winText.enabled = true;
            winText.text = $"Hooray!!!\n" +
                           $"Round: {GameStates.Round + 1}";
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