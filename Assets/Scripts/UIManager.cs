using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image[] livesImage;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private PlayerLives playerLives;
    [SerializeField] private Text scoreText;
    private void Awake()
    {
        Assert.IsNotNull(livesImage, "Images are not assigned.");
    }

    private void Start()
    {
        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);
        UpdateLivesImage(playerLives);

        scoreText.text = "SCORE  " + 00;
    }

    public void UpdateLivesImage(PlayerLives lives)
    {
        switch (lives.Lives)
        {
            case 0:
                foreach (Image item in livesImage)
                    item.enabled = false;
                break;
            case 1:
                livesImage[1].enabled = false;
                livesImage[2].enabled = false;
                break;
            case 2:
                livesImage[2].enabled = false;
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
    }

    public void EnablePausePanel()
    {
        pausePanel.SetActive(true);
    }

    public void DisablePausePanel()
    {
        pausePanel.SetActive(false);
    }

    public void UpdateScore(int score)
    {
        scoreText.text = "SCORE  " + score;
    }

    
}
