using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBlock : MonoBehaviour
{
    public AudioManager audio;
    public GameObject Player;

    public GameObject PlayerDeathVFX;

    public GameObject plus1;
    public Sprite SpriteRed;
    public Sprite SpriteGreen;
    public Sprite SpriteBlue;

    public GameObject[] playerDeathPart;
    public GameObject[] ownDeathPart;
    public GameObject[] ownDeathPartEventSkins;

    public GameObject WeihnachtsDeathVFX;
    public GameObject HalloweenDeathVFX;
    public GameObject SilvesterDeathVFX;

    public GameObject NormalBlockExplosion;
    public GameObject blueBlockExplosion;
    public GameObject GreenBlockExplosion;

    //public UIManager uiManager;
    public GameManager gameManager;

    public int life;
    public bool ringCatched = false;

    public BackGroundMusicManagement BG;
    public GameObject Plus1;

    private int playersSkin;
    private int playerBG;

    public PlayerProfile pp;

    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
        BG = GameObject.FindWithTag("BGMusic").GetComponent<BackGroundMusicManagement>();
        audio = GameObject.FindWithTag("Audio").GetComponent<AudioManager>();
        life = 2;

        pp = SaveManager.Load();
        playersSkin = (int)pp.savedCurrentSkin;
        playerBG = (int)pp.currentBg;
    }

    //----------------------------------------------Wird bei COllsion mit Bullet gecalled--------------------------------------------------------------------------
    public void TakeDamage()
    {
        life--;
        //------------------------------------------Block Stirbt weil abgeschossen und 0 Leben---------------------------------------------------------------------
        if (life <= 0)
        {
            if(BG.muted == false) FindObjectOfType<AudioManager>().Play("Block Explosion");

            if(playersSkin <= 4) Instantiate(ownDeathPart[playerBG], transform.position, Quaternion.identity);

            else Instantiate(ownDeathPartEventSkins[playersSkin], transform.position, Quaternion.identity);
            Instantiate(plus1, new Vector3(transform.position.x, transform.position.y, -1), Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    //---------------------------------------------Nur für den Ring Modus (Wenn player durch Ring fliegt)-----------------------------------------------------------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //----------------------------------------------------Ring wird durchflogen:-----------------------------------------------------------------------------
            if(ringCatched == false)
            {
                if(BG.muted == false) FindObjectOfType<AudioManager>().Play("RingReached");
                Instantiate(Plus1, new Vector3(transform.position.x, transform.position.y, -1), Quaternion.identity);
                GetComponent<Animator>().SetTrigger("close");
                gameManager.IncrementScore();
            }
            ringCatched = true;
        }
    }

    void Update()
    {
        //---------------------------------------Hier ist der Fall, dass ein Enemy Object nicht abgeschossen oder ein Ring nicht gesammelt wird wird und aus dem Bereich der Cam rausgeht, der Player hat also verloren------------------------------------
        if(transform.position.y < - 7f)
        {
            //----------------------------------------------------------------------------Shoot Modus----------------------------------------------------------------------------------------
            if (gameManager.GameMode == 2)
            {
                if (BG.muted == false) Handheld.Vibrate();
                //-------------------------------------------------------------------------Player Dead:--------------------------------------------------------------------------------------
                if (Player.GetComponent<FollowFingerScript>().isDead == false)
                {
                    Player.GetComponent<FollowFingerScript>().isDead = true;
                    Instantiate(playerDeathPart[playersSkin], Player.transform.position, Quaternion.identity);
                    Player.SetActive(false);
                    FindObjectOfType<GameManager>().EndGame();
                }
            }

            //----------------------------------------------------------------------------Ring Modus----------------------------------------------------------------------------------------
            if(this.transform.gameObject.tag == "Rings" )
            {
                //------------------------------------------------------------------Ring nicht eingesammelt:--------------------------------------------------------------------------------
                if(ringCatched == false)
                {
                    if (BG.muted == false)
                    {
                        Handheld.Vibrate();
                        audio.Play("RingMissed");
                    }
                    //--------------------------------------------------------------------Player Dead:----------------------------------------------------------------------------------------
                    if (Player.GetComponent<FollowFingerScript>().isDead == false)
                    {
                        Player.GetComponent<FollowFingerScript>().isDead = true;
                        Instantiate(playerDeathPart[playersSkin], Player.transform.position, Quaternion.identity);
                        Player.SetActive(false);
                        FindObjectOfType<GameManager>().EndGame();
                    }     
                }
            }

            //------------------------------------Objekt wird zerstört wenn es zu tief unten ist, um Hierarchy clean zu halten & On Destroy zu triggern-------------------------------------
            Destroy(gameObject);
        }
    }

    //--------------------------------------------------------------------Wird gecalled wenn das Objekt zerstört wurde----------------------------------------------------------------------
    private void OnDestroy()
    {
        //------------------------------------------------------------------------------Arcade & Shoot Gamemode------------------------------------------------------------------------------
        if(this.gameObject.tag == "Blocks")
        {

            //-------------------------------------------------------------------Wenn Arcade-Mode, dann wird score erhöht--------------------------------------------------------------------------------
            if(gameManager.GameMode != 2) gameManager.IncrementScore();

            //-----------------------------------------------------------Wenn Shoot-Mode, dann wird Block Explosion Sound getriggert---------------------------------------------------------------------
            if (gameManager.GameMode == 2)
            {
                if (BG.muted == false) audio.Play("Block Explosion");
            }
        }
    }
}
