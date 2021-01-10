using UnityEngine;

public class GateLeft : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "player")
        {
            animator.Play("gate_left");
            GameStates.Instance.smoothCameraSpeed = 3;
        }
    }

    public void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "player")
        {
            animator.Play("gate_left0");
        }
    }
}
