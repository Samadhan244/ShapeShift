using UnityEngine;

public class Hider : MonoBehaviour
{
    void Start()
    {
        InvokeRepeating(nameof(HideAndShow), 0, 1.5f);
    }

    void HideAndShow()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
