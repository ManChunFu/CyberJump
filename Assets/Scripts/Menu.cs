using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private PlayerLives playerLives;

    private void Awake()
    {
        Assert.IsNotNull(playerLives, "No reference to PlayerLives scriptable object.");
    }
    public void LoadGame()
    {

        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
