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
    public GameObject HeliFlügel;

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
    public GameObject UIManager;

    [Header("Stats & Abilities")]
    public float DashTime;
    public float ShootTime;
    public float maxDashTIme;
    public float maxShootTime;

    public Image BoostIndicator;
    public Image ShootIndicator;

    //------------------------Bullets--------------------------------------------------------------------------------------
    public GameObject Bullet;
    public GameObject ChristmasBullet;
    public GameObject HalloweenBullet;
    public GameObject SilvesterBullet;



    [Header("VFX")]
    
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
    public bool isInvincible = false;

    private Vector3 mousePos;

    public Skin currentSkin;

    public GameObject TodesBlock;
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

        if(pp.BoostUpgrade == 0)
        {
            maxShootTime = 4;
            maxDashTIme = 4;
        }
        else
        {
            maxShootTime = 4 + pp.ShootUpgrade;
            maxDashTIme = 4 + pp.BoostUpgrade;
        }
        DashTime = maxDashTIme;
        ShootTime = maxShootTime;
    }

    public void changeSkin(Skin selectedSkin)
    {
        // Hier wird der Skin vom Shop Manager übergeben und dann vom Player genutzt um das richtige Image zum Skin zu holen (skins[0]= Triangle, skins[1]= Jet ,skins[2]= Batman, skins[3]= Ninja
        //skins[4]= Easter, skins[5]= Halloween, skins[6]= Christmas, skins[7]= Silvester
        sr.sprite = skins[(int)selectedSkin];
        if (selectedSkin == Skin.Helicopter)
            HeliFlügel.SetActive(true);
        else
            HeliFlügel.SetActive(false);

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
            yield return new WaitForSeconds(0.3f);
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
            yield return new WaitForSeconds(0.3f);
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
            ShootTime -= Time.deltaTime;
            PowerUpSlider.value = ShootTime;
        }

        if (!isShooting)
        {
            ShootTime = maxShootTime;
            ShootIndicator.enabled = false;
        }

        if (isDashing)
        {
            ShootIndicator.enabled = false;
            BoostIndicator.enabled = true;
            DashTime -= Time.deltaTime;
            PowerUpSlider.value = DashTime;
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

        PlayerNotInvincible();
        DeactivatePowerBar();
        DashTime = maxDashTIme;
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
            pp.PowerUpsCollected++;
            SaveManager.Save();
            PowerUpSlider.value = DashTime;
            PowerUpSlider.maxValue = DashTime;
            PowerUpBar.SetActive(true);
            gameManager.setDashSpeed();
            WindParticles.SetActive(true);
            Invoke("endDash", DashTime);
            isDashing = true;
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
        pp.PowerUpsCollected++;
        SaveManager.Save();
        PowerUpBar.SetActive(true);
        PowerUpSlider.value = ShootTime;
        PowerUpSlider.maxValue = ShootTime;
        Invoke("stopShooting", ShootTime);
        isShooting = true;
        StartCoroutine(shoot());
    }

    public void Death()
    {
        playerTrail.SetActive(false);
        isDead = true;
        shake.camShake();
        //this.gameObject.SetActive(false);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<PolygonCollider2D>().enabled = false;
        GameObject playerDeathParticles = Instantiate(DeathParticles[(int)currentSkin], new Vector2(transform.position.x, transform.position.y - 0.2f), Quaternion.identity);
        playerDeathParticles.tag = "Particles";

        //Death SFX & Vibration when not muted
        if (BG.muted == false)
        {
            audio.Play("Explosion Sound");
        }
        if(!BG.vibrationMuted)
        {
            Handheld.Vibrate();
        }
        gameManager.EndGame();
    }

    public void revive()
    {
        isInvincible = true;
        playerTrail.SetActive(true);
        if(TodesBlock != null) Destroy(TodesBlock);
        UIManager.GetComponent<UIMovement>().ingameUI.gameObject.SetActive(true);
        isDead = false;
        gameManager.GameIsPlayed = true;
        gameManager.GameIsOver = false;
        this.gameObject.SetActive(true);
        UIManager.GetComponent<UIMovement>().GameOverPanel.SetActive(false);
        StartCoroutine(revivingEffect());
    }

    public IEnumerator revivingEffect()
    {
        GetComponent<PolygonCollider2D>().enabled = false;

        GetComponent<SpriteRenderer>().enabled = true;

        yield return new WaitForSeconds(0.4f);

        GetComponent<SpriteRenderer>().enabled = false;

        yield return new WaitForSeconds(0.4f);

        GetComponent<SpriteRenderer>().enabled = true;

        yield return new WaitForSeconds(0.4f);

        GetComponent<SpriteRenderer>().enabled = false;

        yield return new WaitForSeconds(0.4f);

        GetComponent<SpriteRenderer>().enabled = true;

        yield return new WaitForSeconds(0.4f);

        GetComponent<SpriteRenderer>().enabled = false;

        yield return new WaitForSeconds(0.4f);

        GetComponent<SpriteRenderer>().enabled = true;

        yield return new WaitForSeconds(0.4f);

        GetComponent<SpriteRenderer>().enabled = false;

        yield return new WaitForSeconds(0.4f);

        GetComponent<SpriteRenderer>().enabled = true;

        yield return new WaitForSeconds(0.4f);

        GetComponent<SpriteRenderer>().enabled = false;

        yield return new WaitForSeconds(0.4f);

        GetComponent<SpriteRenderer>().enabled = true;

        yield return new WaitForSeconds(0.4f);

        GetComponent<SpriteRenderer>().enabled = false;

        yield return new WaitForSeconds(0.4f);

        GetComponent<SpriteRenderer>().enabled = true;

        yield return new WaitForSeconds(0.4f);

        GetComponent<SpriteRenderer>().enabled = false;

        yield return new WaitForSeconds(0.4f);

        isInvincible = false;
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<PolygonCollider2D>().enabled = true;
    }

    void OnCollisionEnter2D(Collision2D c11)
    {
        
        if (c11.gameObject.tag == "Blocks")
        {
            if (isDashing)
            {
                if (BG.muted == false) audio.Play("Block Explosion");
                Instantiate(plus1, new Vector3(c11.gameObject.transform.position.x, c11.gameObject.transform.position.y + 1, -1), Quaternion.identity);
                GameObject BlockDeadParticles = Instantiate(ObstacleDestroyedWithDashParticles[(int)pp.currentBg], new Vector2(c11.transform.position.x, c11.transform.position.y), Quaternion.identity);
                Destroy(c11.gameObject);
            }
            else
            {
                TodesBlock = c11.gameObject;
                Death();
            }
        }
    }
}























