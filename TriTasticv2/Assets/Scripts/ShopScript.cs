using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class ShopScript : MonoBehaviour
{
    //Dieser Animator ist lediglich für den Player und nicht nötig im Normalfall. Habe einen Ninja stern skin der sich drehen sollte also brauchte ich einen animator!

    public Image ShopBackGround;
    public Image ShopSwipeArea;
    public Image CodeBG;
    public Button[] ShopUI;

    public Color CodePanelBGRed;
    public Color CodePanelBGGreen;
    public Color CodePanelBGBlue;

    public Color ShopBGRed;
    public Color ShopBGGreen;
    public Color ShopBGBlue;

    public Color SwipeAreaRed;
    public Color SwipeAreaGreen;
    public Color SwipeAreaBlue;

    public Color UIRed;
    public Color UIGreen;
    public Color UIBlue;



    public Button openCodePanelButton;
    public Button acceptCodeButton;
    public GameObject CodePanel;
    public Animator CodePanelAnim;
    public InputField CodeEingabeFeld;
    public Text CodeNotFoundText;

    public Animator PlayerAnim;

    //Dieser Int Array wird im Inspector gesetzt, so kann man die Preise der Skins ändern. index 0 des Arrays steht für den Preis vom ersten Skin , index 1 für den Preis vom 2. etc.

    public int[] skinHighscorePrices;

    
    public GameObject Hintergrund;

    //Rect Transform ist von DoTween und dient zur Verschiebung der Panel. (Wenn Shop geschlossen wird dann wird das Panel aus der Szene nach unten geschoben)

    public RectTransform mainMenu, shopMenu;

    //Diese Sprites sind lediglich Zeichnungen für die auswählbaren Hintergründe meines Games 
    public Sprite HG1;
    public Sprite HG2;
    public Sprite HG3;

    //Das sind die Buttons mit denen man die HG wechselt
    public Button HG1Button;
    public Button HG2Button;
    public Button HG3Button;

    //Eine Referenz vom Player Object
    public GameObject Player;

    //zum Schließen des Shops
    public Button closeButton;

    //ControlCanvas ist der Name meines Default UI Canvas (der ist immer da aber wird beim Shop deaktiviert damit man paus Button und Score nicht im Shop sieht
    public GameObject ControlCanvas;

    //Referenz zum ShopPanel
    public GameObject ShopCanvas;

    //Das FollowFingerScript ist das PlayerMovement Script meines Games (Das Dreieck folgt ja dem Finger also habe ich das so genannt)
    public FollowFingerScript followFinger;

    //Das ist ein Array mit den Buttons die bei Berührung einen Skin freischalten oder halt nicht (wenn der Score zu klein ist)
    public Button[] skinPurchaseButtons;


    //Dieses Object ist für meine Background Musik verantwortlich
    public BackGroundMusicManagement BG;

    void Start()
    {
        

        CodeNotFoundText.enabled = false;

        CodePanel.SetActive(false);
        CodePanelAnim = CodePanel.GetComponent<Animator>();

        //Erst einmal ich bin noch zu unsicher um mit Save Data (Json Files etc.) zu arbeiten und wollte lediglich einen Shop der leicht erweiterbar ist und Skins freischalten und speichern kann
        //Habe also alles mit den Unity PlayerPrefs gemacht also nicht wundern über den Spaghetti Code xD

        //Ich habe eine Int für den jeweiligen Hintergrund gemacht das heisst wenn die Int "Hintergrund" 0 ist, ist der erste Hintergrund Sprite ausgewählt
        //Wenn die "Hintergrund" Int 1 ist ist der 2. Hintergrund sprite aktiviert und so weiter. 
        //Die Hintergrud Sprites sind oben im Skript auch durch ne Referenz bestimmt.

        openCodePanelButton.onClick.AddListener(openCodePanel);
        acceptCodeButton.onClick.AddListener(closeCodePanel);

        if(PlayerPrefs.GetInt("Hintergrund")== 0)
        {
            CodeBG.color = CodePanelBGRed;
            Hintergrund.GetComponent<SpriteRenderer>().sprite = HG1;
            ShopBackGround.color = ShopBGRed;
            ShopSwipeArea.color = SwipeAreaRed;

            foreach(Button buttons in ShopUI)
            {
                buttons.GetComponent<Image>().color = UIRed;
            }
        }

        if (PlayerPrefs.GetInt("Hintergrund") == 1)
        {
            CodeBG.color = CodePanelBGGreen;
            Hintergrund.GetComponent<SpriteRenderer>().sprite = HG2;
            ShopBackGround.color = ShopBGGreen;
            ShopSwipeArea.color = SwipeAreaGreen;

            foreach (Button buttons in ShopUI)
            {
                buttons.GetComponent<Image>().color = UIGreen;
            }
        }

        if (PlayerPrefs.GetInt("Hintergrund") == 2)
        {
            CodeBG.color = CodePanelBGBlue;
            Hintergrund.GetComponent<SpriteRenderer>().sprite = HG3;
            ShopBackGround.color = ShopBGBlue;
            ShopSwipeArea.color = SwipeAreaBlue;

            foreach (Button buttons in ShopUI)
            {
                buttons.GetComponent<Image>().color = UIBlue;
            }
        }


        //Hier weise ich dn Hintergrund Buttons ihre Funktion zu
        HG1Button.onClick.AddListener(changeToHG1);
        HG2Button.onClick.AddListener(changeToHG2);
        HG3Button.onClick.AddListener(changeToHG3);


        //Hier weise ich einfach dem FollowFinger Skipt das Skript zu um das Im Inspektor nicht tun zu müssen

        followFinger = GameObject.FindWithTag("Player").GetComponent<FollowFingerScript>();

        //Mit der for Loop gehe ich durch alle Skin Kauf Buttons durch und gebe denen so ihre Funktion. Habe ja mit nem Array gearbeitet und mache das daher durch ne For Loop

        if(PlayerPrefs.GetInt("CodeRight") == 2)
        {
            //Alle Buttons sind entsperrt
            foreach (Button buttons in skinPurchaseButtons)
                buttons.interactable = true;

        }

        
            for (int index = 0; index < skinPurchaseButtons.Length; index++)
            {
                switch (index)
                {
                    case 0:
                        skinPurchaseButtons[index].onClick.AddListener(() => selectSkin(0));
                        break;
                    case 1:
                        skinPurchaseButtons[index].onClick.AddListener(() => selectSkin(1));
                        break;
                    case 2:
                        skinPurchaseButtons[index].onClick.AddListener(() => selectSkin(2));
                        break;

                    case 3:
                        skinPurchaseButtons[index].onClick.AddListener(() => selectSkin(3));
                        break;


                }

            
        }
        

        //Hier wird dann auch dem Shop Schließ Button seine Funktion zugewiesen
        closeButton.onClick.AddListener(closeShop);
    }

    public void openCodePanel()
    {
        closeButton.enabled = false;
        CodePanel.SetActive(true);
        //CodePanelAnim.SetTrigger("openPanel");
    }

    public void closeCodePanel()
    {
        closeButton.enabled = true;
        if (CodeEingabeFeld.text == "Tritastic2020")
        {
            //Skin freigeschalten

            foreach (Button skinButton in skinPurchaseButtons)
                skinButton.interactable = true;

            PlayerPrefs.SetInt("CodeRight", 2);
        }

        else
            StartCoroutine(showCodeNotFoundMessage());

        //CodePanelAnim.SetTrigger("closePanel");
        Invoke("DeactivateCodePanel", 0.75f);
    }

    public void DeactivateCodePanel()
    {
        CodePanel.SetActive(false);
    }

    public void changeToHG1()
    {
        CodeBG.color = CodePanelBGRed;
        ShopBackGround.color = ShopBGRed;
        ShopSwipeArea.color = SwipeAreaRed;

        foreach (Button buttons in ShopUI)
        {
            buttons.GetComponent<Image>().color = UIRed;
        }

        //Diese Methode wechselt zum ersten HG

        //Musik wird je nachdem ob der Spieler den Sound gemutet hat oder nicht abgespielt oder nicht abgespielt 
        if (BG.muted == false)
        FindObjectOfType<AudioManager>().Play("Select Sound");

        //Das Hintergrund Object bekommt den ersten HG Sprite zugewiesen
        Hintergrund.GetComponent<SpriteRenderer>().sprite = HG1;

        //Die Hintergrund Player Pref ist nun auf 0 gesetzt, so wird die auch nach beeenden des Games gespeichert
        PlayerPrefs.SetInt("Hintergrund", 0);

        //Hier wird jetzt das Shop Panel nach unten verschoben (Das ist Geschmacks Sache aber ich finde das gut, wenn nach einem Kauf der Shop weg geht)
        shopMenu.DOAnchorPos(new Vector2(0, -1402), 0.25f);
    }

    public IEnumerator showCodeNotFoundMessage()
    {
        CodeNotFoundText.enabled = true;

        yield return new WaitForSeconds(.5f);

        CodeNotFoundText.enabled = false;

    }

    public void changeToHG2()
    {
        CodeBG.color = CodePanelBGGreen;
        ShopBackGround.color = ShopBGGreen;
        ShopSwipeArea.color = SwipeAreaGreen;

        foreach (Button buttons in ShopUI)
        {
            buttons.GetComponent<Image>().color = UIGreen;
        }


        //Das selbe passiert hier nur das hier die Player Pref Int auf 1 gesetzt und das HG Objekt den 2. Skin zugewiesen bekommt

        if (BG.muted == false)
        FindObjectOfType<AudioManager>().Play("Select Sound");

        Hintergrund.GetComponent<SpriteRenderer>().sprite = HG2;
        PlayerPrefs.SetInt("Hintergrund", 1);
        
        shopMenu.DOAnchorPos(new Vector2(0, -1402), 0.25f);
    }

    public void changeToHG3()
    {
        CodeBG.color = CodePanelBGBlue;
        ShopBackGround.color = ShopBGBlue;
        ShopSwipeArea.color = SwipeAreaBlue;

        foreach (Button buttons in ShopUI)
        {
            buttons.GetComponent<Image>().color = UIBlue;
        }

        //Auch hier passiert das selbe nur mit Player Pref int auf 2 und HG Object HG3 

        if (BG.muted == false)
        FindObjectOfType<AudioManager>().Play("Select Sound");

        Hintergrund.GetComponent<SpriteRenderer>().sprite = HG3;
        PlayerPrefs.SetInt("Hintergrund", 2);
        
        shopMenu.DOAnchorPos(new Vector2(0, -1402), 0.25f);
    }

    


    public void initShop()
    {

        // checke ob Highscore größer als benötigter Highscore ist, wenn ja dann wird button interactable
        //Diese Funktion guckt mit einer For Loop durch alles Skin Prices durch und checkt ob dein HighScore größer als der jeweilige Skin Price ist. Wenn ja dann wir der Button klickbar und hell 
        //wenn nein dann wird er nicht klickbar und dunkel
        
        for (int index = 0; index < skinPurchaseButtons.Length; index++)
        {
            if(PlayerPrefs.GetInt("CodeRight")== 2)
            {
                foreach (Button b in skinPurchaseButtons)
                    b.GetComponent<Button>().interactable = true;
            }

            else
            {
                if (PlayerPrefs.GetInt("PlayerHighScore", 0) < skinHighscorePrices[index])
                {
                    skinPurchaseButtons[index].GetComponent<Button>().interactable = false;
                }
                else
                {
                    skinPurchaseButtons[index].GetComponent<Button>().interactable = true;
                }
            }

            
        }
    }

    public void closeShop()
    {
        //Diese Funktion dient zum Schließen des Shops
        if(BG.muted == false)
        FindObjectOfType<AudioManager>().Play("Select Sound");
        

        shopMenu.DOAnchorPos(new Vector2(0,-1402), 0.25f);
    }

    public void selectSkin(int skinIndex)
    {
        //Mit dieser Parameter Funktion wird der Skin selected es kommt eine int die den Name skinIndex hat in die Klammern diese steht dann für den jeweiligen Skin
        if (BG.muted == false)
            FindObjectOfType<AudioManager>().Play("Select Sound");
        
        shopMenu.DOAnchorPos(new Vector2(0, -1402), 0.25f);

        //Die PlayerPref int wird dann mit der jeweiligen Skin Index int gleichgesetzt und so wird die jeweilige Int abgespeichert
        PlayerPrefs.SetInt("skinSelected", skinIndex);

        //Diese Funktion ist in meinem Follow Finger Skrip also im Player Skript und checkt einfach nur die PlayerPref int vom selected Skin und wechselt dann einfach den Sprite zu der Int passend
        followFinger.updateSkin();
        
    }
}
