using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DebugMenuScript : MonoBehaviour
{
    public Button setHighscoreButton;
    public Text newHighscoreText;
    public Text currentHighscoreText;
    public Button closeButton;
    public Button openButton;
    public GameObject debugMenuPanel;
    // Start is called before the first frame update
    void Start()
    {
        openButton.onClick.AddListener(() =>
        {
            debugMenuPanel.SetActive(true);
            updateMenu();
            Time.timeScale = 0;
        });
        closeButton.onClick.AddListener(() =>
        {
            debugMenuPanel.SetActive(false);
            Time.timeScale = 1;
        });
        setHighscoreButton.onClick.AddListener(updateHighscore);
    }

    void updateHighscore()
    {
        PlayerPrefs.SetInt("PlayerHighScore", Int32.Parse(newHighscoreText.text));
        updateMenu();
    }

    void updateMenu()
    {
        currentHighscoreText.text = PlayerPrefs.GetInt("PlayerHighScore", 0).ToString();
    }


}
