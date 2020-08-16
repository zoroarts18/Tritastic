using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Rendering.PostProcessing;

public class ControlManagerScript : MonoBehaviour
{
    
    public int GameMode;

    public Text currentHighscoreText;

    public GameObject spawnPointLeft, spawnPointMid, spawnPointRight;

    public GameObject GameModeAnzeige;
    public Text GameModeText;

    public Button QuitButton;

    public Button changeToArcadeButton;
    public Button changeToRingsButton;

    public BackGroundMusicManagement BG;

    
    public Button ShopButton;
    public Button StartGameButton;
    public Button closeSettingsButton;
    public Button openSettingsButton;

    public RectTransform mainMenu, shopMenu, Settings;

    private MoveUIPanelDown MoveUIPanel;
    private MoveUIDown MoveUI;
    private MoveLogoUp MoveLogo;

    public ShopScript shopScript;
    public GameObject ShopCanvas;
    public GameObject ControlCanvas;
    public bool GameIsPlayed = false;

    public GameObject PausePanel;
    public Dropdown QualityChanger;
    
    void Start()
    {
        

        if(PlayerPrefs.GetInt("Quality") == 1)
        {
            Camera.main.GetComponent<PostProcessVolume>().enabled = false;
            QualityChanger.value = 1;
        }
            

        if (PlayerPrefs.GetInt("Quality") == 0)
        {
            Camera.main.GetComponent<PostProcessVolume>().enabled = true;
            QualityChanger.value = 0;
        }
            


        openSettingsButton.onClick.AddListener(openSettings);
        closeSettingsButton.onClick.AddListener(closeSettings);

        GameMode = 0;

        changeToArcadeButton.onClick.AddListener(switchLeft);
        changeToRingsButton.onClick.AddListener(switchRight);

        QuitButton.onClick.AddListener(closeGame);
        shopScript = GameObject.Find("ShopManager").GetComponent<ShopScript>();
        ShopButton.onClick.AddListener(openShop);
        
        MoveLogo = GameObject.FindWithTag("LogoUp").GetComponent<MoveLogoUp>();
        MoveUI = GameObject.FindWithTag("UIDown").GetComponent<MoveUIDown>();
        MoveUIPanel = GameObject.FindWithTag("UIPanelDown").GetComponent<MoveUIPanelDown>();
        StartGameButton.onClick.AddListener(StartTheGamePlay);
    }
    public void changeQuality(int option)
    {
        if (option == 1)
        {
            PlayerPrefs.SetInt("Quality", 1);
            Debug.Log("Schlechte Grafik");
            Camera.main.GetComponent<PostProcessVolume>().enabled = false;
        }
            

        if(option == 0)
        {
            PlayerPrefs.SetInt("Quality", 0);
            Debug.Log("Gute Grafik");
            Camera.main.GetComponent<PostProcessVolume>().enabled = true;
        }
            

    }

    public void openSettings()
    {
        Settings.DOAnchorPos(Vector2.zero, 0.25f);
    }

    public void closeSettings()
    {
        Settings.DOAnchorPos(new Vector2(0,1200), 0.25f);
    }



    public void closeGame()
    {
        Application.Quit();
    }
    public void openShop()
    {
        if (BG.muted == false)
            FindObjectOfType<AudioManager>().Play("Select Sound");

        shopScript.initShop();
        //ShopCanvas.SetActive(true);
        shopMenu.DOAnchorPos(Vector2.zero, 0.25f);
        //ControlCanvas.SetActive(false);

    }

    

    public void switchRight()
    {
        
        if (GameMode == 0)
        {
            switchToRings();
            return;
        }
            

        if (GameMode == 1)
        {
            switchToShoot();
            return;
        }
            
    }

    public void switchLeft()
    {
        if (GameMode == 1)
        {
            switchToArcade();
            return;
        }
            

        if (GameMode == 2)
        {
            switchBackToRings();
            return;
        }
            
    }



    public void switchToShoot()
    {
        if (BG.muted == false)
            FindObjectOfType<AudioManager>().Play("Select Sound");

        GameModeText.text = "Shoot";
        GameMode = 2;
    }

    public void switchBackToRings()
    {
        if (BG.muted == false)
            FindObjectOfType<AudioManager>().Play("Select Sound");

        GameModeText.text = "Rings";
        GameMode = 1;
    }

    public void switchToArcade()
    {
        if(BG.muted == false)
            FindObjectOfType<AudioManager>().Play("Select Sound");

        GameModeText.text = "Arcade";
        GameMode = 0;
    }

    public void switchToRings()
    {
        if (BG.muted == false)
            FindObjectOfType<AudioManager>().Play("Select Sound");

        GameModeText.text = "Rings";
        GameMode = 1;
    }


    public void StartTheGamePlay()
    {
        if (BG.muted == false)
            FindObjectOfType<AudioManager>().Play("Select Sound");
        Debug.Log("Los");
        GameIsPlayed = true;
    }

    void Update()
    {
        

        if(GameMode == 2)
        {
            currentHighscoreText.text = PlayerPrefs.GetInt("ShootHighScore", 0).ToString();
            spawnPointLeft.transform.position = new Vector2(-1.98f, 8.68f);
            spawnPointRight.transform.position = new Vector2(2.02f, 8.68f);
            spawnPointMid.transform.position = new Vector2(0.04f, 6.53f);
        }

        else
        {
            spawnPointLeft.transform.position = new Vector2(-1.98f, 6.53f);
            spawnPointRight.transform.position = new Vector2(2.02f, 6.53f);
            spawnPointMid.transform.position = new Vector2(0.04f, 8.68f);
        }


        if (GameMode == 2)
        {
            
            changeToArcadeButton.enabled = true;
            changeToArcadeButton.GetComponent<Image>().enabled = true;

            changeToRingsButton.enabled = false;
            changeToRingsButton.GetComponent<Image>().enabled = false;
        }


        if (GameMode == 1)
        {
            currentHighscoreText.text = PlayerPrefs.GetInt("RingsHighScore", 0).ToString();
            changeToArcadeButton.enabled = true;
            changeToArcadeButton.GetComponent<Image>().enabled = true;

            changeToRingsButton.enabled = true;
            changeToRingsButton.GetComponent<Image>().enabled = true;
        }

        if(GameMode == 0)
        {
            currentHighscoreText.text = PlayerPrefs.GetInt("PlayerHighScore", 0).ToString();
            changeToArcadeButton.enabled = false;
            changeToArcadeButton.GetComponent<Image>().enabled = false;

            changeToRingsButton.enabled = true;
            changeToRingsButton.GetComponent<Image>().enabled = true;
        }


        if (GameIsPlayed == true)
        {
            MoveLogo.MoveTheLogoUp();
            MoveUI.MoveTheUIDown();

            GameModeAnzeige.SetActive(false);

            Invoke("DeactivateCanvas", 2f);

        }
    }

    public void DeactivateCanvas()
    {
        ControlCanvas.SetActive(false);

        MoveUIPanel.MoveUIPanelDownOnStart();
    }

}


















