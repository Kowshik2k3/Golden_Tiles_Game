/*
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject gameOverPanel;
    public TMP_Text scoreText;
    public TMP_Text finalScoreText;
    public TMP_Text levelText;

    void Start()
    {
        ShowMenu();
        if (scoreText != null) scoreText.text = "0";
    }

    public void ShowMenu()
    {
        if (menuPanel != null) menuPanel.SetActive(true);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    public void HideMenu()
    {
        if (menuPanel != null) menuPanel.SetActive(false);
    }

    public void UpdateScoreText(int s)
    {
        if (scoreText != null) scoreText.text = s.ToString();
    }

    public void ShowGameOver(int finalScore)
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        if (finalScoreText != null) finalScoreText.text = "Score: " + finalScore.ToString();
    }

    public void OnLevelUp(int newLevel)
    {
        if (levelText != null) levelText.text = "Level " + newLevel.ToString();
    }
    public void UpdateLevel(int level)
    {
        levelText.text = "Level : " + level;
    }
}
*/

using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject gameOverPanel;
    public TMP_Text scoreText;
    public TMP_Text finalScoreText;
    public TMP_Text levelText;

    void Start()
    {
        ShowMenu();
        if (scoreText != null) scoreText.text = "0";
    }

    public void ShowMenu()
    {
        if (menuPanel != null) menuPanel.SetActive(true);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    public void HideMenu()
    {
        if (menuPanel != null) menuPanel.SetActive(false);
    }

    public void UpdateScoreText(int s)
    {
        if (scoreText != null) scoreText.text = s.ToString();
    }

    public void ShowGameOver(int finalScore)
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        if (finalScoreText != null) finalScoreText.text = "Score: " + finalScore.ToString();
    }

    public void OnLevelUp(int newLevel)
    {
        if (levelText != null) levelText.text = "Level " + newLevel.ToString();
    }
    public void UpdateLevel(int level)
    {
        levelText.text = "Level : " + level;
    }
}
