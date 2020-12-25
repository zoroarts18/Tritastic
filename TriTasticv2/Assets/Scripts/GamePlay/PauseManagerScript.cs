using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;


public class PauseManagerScript : MonoBehaviour
{
    public BackGroundMusicManagement BG;

    public GameObject PausedCanvas;
    public RectTransform pauseMenu;

    public Button PauseButton;
    public Button HomeButton;
    public Button ResumeButton;
    public Button SettingsButton;

    private Vector2 PausePanelStart;
    public GameObject SettingsPanel;

    void Start()
    {
        

        
        PausedCanvas.SetActive(true);

        GameObject.Find("Background Music").GetComponent<AudioSource>().enabled = true;
        PauseButton.onClick.AddListener(pauseGame);
        HomeButton.onClick.AddListener(backToHomeMenu);
        ResumeButton.onClick.AddListener(resumeGame);
    }
    public void pauseGame()
    {
        if(BG.muted == false)
        FindObjectOfType<AudioManager>().Play("Select Sound");
        
        
        PauseButton.GetComponent<Image>().enabled = false;
        
        pauseMenu.DOAnchorPos(Vector2.zero ,0.2f);
        
        Invoke("StopTime", 0.2f);
        
    }

    public void StopTime()
    {
        Time.timeScale = 0;
    }
    public void resumeGame()
    {
        if (BG.muted == false)
        FindObjectOfType<AudioManager>().Play("Select Sound");

        PauseButton.GetComponent<Image>().enabled = true;
        
        Time.timeScale = 1;
        pauseMenu.DOAnchorPos(new Vector2(-1000, 0),0.1f);

    }

    

    
    public void backToHomeMenu()
    {
        if(BG.muted == false)
        FindObjectOfType<AudioManager>().Play("Select Sound");

        PausedCanvas.SetActive(false);
        SceneManager.LoadScene("FollowFinger");
        Time.timeScale = 1;
        Physics2D.gravity = new Vector2(0, -9.81f);
    }
}



