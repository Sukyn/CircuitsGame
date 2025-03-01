using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button playButton;
    public string SceneName;

    void Start()
    {
        playButton.onClick.AddListener(StartGame);
    }

    void StartGame()
    {
        SceneManager.LoadScene(SceneName);
    }
}
