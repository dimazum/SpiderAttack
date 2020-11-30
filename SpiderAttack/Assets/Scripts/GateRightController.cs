using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.enums;
using UnityEngine;
using UnityEngine.UI;

public class GateRightController : MonoBehaviour, IListener
{
    private Animator _animator;
    public Slider gateHpSlider;
    public int gateRightHp = 10000;
    public GameObject door;
    private bool _isDestroyed;


    void Start()
    {
        _animator = GetComponent<Animator>();
        gateHpSlider.maxValue = gateRightHp;
        gateHpSlider.value = gateRightHp;

        EventManager.Instance.AddListener(EVENT_TYPE.SpiderMeleeAttackGate, this);
        EventManager.Instance.AddListener(EVENT_TYPE.SpiderWebReachedWall, this);
    }


    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.SpiderMeleeAttackGate:
                _animator.Play("gate_right_Hit");
                if (Param != null)
                {
                    ChangeGateHp((int)Param);
                }
                break;

            case EVENT_TYPE.SpiderWebReachedWall:
                _animator.Play("web");
                if (Param != null)
                {
                    ChangeGateHp((int)Param);
                }
                break;
        }
    }


    private void ChangeGateHp(int damage)
    {
        gateHpSlider.value -= damage;
        gateRightHp -= damage;

        if (gateRightHp <= 0 && !_isDestroyed)
        {
            _isDestroyed = true;
            EventManager.Instance.PostNotification(EVENT_TYPE.GateDestroy,this);
            door.SetActive(false);
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }

    }

}
