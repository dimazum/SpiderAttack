using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
        void Start()
        {
            roundText.text = $"Round: {GameStates.Instance.round+1}";
            EventManager.Instance.AddListener(EVENT_TYPE.GameOver, this);
            EventManager.Instance.AddListener(EVENT_TYPE.StartDay, this);
            EventManager.Instance.AddListener(EVENT_TYPE.StartNight, this);
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
    }
}