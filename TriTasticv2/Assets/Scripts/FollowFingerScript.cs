﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class FollowFingerScript : MonoBehaviour
{
    public BackGroundMusicManagement BG;
    public Sprite[] skins;
    public GameObject controller;

    public Animator PlayerAnim;

    //public GameObject BoostPanel;
    public GameObject BlockExplosion;

    public GameObject BlueBlockParticles;
    public GameObject GreenBlockParticles;

    private Rigidbody2D rb;
    private Shake shake;
    public UIManager uiManager;

    public float DashTime = 10f;
    public GameObject WindParticles;
    public GameObject goldenExplosion;
    public GameObject explosionPrefab;

    public GameObject Bullet;

    public bool isDead;
    public bool isDashing = false;
    public bool DashOver = true;
    public bool DashUnlocked = true;
    public bool isShooting;

    private bool startedShooting = false;

    public SpriteRenderer sr;

    private float deltaX, deltaY;


    public Image BoostIndicator;
    public Image ShootIndicator;


    private Color green;

    private Color blue;
    void Start()
    {

        blue = new Color(0, 23, 79);
        green = new Color(0, 58, 0);


        isShooting = false;

        ShootIndicator.enabled = false;
        BoostIndicator.enabled = false;

        sr = GetComponent<SpriteRenderer>();
        updateSkin();

        isDead = false;
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();

        shake = GameObject.FindWithTag("ScreenShake").GetComponent<Shake>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void updateSkin()
    {
        sr.sprite = skins[PlayerPrefs.GetInt("skinSelected", 0)];
    }
    
    public IEnumerator autoShoot()
    {
        while(!isDead)
        {
            GameObject inGameBullet = Instantiate(Bullet, new Vector2(transform.position.x, transform.position.y + 0.2f), Quaternion.identity);
            if (BG.muted == false)
            {
                FindObjectOfType<AudioManager>().Play("Shoot SFX");
            }



            yield return new WaitForSeconds(0.2f);
        }
    }
    public IEnumerator shoot()
    {
        while(isShooting)
        {
            GameObject inGameBullet = Instantiate(Bullet, new Vector2(transform.position.x, transform.position.y + 0.2f), Quaternion.identity);
            if(BG.muted == false)
            {
                FindObjectOfType<AudioManager>().Play("Shoot SFX");
            }
            
            

            yield return new WaitForSeconds(0.25f);
        }
        
    }
    public void Update()
    {
        if(controller.GetComponent<ControlManagerScript>().GameIsPlayed == true && startedShooting == false && controller.GetComponent<ControlManagerScript>().GameMode == 2)
        {
            StartCoroutine(autoShoot());
            startedShooting = true;
        }


        if (isShooting)
        {
            ShootIndicator.enabled = true;
            BoostIndicator.enabled = false;
        }

        if(!isShooting)
        {
            ShootIndicator.enabled = false;
        }

        if (isDashing)
        {
            ShootIndicator.enabled = false;
            BoostIndicator.enabled = true;
        }

        if(!isDashing )
        {
            BoostIndicator.enabled = false;
        }


        if (sr.sprite == skins[3])
        {
            PlayerAnim.SetTrigger("NinjaSelected");
        }

        else
            PlayerAnim.SetTrigger("OtherSelected");


        if (Time.timeScale == 0 && isDead == false)
        {
            DeactivateParticles();
        }

        else //(Time.timeScale == 0 && isDead == true)
        {

            explosionPrefab.SetActive(true);
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);         
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            switch (touch.phase)
            {                
                case TouchPhase.Began:
                    deltaX = touchPos.x - transform.position.x;
                    deltaY = touchPos.y - transform.position.y;
                    break;               
                case TouchPhase.Moved:
                    rb.MovePosition(new Vector2(touchPos.x - deltaX, touchPos.y - deltaY));
                    break;             
                case TouchPhase.Ended:
                    rb.velocity = Vector2.zero;
                    break;
            }
        }
    }

    public void endDash()
    {
        
        uiManager.resetDashSpeed();
        WindParticles.SetActive(false);
        Invoke("PlayerNotInvincible", 2f);
        
    }

    public void PlayerNotInvincible()
    {
        //BoostPanel.SetActive(false);
        //GetComponent<Renderer>().material.color = Color.white;
        isDashing = false;
    }

    

    public void Dash()
    {
        if (isDashing == false)
        {
            //vlt den Player schwarz machen beim dash aber IDK
            //BoostPanel.SetActive(true);
            //GetComponent<Renderer>().material.color = Color.gray;
            isDashing = true;
            //rb.velocity = new Vector2(0, 1f);
            uiManager.setDashSpeed();
            WindParticles.SetActive(true);
            Invoke("endDash", DashTime);                            
        }
    }

    public void DeactivateParticles()
    {
        explosionPrefab.SetActive(false);
    }

    public void stopShooting()
    {
        isShooting = false;
    }

    public void ActivateShoot()
    {
        Debug.Log("Shoot");
        isShooting = true;
        StartCoroutine(shoot());
        Invoke("stopShooting", 7f);
    }
    void OnCollisionEnter2D(Collision2D c11)
    {
        
        if (c11.gameObject.tag == "Blocks")
        {

            if(isDashing)
            {
                if(BG.muted== false)
                 FindObjectOfType<AudioManager>().Play("Block Explosion");
                
                
                //shake.camShake();
                Destroy(c11.gameObject);
                

                if (PlayerPrefs.GetInt("Hintergrund") == 0)
                {
                    GameObject BlockDeadParticles = Instantiate(BlockExplosion, new Vector2(c11.transform.position.x, c11.transform.position.y), Quaternion.identity);
                }

                if (PlayerPrefs.GetInt("Hintergrund") == 2)
                {
                    GameObject BlockDeadParticles = Instantiate(BlueBlockParticles, new Vector2(c11.transform.position.x, c11.transform.position.y), Quaternion.identity);
                }
                if (PlayerPrefs.GetInt("Hintergrund") == 1)
                {
                    GameObject BlockDeadParticles = Instantiate(GreenBlockParticles, new Vector2(c11.transform.position.x, c11.transform.position.y), Quaternion.identity);
                }








            }

            else
            {
                isDead = true;
                shake.camShake();
                this.gameObject.SetActive(false);
                //Invoke("DeactivateParticles", 1f);
                GameObject PlayerParticles =Instantiate(explosionPrefab, new Vector2(transform.position.x, transform.position.y- 0.2f), Quaternion.identity);
                PlayerParticles.tag = "Particles";
                //Destroy(PlayerParticles, 1f);

                if(BG.muted == false)
                {
                    FindObjectOfType<AudioManager>().Play("Explosion Sound");
                    Handheld.Vibrate();
                }
                 
                
                
                FindObjectOfType<GameManager>().EndGame();
                
            }
            
        }
    }
}






















