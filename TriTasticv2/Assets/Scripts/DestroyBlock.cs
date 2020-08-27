using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBlock : MonoBehaviour
{
    public GameObject Player;

    public GameObject PlayerDeathVFX;

    public GameObject plus1;
    public Sprite SpriteRed;
    public Sprite SpriteGreen;
    public Sprite SpriteBlue;

    public GameObject blockExplosion;
    public GameObject blueBlockExplosion;
    public GameObject GreenBlockExplosion;

    public UIManager uiManager;

    public ControlManagerScript CS;

    public int life;
    public bool ringCatched = false;

    public BackGroundMusicManagement BG;
    public GameObject Plus1;
    private void Start()
    {
        CS = GameObject.FindWithTag("Controller").GetComponent<ControlManagerScript>();

        Player = GameObject.FindWithTag("Player");

        BG = GameObject.FindWithTag("BGMusic").GetComponent<BackGroundMusicManagement>();
        life = 2;
    }

    public void TakeDamage()
    {
        life--;

        
        if (life <= 0)
        {
            if(BG.muted == false)
            {
                FindObjectOfType<AudioManager>().Play("Block Explosion");
            }


            if (PlayerPrefs.GetInt("Hintergrund") == 0)
            {
                Instantiate(blockExplosion, transform.position, Quaternion.identity);
            }

            if (PlayerPrefs.GetInt("Hintergrund") == 2)
            {
                Instantiate(blueBlockExplosion, transform.position, Quaternion.identity);
            }

            if (PlayerPrefs.GetInt("Hintergrund") == 1)
            {
                Instantiate(GreenBlockExplosion , transform.position, Quaternion.identity);
            }

            Instantiate(plus1, new Vector3(transform.position.x, transform.position.y, -1), Quaternion.identity);

            Destroy(this.gameObject);

        }
    }

    


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {

            if(ringCatched == false)
            {
                if(BG.muted == false)
                    FindObjectOfType<AudioManager>().Play("RingReached");

                Instantiate(Plus1, new Vector3(transform.position.x, transform.position.y, -1), Quaternion.identity);
                GetComponent<Animator>().SetTrigger("close");
                uiManager.IncrementScore();
            }
            
            ringCatched = true;
        }
    }

    void Update()
    {

        
            

        if (PlayerPrefs.GetInt("Hintergrund") == 0)
        {
            GetComponent<SpriteRenderer>().sprite = SpriteRed;
        }

        if (PlayerPrefs.GetInt("Hintergrund") == 2)
        {
            GetComponent<SpriteRenderer>().sprite = SpriteBlue;
        }

        if (PlayerPrefs.GetInt("Hintergrund") == 1)
        {
            GetComponent<SpriteRenderer>().sprite = SpriteGreen; 
        }

        
        if(transform.position.y < - 7f)
        {
            if(CS.GameMode == 2)
            {
                if (BG.muted == false)
                {
                    Handheld.Vibrate();
                    
                }

                if (Player.GetComponent<FollowFingerScript>().isDead == false)
                {
                    Player.GetComponent<FollowFingerScript>().isDead = true;
                    Instantiate(PlayerDeathVFX, Player.transform.position, Quaternion.identity);
                    Player.SetActive(false);
                    FindObjectOfType<GameManager>().EndGame();
                }
                   
            }

            if(this.transform.gameObject.tag == "Rings" )
            {
                if(ringCatched == false)
                {
                    if (BG.muted == false)
                    {
                        Handheld.Vibrate();
                        FindObjectOfType<AudioManager>().Play("RingMissed");
                    }


                    if (Player.GetComponent<FollowFingerScript>().isDead == false)
                    {
                        Player.GetComponent<FollowFingerScript>().isDead = true;
                        Instantiate(PlayerDeathVFX, Player.transform.position, Quaternion.identity);
                        Player.SetActive(false);


                        FindObjectOfType<GameManager>().EndGame();
                    }
                        
                }
                
            }
            
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if(this.gameObject.tag == "Blocks")
        {
            if(CS.GameMode != 2)
                uiManager.IncrementScore();


            
            if (CS.GameMode == 2)
            {
                if (BG.muted == false)
                {
                    FindObjectOfType<AudioManager>().Play("Block Explosion");
                }
                
            }
        }
        
    }
}
