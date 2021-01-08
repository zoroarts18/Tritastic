using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;


public class GameManager : MonoBehaviour
{
    public PlayerProfile playerProfile;
    //------------------------------------------------------------------------------------------------------
    [Header("Values for the growing difficulty")]
    public int score;
    public float startSpeed = 5;
    public float speedStep = 1;
    public float scoreBetweenSteps = 8;
    public float lastStepScore = 0;
    public float savedSpeed = 0;
    public float[] timeBetweenSpawnList =
    {
        2.5f,
        2f,
        1.5f,
        1f,
    };
    public int GameMode;
    //------------------------------------------------------------------------------------------------------
    [Header("Components & Scripts to assign")]
    public GameObject Player;
    public BlockSpawner blockSpawner;
    public GameObject spawnPointLeft, spawnPointMid, spawnPointRight;
    public BackGroundMusicManagement BG;
    public UIMovement ui;
    public AudioManager audio;

    //------------------------------------------------------------------------------------------------------
    [Header("GameOver & Restart")]
    public float slowness = 10f;
    public bool GameIsOver = false;
    public static bool gameOver = false;
    public bool GameIsPlayed;

    //------------------------------------------------------------------------------------------------------
    [Header("Ingame UI")]
    public Button pauseButton;
    public Text ScoreTextIngame;

    //------------------------------------------------------------------------------------------------------
    [Header("Pause Menu UI")]
    public Button resumeButton;
    public Button backToHomeButton;
    public Button muteSoundButtonPause;

    //------------------------------------------------------------------------------------------------------
    [Header("Game Over Menu")]
    public Button retryButton;
    public Text scoreTextGameOver;
    public Text TricoinsTextGameOver;
    public Text HighScoreTxtGameOver;

    //------------------------------------------------------------------------------------------------------
    [Header("Start Menu UI Buttons")]
    public Button StartGameButton;
    public Button switchGameModeToRight;
    public Button switchGameModeToLeft;
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
    public Button UpgradeBoostAbilityButton;
    public Button UpgradeShootAbilityButton;
    public Button UpgradeEarningsButton;

    //------------------------------------------------------------------------------------------------------
    [Header("Start Menu UI Texts")]
    public Text highscoreTextStartMenu;
    public Text GameModeTxt;
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
    public Text ShootUpgradePriceTxt;
    public Text BoostUpgradePriceTxt;
    public Text TricoinsUpgradePriceTxt;

    //------------------------------------------------------------------------------------------------------
    [Header("Profile Ints")]
    public int RingscatchedThisRound;
    public int BlocksAvoidedThisRound;
    public int ShootKillsThisRound;

    public int[] UpgradePrices;
    //------------------------------------------------------------------------------------------------------
    [Header("Important for Changing Colors")]
    public SpriteRenderer BackGroundImg;
    //Die Sprites müssen in der Reihenfolge sein wie die Werte im Enum Background 
    public Sprite[] backGroundSprites;
    public Sprite[] backGroundSpritesUI;
    public Image[] backGroundsToChange;
    public Button[] ButtonsToChangeDesign;
    public Color[] colors;

    //------------------------------------------------------------------------------------------------------

    public GameObject adManager;

    public Button ShowLeaderBoardBtn;
    public static bool isConnectedToGooglePlayServices = false;
    public Button ReviveByWatchingAdButton;

    private string ShareMessage;
    public Button shareBtn;
    private void Awake()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }
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
        UpgradeBoostAbilityButton.onClick.AddListener(UpgradeBoostAbility);
        UpgradeShootAbilityButton.onClick.AddListener(UpgradeShootAbility);
        UpgradeEarningsButton.onClick.AddListener(UpgradeEarnings);
        ShowLeaderBoardBtn.onClick.AddListener(openLeaderboard);
        shareBtn.onClick.AddListener(clickShareButton);

        blockSpawner.currentSpeed = startSpeed;
        playerProfile = SaveManager.Load();

        GetGameModeFromSaveFile();
        GetBackGroundFromSaveFile();

        GameIsPlayed = false;

        RingscatchedThisRound = 0;
        BlocksAvoidedThisRound= 0;
        ShootKillsThisRound = 0;

        UpgradeShootAbilityButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = playerProfile.ShootUpgradeCount.ToString() + "/10";
        UpgradeBoostAbilityButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = playerProfile.BoostUpgradeCount.ToString() + "/10";
        UpgradeEarningsButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = playerProfile.TricoinsUpgradeCount.ToString() + "/10";

        if(isConnectedToGooglePlayServices == false) SignInToGooglePlayServices();

        ShootUpgradePriceTxt.text = UpgradePrices[playerProfile.ShootUpgradeCount].ToString();
        BoostUpgradePriceTxt.text = UpgradePrices[playerProfile.BoostUpgradeCount].ToString();
        TricoinsUpgradePriceTxt.text = UpgradePrices[playerProfile.TricoinsUpgradeCount].ToString();
    }

    public void SignInToGooglePlayServices()
    {
        PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, (result) =>
        {
            Debug.Log("PlayGamesPlatform Authenticate returned status code " + (int)result + ": " + result.ToString());
            switch (result)
            {
                case SignInStatus.Success:
                    {
                        isConnectedToGooglePlayServices = true;
                        Debug.LogError("Sign in Success");
                        break;
                    }
                case SignInStatus.Failed:
                    {
                        isConnectedToGooglePlayServices = false;
                        Debug.LogError("Sign in Failed");
                        break;
                    }
                case SignInStatus.NotAuthenticated:
                    {
                        isConnectedToGooglePlayServices = false;
                        Debug.LogError("Not Authenticated");
                        break;
                    }
                case SignInStatus.DeveloperError:
                    {
                        isConnectedToGooglePlayServices = false;
                        Debug.LogError("Developer Error on Sign In");
                        break;
                    }
                case SignInStatus.Canceled:
                    {
                        isConnectedToGooglePlayServices = false;
                        Debug.LogError("Sign In Canceled");
                        break;
                    }
                case SignInStatus.NetworkError:
                    {
                        isConnectedToGooglePlayServices = false;
                        Debug.LogError("Network Error on Sign In");
                        break;
                    }
                case SignInStatus.AlreadyInProgress:
                    {
                        Debug.LogError("Sign in already in Progress!");
                        break;
                    }

            }
        });
    }
    //SFX:
    public void UpgradeShootAbility()
    {
        if(playerProfile.ShootUpgradeCount <10 && playerProfile.Tricoins >= UpgradePrices[playerProfile.ShootUpgradeCount])
        {
            playerProfile.ShootUpgrade += 0.5f;
            playerProfile.ShootUpgradeCount++;
            playerProfile.Tricoins -= UpgradePrices[playerProfile.ShootUpgradeCount];
            UpgradeShootAbilityButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = playerProfile.ShootUpgradeCount.ToString() + "/10";
            SaveManager.Save();
        }
        
    }
    public void UpgradeBoostAbility()
    {
        if(playerProfile.BoostUpgradeCount <10 && playerProfile.Tricoins>= UpgradePrices[playerProfile.BoostUpgradeCount])
        {
            playerProfile.BoostUpgrade += 0.5f;
            playerProfile.BoostUpgradeCount++;
            playerProfile.Tricoins -= UpgradePrices[playerProfile.BoostUpgradeCount];
            UpgradeBoostAbilityButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = playerProfile.BoostUpgradeCount.ToString() + "/10";
            SaveManager.Save();
        }
    }
    public void UpgradeEarnings()
    {
        if(playerProfile.TricoinsUpgradeCount < 10 && playerProfile.Tricoins >= UpgradePrices[playerProfile.TricoinsUpgradeCount])
        {
            playerProfile.TricoinsUpgrade += 1;
            playerProfile.TricoinsUpgradeCount++;
            playerProfile.Tricoins -= UpgradePrices[playerProfile.TricoinsUpgradeCount];
            UpgradeEarningsButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = playerProfile.TricoinsUpgradeCount.ToString() + "/10";
            SaveManager.Save();
        }
    }
    public void selectSound()
    {
        if (BG.muted == false)
            audio.Play("Select Sound");
    }

    #region GetInfosFromSaveFile
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

    #endregion

    #region pause & unpause
    public void pauseGame()
    {
        if(!ui.GameOverPanel.activeSelf)
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
        GameIsPlayed = true;
        selectSound();
        Player.GetComponent<FollowFingerScript>().ActivateTrails();
        ui.MoveStartMenuUIOut();
        blockSpawner.GetProfile();

        SaveManager.Save();
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
        if(playerProfile.ShootUpgradeCount == 10)
        {
            UpgradeShootAbilityButton.interactable = false;
        }
        if(playerProfile.BoostUpgradeCount == 10)
        {
            UpgradeBoostAbilityButton.interactable = false;
        }
        if(playerProfile.TricoinsUpgradeCount == 10)
        {
            UpgradeEarningsButton.interactable = false;
        }

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
        Time.timeScale = 1;
        SceneManager.LoadScene("FollowFinger");
    }
    public void openLeaderboard()
    {
        Social.ShowLeaderboardUI();
    }

    public void clickShareButton()
    {
        ShareMessage = "WOW! I just scored " + score.ToString() + " points in my favourite mobile game: Tritastic! Can you beat me?";
        StartCoroutine(TakeScreenshotAndShare());
    }

    private IEnumerator TakeScreenshotAndShare()
    {
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
        File.WriteAllBytes(filePath, ss.EncodeToPNG());

        // To avoid memory leaks
        Destroy(ss);

        new NativeShare().AddFile(filePath).SetSubject("Tritastic").SetText (ShareMessage).Share();
    }

    public void EndGame()
    {
        //playerProfile = SaveManager.Load();

        if(GameMode == 0)
        {
            ReviveByWatchingAdButton.gameObject.SetActive(true);
        }

        else
        {
            ReviveByWatchingAdButton.gameObject.SetActive(false);
        }


        playerProfile.ArcadeBlocksAvoided += BlocksAvoidedThisRound;
        playerProfile.RingsCatched += RingscatchedThisRound;
        playerProfile.ShootKills += ShootKillsThisRound;

        GameIsOver = true;
        
        if (GameMode == 0)
        {
            playerProfile.ArcadeMatchesPlayed++;
            if(playerProfile.TricoinsUpgrade == 0)
            {
                int TricoinsFromCurrentRound = score / 10;
                TricoinsTextGameOver.text = TricoinsFromCurrentRound.ToString();
                playerProfile.Tricoins += TricoinsFromCurrentRound;
            }

            else
            {
                int TricoinsFromCurrentRound = score / (11 - playerProfile.TricoinsUpgrade);
                TricoinsTextGameOver.text = TricoinsFromCurrentRound.ToString();
                playerProfile.Tricoins += TricoinsFromCurrentRound;
            }
            
            if (score > playerProfile.HighScoreArcade)
            {
                playerProfile.HighScoreArcade = score;
                HighScoreTxtGameOver.text = score.ToString();

                if(isConnectedToGooglePlayServices == true)
                {
                    Social.ReportScore(score, GPGSIds.leaderboard_highscore, (success) =>
                    {
                        if (!success) Debug.LogError("HighScore could not be posted");
                        if (success) Debug.LogError("HighScore posted");
                    });
                }
            }

            else HighScoreTxtGameOver.text = playerProfile.HighScoreArcade.ToString();


            SaveManager.Save();

        }
        if (GameMode == 1)
        {
            playerProfile.RingsMatchesPlayed++;

            if (score > playerProfile.HighScoreRings)
            {
                playerProfile.HighScoreRings = score;
                
                HighScoreTxtGameOver.text = score.ToString();
            }

            else HighScoreTxtGameOver.text = playerProfile.HighScoreRings.ToString();

            SaveManager.Save();
        }
        if (GameMode == 2)
        {
            playerProfile.ShootMatchesPlayed++;

            if (score > playerProfile.HighScoreShoot)
            {
                playerProfile.HighScoreShoot = score;
                HighScoreTxtGameOver.text = score.ToString();
            }
            else HighScoreTxtGameOver.text = playerProfile.HighScoreShoot.ToString();
            SaveManager.Save();
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
        ui.openGameOverMenu();
    }
    #endregion

    #region Score Logic & Game Difficulty

    public void IncrementScore()
    {
        if (Player.GetComponent<FollowFingerScript>().isDead) return;
        score++;
        if(ScoreTextIngame != null) ScoreTextIngame.text = score.ToString();
        if (GameMode == 0) BlocksAvoidedThisRound ++;
        else if (GameMode == 1) RingscatchedThisRound++;
        else if (GameMode == 2) ShootKillsThisRound++;


        //SaveManager.Save();
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
