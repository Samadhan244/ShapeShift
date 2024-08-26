using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Animator doorAnimator;
    [SerializeField] bool isActivated;

    public void Open()
    {
        if (!isActivated)
        {
            isActivated = true;
            animator.Play("ButtonPressed");
            doorAnimator.Play("Open");
        }
    }

    public void Close()
    {
        if (isActivated)
        {
            isActivated = false;
            animator.Play("ButtonUp");
            this.Wait(0.5f, ()=> doorAnimator.Play("Close"));
        }
    }
}
