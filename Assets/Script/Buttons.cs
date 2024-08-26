using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
    [SerializeField] AudioSource audioSource; [SerializeField] AudioClip menuSelection;
    bool isGoingToNewScene;
    float elapsedTime;
    [SerializeField] GameObject barImages;
    [SerializeField] Image imageToFill;
    [SerializeField] GameObject[] thingsToToggle;

    public void DoThis(int choose)
    {
        audioSource.PlayOneShot(menuSelection);
        if (choose == 1) InvokeRepeating(nameof(GoToNextScene), 0, Time.deltaTime);
        else if (choose == 2) foreach (GameObject x in thingsToToggle) x.SetActive(!x.activeSelf);
        else if (choose == 3)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Exit Play mode in the Editor
#else
        Application.Quit(); // Quit the application (works for standalone builds)
#endif
        }
        else if (choose == 4) foreach (GameObject x in thingsToToggle) x.SetActive(!x.activeSelf);
    }

    void GoToNextScene()
    {
        if (!isGoingToNewScene)
        {
            isGoingToNewScene = true;
            foreach (GameObject x in thingsToToggle) x.SetActive(false);
            barImages.SetActive(true);
        }
        elapsedTime += Time.deltaTime;
        imageToFill.fillAmount = elapsedTime / 3f;
        if (elapsedTime >= 3f) SceneManager.LoadScene(1);
    }
}

