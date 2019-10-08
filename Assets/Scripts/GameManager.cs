using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform background;
    [SerializeField] private GameObject backgroundNightPrefab;
    [SerializeField] private Transform platform;
    [SerializeField] private GameObject ground;
    [SerializeField] private PlayerLives playerLives;
    [SerializeField] private UIManager uiManager;

    public bool BackgroundChanged = false;
    public bool NotFirstFloor = false;

    private void Awake()
    {
        Assert.IsNotNull(player, "No reference to Player's Trasform.");

        Assert.IsNotNull(background, "No reference to Background's Transform.");

        Assert.IsNotNull(backgroundNightPrefab, "No reference to BackgroundNight prefab.");

        Assert.IsNotNull(ground, "No reference to Ground game object.");

        Assert.IsNotNull(playerLives, "No reference to PlayerLives scriptable object.");

        Assert.IsNotNull(uiManager, "No reference to UIManager script.");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            uiManager.EnablePausePanel();
            Time.timeScale = 0;
        }

        if (player.position.y > 2f)
        {
            if (BackgroundChanged == false)
                SpawnBackgroundNight();
        }

        if (player.position.y < -4f)
            BackgroundChanged = false;

        if (BackgroundChanged == true)
            MovingBackgroundAndDestroyPlatform();


    }

    private void SpawnBackgroundNight()
    {
        GameObject bgNight = Instantiate(backgroundNightPrefab, new Vector2(0f, 10f), Quaternion.identity);
        BackgroundChanged = true;
        bgNight.transform.SetParent(background);
        NotFirstFloor = true;
    }

    public void AdjustParallexPosition()
    {
        foreach (Transform item in background)
        {
            BackgroundMoving bM = item.GetComponent<BackgroundMoving>();

            if (bM != null)
            {
                if (bM.MovingSpeed != 1)
                {
                    item.position = background.GetChild(1).position - (bM.DifferenceToTheFirst * 0.83f);
                }
            }
        }
    }
    private void MovingBackgroundAndDestroyPlatform()
    {
        ground.transform.Translate(Vector2.down * Time.deltaTime);

        foreach (Transform item in background)
        {
            BackgroundMoving bM = item.GetComponent<BackgroundMoving>();
            if (bM != null)
                bM.MoveDown();

            foreach (Transform o in platform)
            {
                if (o.position.y < -5f)
                    Destroy(o.gameObject);
            }
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        uiManager.DisablePausePanel();
    }

    public IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }
    public void LoadGame()
    {
        playerLives.Lives = 3;
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
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
