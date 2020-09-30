using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Written by Mandy

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform background;
    [SerializeField] private PlayerValues playerValues;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private Button[] buttons;
    [SerializeField] private JoystickMode joystick;

    private Player playerScript;
    private int? selectedButtonIndex = null;
    private bool joystickPressedUp = true;

    public GameObject Ground; //needs to be public so SpawnPlatform / SpawnMiddleground works

    public float maximumDeviationLeft = -5f; //default maximum -x axis for where platforms or enemies can spawn //Kevin's code
    public float maximumDeviationRight = 5f; //default maximum +x axis for where platforms or enemies can spawn //Kevin's code

    public bool BackgroundChanged = false;
    public bool NotFirstFloor = false;

    public bool IsRestarting;
    public bool TimeToDestroyPlatform = false;
    public bool JoystickMode = false;

    private void Awake()
    {
        buttons[0].Select();
        Assert.IsNotNull(player, "No reference to Player's Trasform.");

        Assert.IsNotNull(background, "No reference to Background's Transform.");

        Assert.IsNotNull(Ground, "No reference to Ground game object.");

        Assert.IsNotNull(playerValues, "No reference to PlayerLives scriptable object.");

        Assert.IsNotNull(uiManager, "No reference to UIManager script.");

        playerScript = player.GetComponent<Player>();
        Assert.IsNotNull(playerScript, "Failed to access Player script.");

        Assert.IsNotNull(joystick, "No reference to JoystickMode scriptable object.");
    }

    private void Start()
    {
        if (joystick.JoystickOn)
            JoystickMode = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button8))
        {
            uiManager.EnablePausePanel();
            Time.timeScale = 0;
        }

        if (player.position.y > 2f)
            BackgroundChanged = true;

        if (player.position.y < -4f)
            BackgroundChanged = false;

        if (BackgroundChanged == true)
        {
            MovingBackground();
            TimeToDestroyPlatform = true;
        }
    }

    public void AdjustParallexPosition()
    {
        foreach (Transform item in background)
        {
            BackgroundMoving backgrounMoving = item.GetComponent<BackgroundMoving>();

            if (backgrounMoving != null)
                backgrounMoving.Reset();
        }
        GroundFloor groundFloor = Ground.GetComponent<GroundFloor>();
        if (groundFloor != null)
            groundFloor.Reset();

    }
    private void MovingBackground()
    {
        Ground.transform.Translate(Vector2.down * (Time.deltaTime * 1.2f));

        foreach (Transform item in background)
        {
            BackgroundMoving backgroundMoving = item.GetComponent<BackgroundMoving>();
            if (backgroundMoving != null)
                backgroundMoving.MoveDown();
        }
        NotFirstFloor = true;
    }


    public void ResumeGame()
    {
        Time.timeScale = 1;
        uiManager.DisablePausePanel();
    }

    public void LoadGame()
    {
        IsRestarting = false;
        playerValues.Lives = 3;
        playerValues.Scores = 0;
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
