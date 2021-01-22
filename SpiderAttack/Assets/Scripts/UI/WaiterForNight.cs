using UnityEngine;

public class WaiterForNight : MonoBehaviour
{
    [SerializeField]
    private GameObject mainChar;
    [SerializeField]
    private GameObject mainHouseImage;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void InHouse()
    {
        _animator.Play("WaitingForTheNight");
        mainChar.SetActive(false);
        mainHouseImage.SetActive(true);
    }

    public void OutsideHouse()
    {
        mainChar.SetActive(true);
        mainHouseImage.SetActive(false);
        EventManager.Instance.PostNotification(EVENT_TYPE.ResetTime, this);
    }
}
