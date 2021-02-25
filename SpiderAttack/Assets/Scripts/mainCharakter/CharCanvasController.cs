using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CharCanvasController : MonoBehaviour, IListener
{
    [SerializeField]
    private GameObject charThoughts;
    [SerializeField]
    private Sprite[] picks;
    [SerializeField]
    private Sprite _backpackSprite;
    [SerializeField]
    private GameObject _charCanvas;
    private Animator _canvasAnimator;
    [SerializeField]
    private Image _thoughtsImage;


    void Start()
    {
        _canvasAnimator = _charCanvas.GetComponent<Animator>();
        EventManager.Instance.AddListener(EVENT_TYPE.NeedNextBackpack, this);
        EventManager.Instance.AddListener(EVENT_TYPE.NeedNextPick, this);
        EventManager.Instance.AddListener(EVENT_TYPE.CharCanvasLeftArrow, this);
    }

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.NeedNextPick:
                {
                    if (Param == null) return;
                    var sprite = picks[(byte)Param];
                    SetThoughts(sprite);

                    break;
                }


            case EVENT_TYPE.NeedNextBackpack:
                {
                    SetThoughts(_backpackSprite);

                    break;
                }

            case EVENT_TYPE.CharCanvasLeftArrow:
                {
                    _canvasAnimator.Play("LeftArrow");

                    break;
                }
        }
    }

    private void SetThoughts(Sprite sprite)
    {
        _thoughtsImage.sprite = sprite;
        _canvasAnimator.Play("Thoughts");
    }
}
