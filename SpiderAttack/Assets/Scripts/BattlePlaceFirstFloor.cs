using UnityEngine;

namespace Assets.Scripts
{
    public class BattlePlaceFirstFloor : MonoBehaviour, IListener
    {
        private Animator animator;
        private CameraController cameraController;
        //private float screen;
        public Vector3 DefaultState { get; set; } = new Vector3(0, 1.5f, -10);
        private Camera _camera;     
        private int _transition;
        public GameObject ballistaButtons;
        public GameObject trebuchetButtons;
        public float duration = 1f;
        public float Elapsed { get; set; } = 0.0f;
        public int Transition
        {
            get => _transition;
            set
            {
                Elapsed = 0;
                _transition = value;
            }
        }

        void Awake()
        {
            _camera = Camera.main;
            cameraController = _camera.GetComponent<CameraController>();
            animator = GetComponent<Animator>();
            //screen = (float)Screen.width / 1000;
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
                EventManager.Instance.PostNotification(EVENT_TYPE.CharacterEnterFirstFloor, this);
                //cameraController.index = screen;
                animator.Play("SecondBatlePlaceDisappear");
                cameraController.offset = new Vector3(5f , 1.5f, -10); //offset first floor
                Transition = 3;
                Elapsed = 0;
                trebuchetButtons.SetActive(true);
                ballistaButtons.SetActive(false);
            }
        }

        public void OnTriggerExit2D(Collider2D coll)
        {
            if (coll.tag == "player")
            {
                EventManager.Instance.PostNotification(EVENT_TYPE.CharacterExitFirstFloor, this);
                animator.Play("SecondBatlePlaceDisappear0"); 
                cameraController.offset = DefaultState;
                Transition = 4;
                Elapsed = 0;
            }
        }

        void LateUpdate()
        {
            if (Elapsed > 1.1f)
            {
                if (GameStates.Instance.smoothCameraSpeed != 5.0f)
                {
                    GameStates.Instance.smoothCameraSpeed = 5;
                }
                return;
            }
            //1st floor => treb shot
            if (Transition == 1)
            {
                Elapsed += Time.deltaTime / duration;
                _camera.orthographicSize = Mathf.Lerp(4.5f, 5, Elapsed);
                _camera.GetComponent<CameraController>().offset.y = Mathf.Lerp(1.5f, 3f, Elapsed);
                if (Elapsed > 1.0f)
                {
                    Elapsed = 0;
                    Transition = 2;
                }
            }
            // treb shot => 1st floor
            else if (Transition == 2)
            {
                Elapsed += Time.deltaTime / duration;
                _camera.orthographicSize = Mathf.Lerp(5, 4.5f, Elapsed);
                _camera.gameObject.GetComponent<CameraController>().offset.y = Mathf.Lerp(3f, 1.5f, Elapsed);
            }
            // village => 1st floor
            else if (Transition == 3)
            {
                Elapsed += Time.deltaTime / duration;
                _camera.orthographicSize = Mathf.Lerp(2.5f, 4.5f, Elapsed);
            }
            // 1st floor => village
            else if (Transition == 4)
            {
                Elapsed += Time.deltaTime / duration;
                _camera.orthographicSize = Mathf.Lerp(4.5f, 2.5f, Elapsed);
            }

            // village => 2st floor
            else if (Transition == 5)
            {
                Elapsed += Time.deltaTime / duration;
                _camera.orthographicSize = Mathf.Lerp(2.5f, 5f, Elapsed);
            }
            // 2st floor =>  village 
            else if (Transition == 6)
            {
                Elapsed += Time.deltaTime / duration;
                _camera.orthographicSize = Mathf.Lerp(5f, 2.5f, Elapsed);
            }

            // village => observation tower   
            else if (Transition == 7)
            {
                Elapsed += Time.deltaTime / duration;
                _camera.orthographicSize = Mathf.Lerp(2.5f, 14f, Elapsed);
                GameStates.Instance.smoothCameraSpeed = 15;
            }

            // observation tower => village
            else if (Transition == 8)
            {
                Elapsed += Time.deltaTime / duration;
                _camera.orthographicSize = Mathf.Lerp(14f, 2.5f, Elapsed);
                GameStates.Instance.smoothCameraSpeed = 15;
            }
        }

        public void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null)
        {
            switch (Event_Type)
            {
                case EVENT_TYPE.TrebShot:
                    {
                        Elapsed = 0;
                        Transition = 1;
                        break;
                    }

                case EVENT_TYPE.GameOver:
                    {
                        Transition = 4;
                        break;
                    }
            }
        }
    }
}
