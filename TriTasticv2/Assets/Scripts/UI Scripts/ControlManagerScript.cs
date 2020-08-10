using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class ControlManagerScript : MonoBehaviour
{
    public int GameMode;

    public GameObject GameModeAnzeige;

    public Button QuitButton;

    public Button changeToArcadeButton;
    public Button changeToRingsButton;

    public BackGroundMusicManagement BG;

    
    public Button ShopButton;
    public Button StartGameButton;

    public RectTransform mainMenu, shopMenu;

    private MoveUIPanelDown MoveUIPanel;
    private MoveUIDown MoveUI;
    private MoveLogoUp MoveLogo;

    public ShopScript shopScript;
    public GameObject ShopCanvas;
    public GameObject ControlCanvas;
    public bool GameIsPlayed = false;
    

    void Start()
    {
        GameMode = 0;

        changeToArcadeButton.onClick.AddListener(switchToArcade);
        changeToRingsButton.onClick.AddListener(switchToRings);

        QuitButton.onClick.AddListener(closeGame);
        shopScript = GameObject.Find("ShopManager").GetComponent<ShopScript>();
        ShopButton.onClick.AddListener(openShop);
        
        MoveLogo = GameObject.FindWithTag("LogoUp").GetComponent<MoveLogoUp>();
        MoveUI = GameObject.FindWithTag("UIDown").GetComponent<MoveUIDown>();
        MoveUIPanel = GameObject.FindWithTag("UIPanelDown").GetComponent<MoveUIPanelDown>();
        StartGameButton.onClick.AddListener(StartTheGamePlay);
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


    public void switchToArcade()
    {
        if(BG.muted == false)
            FindObjectOfType<AudioManager>().Play("Select Sound");

        MoveLogo.changeToArcade();
        GameMode = 0;
    }

    public void switchToRings()
    {
        if (BG.muted == false)
            FindObjectOfType<AudioManager>().Play("Select Sound");

        MoveLogo.changeToRings();
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
        Debug.Log(GameMode);
        if(GameMode == 1)
        {
            changeToArcadeButton.enabled = true;
            changeToArcadeButton.GetComponent<Image>().enabled = true;

            changeToRingsButton.enabled = false;
            changeToRingsButton.GetComponent<Image>().enabled = false;
        }

        if(GameMode == 0)
        {
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

















