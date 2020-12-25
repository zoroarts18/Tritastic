using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject deadParticles;
    public BackGroundMusicManagement BG;
    public ControlManagerScript CS;
   
    public bool GameIsOver = false;
    public static bool gameOver = false;
    public GameObject gameOverCanvas;
    public Button retryButton;    
    public float slowness = 10f;
    public Text highscoreText;
    public Text scoreText;
    public UIManager uiManager;
    public GameObject UICanvas;

    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        uiManager = GameObject.FindWithTag("UIManager").GetComponent<UIManager>();
        Time.timeScale = 1;
        retryButton.onClick.AddListener(retryGame);
        
        
    }   
    public void retryGame()
    {
        UICanvas.SetActive(true);

        if(BG.muted == false)
        FindObjectOfType<AudioManager>().Play("Select Sound");

        GameObject.Find("Background Music").GetComponent<AudioSource>().enabled = true;
        gameOverCanvas.SetActive(false);
        SceneManager.LoadScene("FollowFinger");
        Time.timeScale = 1;
    }
    
    
    
    public void EndGame()
    {
        GameIsOver = true;
        if(CS.GameMode == 0)
        {
            highscoreText.text = PlayerPrefs.GetInt("PlayerHighScore", 0).ToString();
        }
        if (CS.GameMode == 1)
        {
            highscoreText.text = PlayerPrefs.GetInt("RingsHighScore", 0).ToString();
        }
        if (CS.GameMode == 2)
        {
            highscoreText.text = PlayerPrefs.GetInt("ShootHighScore", 0).ToString();
        }

        scoreText.text = uiManager.score.ToString();
        StartCoroutine(RestartLevel());
        //Player.GetComponent<FollowFingerScript>().PlayerParticles.SetActive(false);

        
        
    }

    
    IEnumerator RestartLevel()
    {
        
        Time.timeScale = 1f / slowness;
        Time.fixedDeltaTime = Time.fixedDeltaTime / slowness;
        yield return new WaitForSeconds(1f / slowness);
        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.fixedDeltaTime / slowness;
        //Time.timeScale = 0;


        //Destroy(GameObject.FindWithTag("Particles"));
        gameOverCanvas.SetActive(true);
        
        UICanvas.SetActive(false);
    }      
}
