using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.enums;
using UnityEngine;

public class BattlePlaceFirstFloor : MonoBehaviour, IListener
{
    private Animator animator;
    private Animator _mainUIAnimator;
    private CameraController cameraController;
    private float screen;
    public GameObject mainCamera;
    public GameObject mainUI;

    private Vector3 defaultState = new Vector3(0, 1, -10);


    public float duration = 1f;
    private float elapsed = 0.0f;
    private int transition ;

    void Awake()
    {
        _mainUIAnimator = mainUI.GetComponent<Animator>();
        cameraController = mainCamera.GetComponent<CameraController>();
        animator = GetComponent<Animator>();
        screen = (float)Screen.width / 1000;
    }

    void Start()
    {
        EventManager.Instance.AddListener(EVENT_TYPE.TrebShot, this);
        EventManager.Instance.AddListener(EVENT_TYPE.GameOver, this);
    }

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "player")
        {
            cameraController.index = screen;
            animator.Play("SecondBatlePlaceDisappear");
            _mainUIAnimator.Play("HideButtonsOnBattle");
            cameraController.offset = new Vector3(4f , .7f, -10);
            transition = 3;
            elapsed = 0;
        }
    }

    public void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "player")
        {
            animator.Play("SecondBatlePlaceDisappear0");
            _mainUIAnimator.Play("ShowButtonsOnBattle");
            cameraController.offset = defaultState;
            transition = 4;
            elapsed = 0;
        }
    }

    void LateUpdate()
    {
        //1st floor => treb shot
        if (transition == 1)
        {
            elapsed += Time.deltaTime / duration;
            Camera.main.orthographicSize = Mathf.Lerp(3.5f, 5, elapsed);
            mainCamera.GetComponent<CameraController>().offset.y = Mathf.Lerp(.7f, 2.5f, elapsed);
            if (elapsed > 1.0f)
            {
                elapsed = 0;
                transition = 2;
            }
        }
        // treb shot => 1st floor
        else if (transition == 2)
        {
            elapsed += Time.deltaTime / duration;
            Camera.main.orthographicSize = Mathf.Lerp(5, 3.5f, elapsed);
            mainCamera.GetComponent<CameraController>().offset.y = Mathf.Lerp(2.5f, .7f, elapsed);
        }
        // village => 1st floor
        else if (transition == 3)
        {
            elapsed += Time.deltaTime / duration;
            Camera.main.orthographicSize = Mathf.Lerp(2.5f, 3.5f, elapsed);
        }
        // 1st floor => village
        else if (transition == 4)
        {
            elapsed += Time.deltaTime / duration;
            Camera.main.orthographicSize = Mathf.Lerp(3.5f, 2.5f, elapsed);
        }

    }

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.TrebShot:
                elapsed = 0;
                transition = 1;
                //cameraController.offset = new Vector3(4f, 2.5f, -10);
                break;

            case EVENT_TYPE.GameOver:
                transition = 4;

                break;




        }
    }
}
