using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class GameManager : MonoBehaviour
{
    public PlayerProfile playerProfile;

    public int score;
    public float startSpeed = 5;
    public float speedStep = 1;
    public float scoreBetweenSteps = 8;
    public float lastStepScore = 0;
    public float savedSpeed = 0;

    //Alle floats für die Zeiten zwischen den Spawns, je kleiner die Zahl desto schwerer, weil weniger Zeit zwischen Spawn
    public float[] timeBetweenSpawnList =
    {
        2.5f,
        2f,
        1.5f,
        1f,
    };

    public int GameMode;


    [Header("Components & Scripts to assign")]
    public GameObject Player;
    public BlockSpawner blockSpawner;
    public GameObject spawnPointLeft, spawnPointMid, spawnPointRight;

    public BackGroundMusicManagement BG;
    public UIMovement ui;
    public GameObject deadParticles;
    public AudioManager audio;

    //------------------------------------------------------------------------------------------------------
    [Header("GameOver & Restart")]
    public float slowness = 10f;
    public bool GameIsOver = false;
    public static bool gameOver = false;
    public bool GameIsPlayed;

    //------------------------------------------------------------------------------------------------------
    [Header("UI")]
    //UI Buttons:
    public Button retryButton;
    public Button pauseButton;
    public Button resumeButton;
    public Button backToHomeButton;

    public Button StartGameButton;
    public Button openShopButton;
    public Button closeShopButton;

    public Button openMoreTricoinsPanelBtn;
    public Button closeMoreTricoinsPanelBtn;


    public Button openMenuButton;
    public Button closeMenuButton;

    public Button openProfileButton;
    public Button closeProfileButton;

    public Button openSettingsButton;
    public Button closeSettingsButton;

    //Settings UI
    public Dropdown QualityDropDown;
    public Button muteSoundButton;

    public Button switchGameModeToRight;
    public Button switchGameModeToLeft;

    //UI Panels:
    
    public GameObject gameOverCanvas;

    //UI Texts:
    public Text highscoreTextStartMenu;
    public Text scoreTextGameOver;
    public Text TricoinsTextGameOver;

    //Profile UI:
    public Text ProfileArcadeHighScoreTxt;
    public Text ProfileRingsHighScoreTxt;
    public Text ProfileShootHighScoreTxt;
    
    public Text ProfileArcadeMatchesTxt;
    public Text ProfileArcadeAvoidsTxt;
    public Text ProfileArcadeItemTxt;

    public Text ProfileRingsMatchesTxt;
    public Text ProfileRingsCatchedTxt;

    public Text ProfileShootMatchesTxt;
    public Text ProfileShootKillsTxt;

    //UI
    public Text HighScoreTxtGameOver;
    public Text GameModeTxt;

    public Text ScoreTextIngame;

    public SpriteRenderer BackGroundImg;
    //Die Sprites müssen in der Reihenfolge sein wie die Werte im Enum Background 
    public Sprite[] backGroundSprites;
    public Sprite[] backGroundSpritesUI;
    public Image[] backGroundsToChange;

    public Button[] ButtonsToChangeDesign;
    public Color[] colors;

    void Start()
    {
        Time.timeScale = 1;
        retryButton.onClick.AddListener(retryGame);
        backToHomeButton.onClick.AddListener(backToHomeMenu);
        resumeButton.onClick.AddListener(resumeGame);
        pauseButton.onClick.AddListener(pauseGame);
        openShopButton.onClick.AddListener(openShop);
        closeShopButton.onClick.AddListener(closeShop);
        openSettingsButton.onClick.AddListener(openSettings);
        closeSettingsButton.onClick.AddListener(closeSettings);
        StartGameButton.onClick.AddListener(StartGame);

        openMenuButton.onClick.AddListener(openMenu);
        closeMenuButton.onClick.AddListener(closeMenu);
        openSettingsButton.onClick.AddListener(openSettings);
        closeSettingsButton.onClick.AddListener(closeSettings);
        openProfileButton.onClick.AddListener(openProfile);
        closeProfileButton.onClick.AddListener(closeProfile);
        switchGameModeToLeft.onClick.AddListener(delegate { switchGame(-1); });
        switchGameModeToRight.onClick.AddListener(delegate { switchGame(1); });
        openMoreTricoinsPanelBtn.onClick.AddListener(openMoreTricoinsPanel);
        closeMoreTricoinsPanelBtn.onClick.AddListener(closeMoreTricoinsPanel);

        blockSpawner.currentSpeed = startSpeed;
        playerProfile = SaveManager.Load();

        GetQualityFromSaveFile();
        GetGameModeFromSaveFile();
        GetBackGroundFromSaveFile();

        GameIsPlayed = false;
    }

    public void GetQualityFromSaveFile()
    {
        switch (playerProfile.Quality)
        {
            case 0:
                {
                    Camera.main.GetComponent<PostProcessVolume>().enabled = true;
                    QualityDropDown.value = 0;
                    return;
                }

            case 1:
                {
                    Camera.main.GetComponent<PostProcessVolume>().enabled = false;
                    QualityDropDown.value = 1;
                    return;
                }
        }
    }

    public void GetBackGroundFromSaveFile()
    {
        BackGroundImg.sprite = backGroundSprites[(int)playerProfile.currentBg];

        foreach (var item in backGroundsToChange)
        {
            item.sprite = backGroundSpritesUI[(int)playerProfile.currentBg];
        }

        foreach (var item in ButtonsToChangeDesign)
        {
            item.GetComponent<Image>().color = colors[(int)playerProfile.currentBg];
        }
    }
    public void GetGameModeFromSaveFile()
    {
        switch (playerProfile.GameMode)
        {
            case 0:
                {
                    GameModeTxt.text = "Arcade";
                    GameMode = 0;
                    highscoreTextStartMenu.text = playerProfile.HighScoreArcade.ToString();
                    //currentHighScoreTxt.text = PlayerPrefs.GetInt("PlayerHighScore", 0).ToString();
                    switchGameModeToLeft.gameObject.SetActive(false);
                    switchGameModeToRight.gameObject.SetActive(true);
                    spawnPointLeft.transform.position = new Vector2(-1.98f, 6.53f);
                    spawnPointRight.transform.position = new Vector2(2.02f, 6.53f);
                    spawnPointMid.transform.position = new Vector2(0.04f, 8.68f);
                    return;
                }
            case 1:
                {
                    GameModeTxt.text = "Rings";
                    GameMode = 1;
                    //Default Values noch kein Binary Formatter da
                    highscoreTextStartMenu.text = playerProfile.HighScoreRings.ToString();
                    //currentHighScoreTxt.text = PlayerPrefs.GetInt("RingsHighScore", 0).ToString();
                    switchGameModeToLeft.gameObject.SetActive(true);
                    switchGameModeToRight.gameObject.SetActive(true);
                    spawnPointLeft.transform.position = new Vector2(-1.98f, 6.53f);
                    spawnPointRight.transform.position = new Vector2(2.02f, 6.53f);
                    spawnPointMid.transform.position = new Vector2(0.04f, 8.68f);
                    return;
                }

            case 2:
                {
                    GameModeTxt.text = "Shoot";
                    GameMode = 2;
                    highscoreTextStartMenu.text = playerProfile.HighScoreShoot.ToString();
                    //currentHighScoreTxt.text = PlayerPrefs.GetInt("ShootHighScore", 0).ToString();
                    //Style Entscheidung: Wenn man den Spawner in der Mitte höher macht, dann siet es nicer aus, als wären die Gegner in einer Formation:
                    switchGameModeToLeft.gameObject.SetActive(true);
                    switchGameModeToRight.gameObject.SetActive(false);
                    spawnPointLeft.transform.position = new Vector2(-1.98f, 8.68f);
                    spawnPointMid.transform.position = new Vector2(0.04f, 6.53f);
                    spawnPointRight.transform.position = new Vector2(2.02f, 8.68f);
                    return;
                }
        }
    }
    //SFX:
    public void selectSound()
    {
        if (BG.muted == false)
            audio.Play("Select Sound");
    }

    #region pause & unpause
    public void pauseGame()
    {
        if(!gameOverCanvas.activeSelf)
        {
            selectSound();
            pauseButton.gameObject.SetActive(false);
            ui.movePauseMenuIn();
            Invoke("stopTime", 0.25f);
        }
        
    }

    public void stopTime()
    {
        Time.timeScale = 0;
    }

    public void resumeGame()
    {
        Time.timeScale = 1;
        selectSound();
        pauseButton.gameObject.SetActive(true);
        ui.movePauseMenuOut();
    }

    public void backToHomeMenu()
    {
        selectSound();
        SceneManager.LoadScene("FollowFinger");
        Physics2D.gravity = new Vector2(0, -9.81f);
    }

    #endregion

    #region StartMenu Functions
    
    public void StartGame()
    {
        playerProfile = SaveManager.Load();
        
        if(GameMode == 0)
        {
            Debug.Log("GameMode 0");
            playerProfile.ArcadeMatchesPlayed++;
        }
        else if(GameMode == 1)
        {
            Debug.Log("GameMode 1");
            playerProfile.RingsMatchesPlayed++;
        }
        else if (GameMode == 2)
        {
            Debug.Log("GameMode 2");
            playerProfile.ShootMatchesPlayed++;
        }
        GameIsPlayed = true;
        selectSound();
        Player.GetComponent<FollowFingerScript>().ActivateTrails();
        ui.MoveStartMenuUIOut();
        blockSpawner.GetProfile();
    }

    //--------------------------------------------------------Shop:---------------------------------------------------------------------
    public void openShop()
    {
        selectSound();
        ui.moveShopIn();
    }
    public void closeShop()
    {
        selectSound();
        ui.moveShopOut();
    }

    public void openMoreTricoinsPanel()
    {
        selectSound();
        ui.openMoreTricoinsPanel();
    }
    public void closeMoreTricoinsPanel()
    {
        selectSound();
        ui.closeMoreTricoinsPanel();
    }

    //---------------------------------------------------------Menu:-------------------------------------------------------------------------
    public void openMenu()
    {
        selectSound();
        ui.openMenu();

        ProfileArcadeHighScoreTxt.text = playerProfile.HighScoreArcade.ToString();
        ProfileRingsHighScoreTxt.text = playerProfile.HighScoreRings.ToString();
        ProfileShootHighScoreTxt.text = playerProfile.HighScoreShoot.ToString();
        ProfileArcadeMatchesTxt.text = playerProfile.ArcadeMatchesPlayed.ToString();
        ProfileArcadeItemTxt.text = playerProfile.PowerUpsCollected.ToString();
        ProfileArcadeAvoidsTxt.text = playerProfile.ArcadeBlocksAvoided.ToString();
        ProfileRingsMatchesTxt.text = playerProfile.RingsMatchesPlayed.ToString();
        ProfileRingsCatchedTxt.text = playerProfile.RingsCatched.ToString();
        ProfileShootMatchesTxt.text = playerProfile.ShootMatchesPlayed.ToString();
        ProfileShootKillsTxt.text = playerProfile.ShootKills.ToString();
    }
    public void closeMenu()
    {
        selectSound();
        ui.closeMenu();
    }

    //---------------------------------------------------------Profile:-------------------------------------------------------------------------
    public void openProfile()
    {
        selectSound();
        ui.openProfile();
    }
    public void closeProfile()
    {
        selectSound();
        ui.closeProfile();
    }

    //---------------------------------------------------------Settings:-------------------------------------------------------------------------
    public void openSettings()
    {
        selectSound();
        ui.openSettings();
    }
    
    public void closeSettings()
    {
        selectSound();
        ui.closeSettings();
    }


    public void switchGame(int next)
    {
        selectSound();
        switch (next)
        {
            case -1:
                {
                    GameMode--;
                    playerProfile.GameMode = GameMode;
                    SaveManager.Save();
                    return;
                }
            case 1:
                {
                    GameMode++;
                    playerProfile.GameMode = GameMode;
                    SaveManager.Save();
                    return;
                }

        }
    }

    private void Update()
    {

        scoreTextGameOver.text = score.ToString();
        int difficulty = getDifficultyForScore(score);

        if (GameIsOver == false && Player.GetComponent<FollowFingerScript>().isDashing == false)
        {
            if (score - lastStepScore > scoreBetweenSteps)
            {
                lastStepScore = score;
                blockSpawner.setSpeed(blockSpawner.currentSpeed + speedStep);
            }

            if (difficulty >= timeBetweenSpawnList.Length)
            {
                blockSpawner.timeBetweenSpawn = timeBetweenSpawnList[timeBetweenSpawnList.Length - 1];
            }
            else
            {
                blockSpawner.timeBetweenSpawn = timeBetweenSpawnList[difficulty];
            }
        }


        switch (GameMode)
        {
            case 0:
                {
                    //Arcade---------------------------------------------------------------------------------------------
                    GameModeTxt.text = "Arcade";
                    highscoreTextStartMenu.text = playerProfile.HighScoreArcade.ToString();
                    //SaveManager.Save(playerProfile);
                    switchGameModeToLeft.gameObject.SetActive(false);
                    switchGameModeToRight.gameObject.SetActive(true);
                    return;
                }
            case 1:
                {
                    //Rings---------------------------------------------------------------------------------------------
                    GameModeTxt.text = "Rings";
                    highscoreTextStartMenu.text = playerProfile.HighScoreRings.ToString();
                    //SaveManager.Save(playerProfile);
                    switchGameModeToLeft.gameObject.SetActive(true);
                    switchGameModeToRight.gameObject.SetActive(true);
                    return;
                }
            case 2:
                {
                    //Shoot---------------------------------------------------------------------------------------------
                    GameModeTxt.text = "Shoot";
                    highscoreTextStartMenu.text = playerProfile.HighScoreShoot.ToString();
                    //SaveManager.Save(playerProfile);
                    switchGameModeToLeft.gameObject.SetActive(true);
                    switchGameModeToRight.gameObject.SetActive(false);
                    return;
                }
        }

        
    }

    #endregion

    #region GameOver & Restart
    public void retryGame()
    {
        //selectSound();
        GameObject.Find("Background Music").GetComponent<AudioSource>().enabled = true;
        gameOverCanvas.SetActive(false);
        SceneManager.LoadScene("FollowFinger");
        Time.timeScale = 1;
    }
    public void EndGame()
    {
        playerProfile = SaveManager.Load();

        GameIsOver = true;
        
        if (GameMode == 0)
        {
            int TricoinsFromCurrentRound = score * 50;
            TricoinsTextGameOver.text = TricoinsFromCurrentRound.ToString();
            playerProfile.Tricoins += TricoinsFromCurrentRound;

            if (score > playerProfile.HighScoreArcade)
            {
                playerProfile.HighScoreArcade = score;
                HighScoreTxtGameOver.text = score.ToString();
            }
            else HighScoreTxtGameOver.text = playerProfile.HighScoreArcade.ToString();


            SaveManager.Save();

        }
        if (GameMode == 1)
        {
            if (score > playerProfile.HighScoreRings)
            {
                playerProfile.HighScoreRings = score;
                SaveManager.Save();
                HighScoreTxtGameOver.text = score.ToString();
            }

            else HighScoreTxtGameOver.text = playerProfile.HighScoreRings.ToString();
        }
        if (GameMode == 2)
        {
            if (score > playerProfile.HighScoreShoot)
            {
                playerProfile.HighScoreShoot = score;
                SaveManager.Save();
                HighScoreTxtGameOver.text = score.ToString();
            }

            else HighScoreTxtGameOver.text = playerProfile.HighScoreShoot.ToString();
        }
        scoreTextGameOver.text = score.ToString();
        StartCoroutine(RestartLevel());
    }
    IEnumerator RestartLevel()
    {

        Time.timeScale = 1f / slowness;
        Time.fixedDeltaTime = Time.fixedDeltaTime / slowness;
        yield return new WaitForSeconds(1f / slowness);
        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.fixedDeltaTime / slowness;
        gameOverCanvas.SetActive(true);
    }
    #endregion

    #region Score Logic & Game Difficulty

    public void IncrementScore()
    {
        if (GameIsOver) return;
        score++;
        if(ScoreTextIngame != null) ScoreTextIngame.text = score.ToString();

        if (GameMode == 0) playerProfile.ArcadeBlocksAvoided++;
        else if (GameMode == 1) playerProfile.RingsCatched++;
        else if (GameMode == 2) playerProfile.ShootKills++;
    }
    public int getDifficultyForScore(int score)
    {
        if (score < 10)
        {
            return 0;
        }

        if (score >= 10 && score < 30)
        {
            return 1;
        }

        if (score >= 30 && score < 50)
        {
            return 2;
        }

        if (score >= 50 && score < 70)
        {
            return 3;
        }

        if (score >= 70 && score < 100)
        {
            return 4;
        }

        if (score >= 100 && score < 120)
        {
            return 5;
        }

        if (score >= 120 && score < 140)
        {
            return 6;
        }

        if (score >= 140 && score < 160)
        {
            return 7;
        }

        if (score >= 160 && score < 180)
        {
            return 8;
        }

        if (score >= 180 && score < 200)
        {
            return 9;
        }

        if (score >= 200 && score < 220)
        {
            return 10;
        }

        if (score >= 220 && score < 240)
        {
            return 11;
        }

        if (score >= 240 && score < 260)
        {
            return 12;
        }

        if (score >= 260 && score < 280)
        {
            return 13;
        }

        if (score >= 280 && score < 300)
        {
            return 14;
        }

        else
        {
            return 15;
        }
    }
    public void setDashSpeed()
    {
        savedSpeed = blockSpawner.currentSpeed;
        blockSpawner.setSpeed(blockSpawner.currentSpeed * 3);
    }
    public void resetDashSpeed()
    {
        blockSpawner.setSpeed(savedSpeed);
    }

    #endregion

}
