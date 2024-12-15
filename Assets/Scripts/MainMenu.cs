using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadFirstScene()
    {
        SceneManager.LoadScene(1);

        PlayerPrefs.SetInt("intelligence", 30);
        PlayerPrefs.SetInt("completed_scenes", 0);
    }

    public void Exit()
    {
        Application.Quit();
    }
}