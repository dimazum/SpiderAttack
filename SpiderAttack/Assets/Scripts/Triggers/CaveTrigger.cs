using UnityEngine;

public class CaveTrigger : MonoBehaviour
{
    private CameraController _cameraController;
    private MoveController _moveController;

    private void Start()
    {
        _moveController = FindObjectOfType<MoveController>();
        _cameraController = FindObjectOfType<CameraController>();
        SetCameraOffset();
    }

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "player")
        { 
            GameStates.Instance.smoothCameraSpeed = 3;
            EventManager.Instance.PostNotification(EVENT_TYPE.CheckIfCharInVillage, this);

            SetCameraOffset();
        }
    }

    public void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "player")
        {
            GameStates.Instance.smoothCameraSpeed = 5;
            EventManager.Instance.PostNotification(EVENT_TYPE.CheckIfCharInVillage, this);

            SetCameraOffset();
        }
    }

    private void SetCameraOffset()
    {
        var isNearByCave = _moveController.CheckIfNearByCave();
        if (isNearByCave)
        {
            _moveController.flashlight.SetActive(false);
            _cameraController.offset = new Vector3(0, 1.5f, -10);
        }
        else if (!isNearByCave)
        {
            _cameraController.offset = new Vector3(0, .6f, -10);
            _moveController.flashlight.SetActive(true);
        }
    }

}
