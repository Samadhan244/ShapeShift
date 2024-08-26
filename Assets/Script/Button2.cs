using UnityEngine;

public class Button2 : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject[] objectsToOpen;

    public void Open()
    {
        animator.Play("ButtonPressed");
        foreach (GameObject x in objectsToOpen) x.SetActive(true);
    }
}
