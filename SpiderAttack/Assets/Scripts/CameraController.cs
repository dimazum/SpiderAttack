using UnityEngine;

public class CameraController : MonoBehaviour, IListener
{
    private Camera _camera;
    private Animator _animator;
    public Transform player;
    public Transform mainHouse;
    public Vector3 offset;
    public Vector3 desiredPos;
    public Vector3 smoothPos;
    public float index;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _camera = Camera.main;
        EventManager.Instance.AddListener(EVENT_TYPE.StartNight, this);
        EventManager.Instance.AddListener(EVENT_TYPE.StartDay, this);
        EventManager.Instance.AddListener(EVENT_TYPE.GameOver, this);

        offset = GameStates.Instance.InCity ? new Vector3(0, 1.5f, -10) : offset = new Vector3(0, .6f, -10);
        transform.position = player.position + offset;
    }

    public void LateUpdate()
    {
        desiredPos = player.position + offset;
        if (transform.position != desiredPos)
        {
            smoothPos = Vector3.MoveTowards(transform.position, desiredPos, GameStates.Instance.smoothCameraSpeed * Time.deltaTime);
            transform.position = smoothPos;
        }
    }

    public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
    {
        switch (Event_Type)
        {
            case EVENT_TYPE.StartNight:
                _animator.SetTrigger("NightEnable");
                break;

            case EVENT_TYPE.StartDay:
                _animator.SetTrigger("DayEnable");
                break;

            case EVENT_TYPE.GameOver:
                if (GameStates.Instance.IsGameOver) return;
                offset = new Vector3(0, -.2f, -10);
                _camera.orthographicSize = 2.5f;
                player = mainHouse;
                GameStates.Instance.IsGameOver = true;
                break;

            //case EVENT_TYPE.CharInVillage:
            //    {
            //        offset = new Vector3(0, 1.5f, -10);
            //        break;
            //    }

            //case EVENT_TYPE.CharInCave:
            //    {
            //        offset = new Vector3(0, .6f, -10);
            //        break;
            //    }
        }
    }

}
