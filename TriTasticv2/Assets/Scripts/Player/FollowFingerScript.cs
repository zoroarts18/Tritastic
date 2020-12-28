using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class FollowFingerScript : MonoBehaviour
{
    [Header("Own Components")]
    private Rigidbody2D rb;
    public SpriteRenderer sr;
    public Animator PlayerAnim;

    [Header("Skins")]
    public Sprite[] skins;
    public GameObject[] bullets;
    public GameObject[] DeathParticles;
    public GameObject[] ObstacleDestroyedWithBulletParticles;
    public GameObject[] ObstacleDestroyedWithDashParticles;
    public GameObject[] trails;

    public GameObject playerBullet;
    public GameObject playerDeathVFX;
    public GameObject playerTrail;
    public PlayerProfile pp;

    [Header("Other Components")]
    public BackGroundMusicManagement BG;
    public Shake shake;
    public GameManager gameManager;
    public GameObject PowerUpBar;
    public Slider PowerUpSlider;
    private AudioManager audio;
    

    [Header("Stats & Abilities")]
    public float DashTime = 7f;
    private float Energy;
    private float maxEnergie = 8;

    public Image BoostIndicator;
    public Image ShootIndicator;

    //------------------------Bullets--------------------------------------------------------------------------------------
    public GameObject Bullet;
    public GameObject ChristmasBullet;
    public GameObject HalloweenBullet;
    public GameObject SilvesterBullet;



    [Header("VFX")]
    public GameObject BlockExplosion;
    public GameObject WindParticles;
    public GameObject explosionPrefab;

    //------------------------Event Skins--------------------------------------------------------------------------------------
    public GameObject SilvesterDeathVFX;
    public GameObject HalloweenDeathVFX;
    public GameObject WeihnachtenDeathVFX;
    public GameObject WeihnachtsTrails;
    public GameObject HalloweenTrails;
    public GameObject SilvesterTrails;

    public GameObject BlueBlockParticles;
    public GameObject GreenBlockParticles;
    public GameObject plus1;
    
    [Header("Bool")]
    public bool startedShooting = false;
    public bool isShooting;
    public bool isDashing = false;
    public bool touchDetected;
    public bool isDead;

    private Vector3 mousePos;

    public Skin currentSkin;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        audio = GameObject.FindWithTag("Audio").GetComponent<AudioManager>();
        touchDetected = false;

        PowerUpBar.SetActive(false);

        isShooting = false;

        ShootIndicator.enabled = false;
        BoostIndicator.enabled = false;

        isDead = false;

        foreach (var trail in trails)
        {
            trail.SetActive(false);
        }
    }

    public void ActivateTrails()
    {
        //Die jeweilige GameObjects und Prefabs werden verteilt
        pp = SaveManager.Load();
        playerBullet = bullets[(int)currentSkin];
        playerTrail = trails[(int)currentSkin];
        playerDeathVFX = DeathParticles[(int)currentSkin];

        playerTrail.SetActive(true);
    }

    public void changeSkin(Skin selectedSkin)
    {
        // Hier wird der Skin vom Shop Manager übergeben und dann vom Player genutzt um das richtige Image zum Skin zu holen (skins[0]= Triangle, skins[1]= Jet ,skins[2]= Batman, skins[3]= Ninja
        //skins[4]= Easter, skins[5]= Halloween, skins[6]= Christmas, skins[7]= Silvester
        sr.sprite = skins[(int)selectedSkin];
        currentSkin = selectedSkin;
    }

    //-------------------------------------------------------------Shooting für Shoot Modus------------------------------------------------------------------------------------------
    public IEnumerator autoShoot()
    {
        while(!isDead)
        {
            //Hier wird die Bullet passende Bullet aus dem Array genommen (von dem Selected Skin abhängig)
            GameObject Bullet = Instantiate(playerBullet, new Vector2(transform.position.x, transform.position.y + 0.2f), Quaternion.identity);
            if (BG.muted == false) FindObjectOfType<AudioManager>().Play("Shoot SFX");
            yield return new WaitForSeconds(0.2f);
        }
    }

    //-------------------------------------------------------------Shooting für Arcade Modus------------------------------------------------------------------------------------------
    public IEnumerator shoot()
    {
        while(isShooting)
        {
            //GameObject inGameBullet = Instantiate(Bullet, new Vector2(transform.position.x, transform.position.y + 0.2f), Quaternion.identity);
            GameObject Bullet = Instantiate(playerBullet,  new Vector2(transform.position.x, transform.position.y + 0.2f), Quaternion.identity);
            if (BG.muted == false)
            {
                FindObjectOfType<AudioManager>().Play("Shoot SFX");
            }
            yield return new WaitForSeconds(0.25f);
        }
    }

    public void Update()
    {
        if(Input.GetMouseButton(0) && touchDetected == false) touchDetected = true;

        if(gameManager.GetComponent<GameManager>().GameIsPlayed == true && startedShooting == false && gameManager.GetComponent<GameManager>().GameMode == 2)
        {
            StartCoroutine(autoShoot());
            startedShooting = true;
        }

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if(touchDetected && mousePos.y <= 1.5f && gameManager.GameIsPlayed ) rb.MovePosition(new Vector2(mousePos.x, transform.position.y));

        if (isShooting)
        {
            ShootIndicator.enabled = true;
            BoostIndicator.enabled = false;
            Energy -= Time.deltaTime;
            PowerUpSlider.value = Energy;
        }

        if(!isShooting) ShootIndicator.enabled = false;

        if (isDashing)
        {
            Energy -= Time.deltaTime;
            ShootIndicator.enabled = false;
            BoostIndicator.enabled = true;
            PowerUpSlider.value = Energy;
        }

        if(!isDashing ) BoostIndicator.enabled = false;

        if (currentSkin == Skin.Shuriken) PlayerAnim.SetTrigger("NinjaSelected");

        else PlayerAnim.SetTrigger("OtherSelected");

        if (Time.timeScale == 0 && isDead == false) DeactivateParticles();

        else explosionPrefab.SetActive(true);
    }

    public void endDash()
    {
        WindParticles.SetActive(false);
        gameManager.resetDashSpeed();

        Invoke("PlayerNotInvincible", 2f);
        Invoke("DeactivatePowerBar",1);
    }

    void DeactivatePowerBar()
    {
        PowerUpBar.SetActive(false);
    }

    public void PlayerNotInvincible()
    {
        isDashing = false;
    }

    public void Dash()
    {
        if (isDashing == false)
        {
            isDashing = true;
            PowerUpBar.SetActive(true);
            Energy = maxEnergie;
            PowerUpSlider.value = Energy;
            gameManager.setDashSpeed();
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
        Invoke("DeactivatePowerBar", 1f);
    }

    public void ActivateShoot()
    {
        isShooting = true;
        PowerUpBar.SetActive(true);
        Energy = maxEnergie;
        PowerUpSlider.value = Energy;
        StartCoroutine(shoot());
        Invoke("stopShooting", 8f);
    }

    public void Death()
    {
        isDead = true;
        shake.camShake();
        this.gameObject.SetActive(false);

        GameObject playerDeathParticles = Instantiate(DeathParticles[(int)currentSkin], new Vector2(transform.position.x, transform.position.y - 0.2f), Quaternion.identity);
        playerDeathParticles.tag = "Particles";

        //Death SFX & Vibration when not muted
        if (BG.muted == false)
        {
            audio.Play("Explosion Sound");
            Handheld.Vibrate();
        }
        gameManager.EndGame();
    }

    void OnCollisionEnter2D(Collision2D c11)
    {
        
        if (c11.gameObject.tag == "Blocks")
        {
            if(isDashing)
            {
                if(BG.muted== false) audio.Play("Block Explosion");
                Instantiate(plus1,new Vector3 (c11.gameObject.transform.position.x, c11.gameObject.transform.position.y +1, -1 ), Quaternion.identity);
                GameObject BlockDeadParticles = Instantiate(ObstacleDestroyedWithDashParticles[(int)pp.currentBg], new Vector2(c11.transform.position.x, c11.transform.position.y), Quaternion.identity);
                Debug.LogError("Dash HG" + (int)pp.currentBg);
                Destroy(c11.gameObject);
            }
            else Death();
        }
    }
}























