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
    [SerializeField] private Transform space;

    private Player playerScript;
    private int? selectedButtonIndex = null;
    private bool joystickPressedUp = true;
    private float maxHeight = 2.0f;
    private float minHeight = -4.0f;

    public GameObject Ground; //needs to be public so SpawnPlatform / SpawnMiddleground works

    public float maximumDeviationLeft = -5f; //default maximum -x axis for where platforms or enemies can spawn //Kevin's code
    public float maximumDeviationRight = 5f; //default maximum +x axis for where platforms or enemies can spawn //Kevin's code

    public bool BackgroundChanged = false;
    public bool NotFirstFloor = false;

    public bool IsRestarting;
    public bool TimeToDestroyPlatform = false;
    public bool JoystickMode = false;

    private bool isFallingToDie = false;
    private const float maxDistanceToGround = 200.0f;
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

        Assert.IsNotNull(space, "No reference to Space.");
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

        if (player.position.y > maxHeight)
            BackgroundChanged = true;

        if (player.position.y < minHeight || isFallingToDie)
            BackgroundChanged = false;

        if (BackgroundChanged)
        {
            MovingBackground();
            TimeToDestroyPlatform = true;
        }
    }

    public void AdjustParallexPosition()
    {
        BackgroundChanged = false;
        isFallingToDie = true;
        // turn off background spotlight
        background.GetChild(6).gameObject.SetActive(false); 

        float playerDistanceToGround = Vector2.Distance(player.transform.position, Ground.transform.position);
        
        if (playerDistanceToGround < maxDistanceToGround)
        {
            CallBackgroundReset(playerDistanceToGround);
        }
        else
        {
            CallSpaceReset(playerDistanceToGround);
            CallBackgroundReset(playerDistanceToGround);
        }
    }
    private void MovingBackground()
    {
        Ground.GetComponent<GroundFloor>().MoveDown();

        foreach (Transform item in background)
        {
            BackgroundMoving backgroundMoving = item.GetComponent<BackgroundMoving>();
            if (backgroundMoving != null)
                backgroundMoving.MoveDown();
        }
        NotFirstFloor = true;
    }

    private void CallBackgroundReset(float playerDistance)
    {
        foreach (Transform item in background)
        {
            BackgroundMoving backgrounMoving = item.GetComponent<BackgroundMoving>();

            if (backgrounMoving != null)
            {
                backgrounMoving.SetResetSpeed(Vector2.Distance(backgrounMoving.transform.position, Ground.transform.position), playerDistance);
                backgrounMoving.EnableReset(true);
            }
        }
    }

    private void CallSpaceReset(float playerDistance)
    {
        foreach (Transform item in space)
        {
            ScrollingBackground scrollingBG = item.GetComponent<ScrollingBackground>();
            if (scrollingBG != null)
            {
                scrollingBG.SetResetSpeed(Vector2.Distance(scrollingBG.transform.position, Ground.transform.position), playerDistance);
                scrollingBG.EnableReset(true);
            }
        }
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
