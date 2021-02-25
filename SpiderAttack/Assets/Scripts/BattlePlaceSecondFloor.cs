using Assets.Scripts;
using UnityEngine;

public class BattlePlaceSecondFloor : MonoBehaviour
{
    private BattlePlaceFirstFloor _battlePlaceFirstFloor;
    private CameraController _cameraController;
    private float screen;
    public GameObject ballistaButtons;
    public GameObject trebuchetButtons;

    void Awake()
    {
        _battlePlaceFirstFloor = FindObjectOfType<BattlePlaceFirstFloor>();
        _cameraController = FindObjectOfType<CameraController>();
        screen = (float)Screen.width / 1000;
    }

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "player")
        {
            EventManager.Instance.PostNotification(EVENT_TYPE.CharacterEnterSecondFloor, this);
            _battlePlaceFirstFloor.Elapsed = 0;
            _battlePlaceFirstFloor.Transition = 5;
            _cameraController.index = screen;
            _cameraController.offset = new Vector3(5 , -.27f, -10);//offset second floor
            trebuchetButtons.SetActive(false);
            ballistaButtons.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "player")
        {
            EventManager.Instance.PostNotification(EVENT_TYPE.CharacterExitSecondFloor, this);
            _battlePlaceFirstFloor.Elapsed = 0;
            _battlePlaceFirstFloor.Transition = 6;
            _cameraController.offset = _battlePlaceFirstFloor.DefaultState;
            //ballistaButtons.SetActive(false);
            //trebuchetButtons.SetActive(true);
        }
    }
}
