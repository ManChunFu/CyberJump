using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

//Written by Mandy
public class UIManager : MonoBehaviour
{
    [SerializeField] private Image[] livesImage;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private PlayerValues playerValues;
    [SerializeField] private Text scoreText;
    [SerializeField] private GameManager gameMangaer;
    [SerializeField] private Text gameOverScoreText;

    public bool PauseActive;

    private void Awake()
    {
        Assert.IsNotNull(livesImage, "Images are not assigned.");

        Assert.IsNotNull(gameMangaer, "No reference to GameManager.");

        Assert.IsNotNull(gameOverScoreText, "No reference to GameOver ScoreText.");

    }

    private void Start()
    {

        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);
        UpdateLivesImage(playerValues.Lives);

        UpdateScore(playerValues.Scores);
    }

    public async void UpdateLivesImage(int lives)
    {
        switch (lives)
        {
            case 0:
                if (!gameMangaer.IsRestarting)
                {
                    livesImage[0].GetComponent<HeartDamage>().ShowLiveDamage();
                    await Task.Delay(600);
                    livesImage[0].enabled = false;
                }
                livesImage[1].enabled = false;
                livesImage[2].enabled = false;
                break;
            case 1:
                if (!gameMangaer.IsRestarting)
                {
                    livesImage[1].GetComponent<HeartDamage>().ShowLiveDamage();
                    await Task.Delay(600);
                    livesImage[1].enabled = false;
                }
                livesImage[2].enabled = false;
                break;
            case 2:
                if (!gameMangaer.IsRestarting)
                {
                    livesImage[2].GetComponent<HeartDamage>().ShowLiveDamage();
                    await Task.Delay(600);
                    livesImage[2].enabled = false;
                }
                break;
            case 3:
                foreach (Image item in livesImage)
                    item.enabled = true;
                break;
            default:
                break;
        }

    }

   
    public void EnableGameOverPanel()
    {
        gameOverPanel.SetActive(true);
        gameOverScoreText.text = "Your score: " + playerValues.Scores;
        Time.timeScale = 0;
    }

    public void EnablePausePanel()
    {
        PauseActive = true;
        pausePanel.SetActive(true);
    }

    public void DisablePausePanel()
    {
        PauseActive = false;
        pausePanel.SetActive(false);
    }

    public void UpdateScore(int score)
    {
        scoreText.text = "SCORE  " + score.ToString().PadLeft(2, '0');
    }


}
