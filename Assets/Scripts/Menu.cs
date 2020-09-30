using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//written by Mandy
public class Menu : MonoBehaviour
{
    [SerializeField] private PlayerValues playerValues;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject startVideo;
    [SerializeField] private Selectable[] buttons;
    [SerializeField] private JoystickMode joystickMode;

    private AudioSource walkingAudio;
    private Animator menuAnim;
    private Animator videoAnim;
    private int selectedButtonIndex;
    private bool joystickPressedUp = true;

    private void Awake()
    {
        buttons[2].Select();
        Assert.IsNotNull(playerValues, "No reference to PlayerLives scriptable object.");

        menuAnim = GetComponentInChildren<Animator>();
        Assert.IsNotNull(menuAnim, "Failed to access to Animator component.");

        Assert.IsNotNull(startVideo, "No reference to StartVideo game Object.");

        videoAnim = startVideo.GetComponent<Animator>();
        Assert.IsNotNull(videoAnim, "Failed to access StartVideo's Animator");

        Assert.IsNotNull(startPanel, "No reference to StartPanel");

        Assert.IsNotNull(buttons, "No reference to Start button and Quit button.");

        walkingAudio = GetComponent<AudioSource>();
        Assert.IsNotNull(walkingAudio, "Failed to find Audio Source component.");

    }

    private void Start()
    {
        startVideo.SetActive(true);
        startPanel.SetActive(true);
        joystickMode.JoystickOn = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (selectedButtonIndex)
            {
                case 0:
                    KeyboardSetting();
                    break;
                case 1:
                    JoystickModeSetting();
                    break;
                case 2:
                    LoadGame();
                    break;
                case 3:
                    ExitGame();
                    break;
                default:
                    KeyboardSetting();
                    LoadGame();
                    break;
            }
        }
    }

    public void JoystickModeSetting() => joystickMode.JoystickOn = true;

    public void KeyboardSetting() => joystickMode.JoystickOn = false;
 

    public async void LoadGame()
    {
        playerValues.Lives = 3;
        menuAnim.SetTrigger("LeaveMenu");
        walkingAudio.Play();
        await Task.Delay(900);
        startPanel.SetActive(false);
        videoAnim.SetTrigger("Play");
        await Task.Delay(4000);
        startVideo.SetActive(false);
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
