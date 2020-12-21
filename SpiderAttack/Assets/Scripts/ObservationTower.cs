using UnityEngine;

namespace Assets.Scripts
{
    public class ObservationTower : MonoBehaviour
    {
        private BattlePlaceFirstFloor _battlePlaceFirstFloor;
        private CameraController cameraController;
        private float screen;
        private Camera _camera;

        void Awake()
        {
            _battlePlaceFirstFloor = FindObjectOfType<BattlePlaceFirstFloor>();
            _camera = Camera.main;
            cameraController = _camera.GetComponent<CameraController>();
        }

        public void OnTriggerEnter2D(Collider2D coll)
        {
            if (coll.CompareTag("player"))
            {
                cameraController.offset = new Vector3(22f, .7f, -10); //offset second floor
                _battlePlaceFirstFloor.Transition = 7;
            }
        }

        public void OnTriggerExit2D(Collider2D coll)
        {
            if (coll.CompareTag("player"))
            {
                _battlePlaceFirstFloor.Transition = 8;
                cameraController.offset = _battlePlaceFirstFloor.DefaultState;
            }
        }
    }
}
