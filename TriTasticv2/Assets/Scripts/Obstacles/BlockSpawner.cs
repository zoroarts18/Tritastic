using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockSpawner : MonoBehaviour
{
    public GameObject Player;
    public GameManager GameManager;

    [Header("Obstacle Prefabs")]
    public GameObject Ring;
    public GameObject Enemy;
    public GameObject Block;

    [Header("Item Prefabs")]
    public GameObject BoostItem;
    public GameObject ShootItem;

    [Header("Spawn Logic")]
    public float timeToSpawn = .5f;
    public float timeBetweenSpawn;
    public float TimeToSpawnItem = 2f;
    public float TimeBetweenItemSpawns = 2f;
    public float currentSpeed;
    public Transform[] spawnPoints;

    public Sprite[] blockSprites;

    public PlayerProfile playerProfile;
    public void Update()
    {
        if (Time.time >= timeToSpawn)
        {
            if (GameManager.GetComponent<GameManager>().GameIsPlayed == true)
            { 
                    SpawnBlocks();
                    timeToSpawn = Time.time + timeBetweenSpawn;
            }
        }
    }

    public void GetProfile()
    {
        playerProfile = SaveManager.Load();
    }

    void SpawnBlocks()
    {
        if(Player.GetComponent<FollowFingerScript>().isDead != true)
        {

            int randomIndex = Random.Range(0, spawnPoints.Length);
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                //-----------------------------------------------------------------------Rings Mode:-----------------------------------------------------------
                #region Rings Gamemode Logic:
                if (GameManager.GetComponent<GameManager>().GameMode == 1)
                {
                    if (randomIndex == i)
                    {
                        //----------------------------------------------------------------Spawns a Ring---------------------------------------------------------
                        GameObject RingInGame = Instantiate(Ring, spawnPoints[i].position, Quaternion.identity);
                        RingInGame.tag = "Rings";
                        RingInGame.GetComponent<DestroyBlock>().gameManager = GameManager;
                        RingInGame.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -currentSpeed);
                    }

                    else
                        TimeToSpawnItem = Time.time + TimeBetweenItemSpawns;
                }
                #endregion

                //-----------------------------------------------------------------------Shoot Mode:-----------------------------------------------------------
                #region Shoot Gamemode Logic
                if (GameManager.GetComponent<GameManager>().GameMode == 2)
                {
                    if (randomIndex == i)
                    {
                        //----------------------------------------------------------------Spawns a Enemy---------------------------------------------------------
                        GameObject EnemyInGame = Instantiate(Enemy , spawnPoints[i].position, Quaternion.identity);
                        EnemyInGame.tag = "Blocks";
                        EnemyInGame.GetComponent<DestroyBlock>().gameManager = GameManager;
                        EnemyInGame.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -currentSpeed);
                    }

                    else
                    {
                        TimeToSpawnItem = Time.time + TimeBetweenItemSpawns;
                    }

                }
                #endregion

                //-----------------------------------------------------------------------Arcade Mode:-----------------------------------------------------------
                #region Arcade Gamemode Logic:
                if (GameManager.GetComponent<GameManager>().GameMode == 0)
                {
                    if (randomIndex != i)
                    {
                        //----------------------------------------------------------------Spawns a Block---------------------------------------------------------
                        GameObject block = Instantiate(Block, spawnPoints[i].position, Quaternion.identity);
                        //................Wenn Hintergrund Rot, sind Blöcke dunkelrot.....................................
                        if (playerProfile.currentBg == Background.SolidRed)
                        {
                            block.GetComponent<SpriteRenderer>().sprite = blockSprites[0];
                        }
                        //................Wenn Hintergrund Grün, sind Blöcke dunkelgrün.....................................
                        if (playerProfile.currentBg == Background.SolidGreen)
                        {
                            block.GetComponent<SpriteRenderer>().sprite = blockSprites[1];
                        }
                        //................Wenn Hintergrund Blau, sind Blöcke dunkelBlau.....................................
                        if (playerProfile.currentBg == Background.SolidBlue)
                        {
                            block.GetComponent<SpriteRenderer>().sprite = blockSprites[2];
                        }
                        //................Wenn Hintergrund Grau, sind Blöcke dunkelGrau.....................................
                        if (playerProfile.currentBg == Background.SolidGrey)
                        {
                            block.GetComponent<SpriteRenderer>().sprite = blockSprites[3];
                        }

                        block.tag = "Blocks";
                        block.GetComponent<DestroyBlock>().gameManager = GameManager;
                        block.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -currentSpeed);
                    }

                    //--------------------------------------------------------------Item Spawning:-----------------------------------------------------------------
                    else
                    {
                        if (GameManager.GetComponent<GameManager>().score >= 10 && Time.time >= TimeToSpawnItem)
                        {
                            float FortuneNumber = Random.Range(0f, 1f);

                            if (FortuneNumber < 0.4f)
                            {
                                if (Player.GetComponent<FollowFingerScript>().isDashing == false && Player.GetComponent<FollowFingerScript>().isShooting == false)
                                {
                                    if (FortuneNumber <= 0.2f)
                                    {
                                        GameObject BoostItemSpawned = Instantiate(BoostItem, spawnPoints[i].position, Quaternion.identity);
                                        BoostItemSpawned.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -currentSpeed);
                                    }

                                    else
                                    {
                                        GameObject ShootItemSpawned = Instantiate(ShootItem, spawnPoints[i].position, Quaternion.identity);
                                        ShootItemSpawned.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -currentSpeed);
                                    }
                                }
                            }
                            TimeToSpawnItem = Time.time + TimeBetweenItemSpawns;
                        }
                    }
                }
                #endregion
            }
        }   
    }

    public void setSpeed(float speed)
    {
        currentSpeed = speed;

        List<GameObject> ToBeUpdated = new List<GameObject>();

        ToBeUpdated.AddRange(GameObject.FindGameObjectsWithTag("Blocks"));

        ToBeUpdated.AddRange(GameObject.FindGameObjectsWithTag("Boost"));

        foreach (GameObject blockInGame in ToBeUpdated)
        {
            blockInGame.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -currentSpeed);
        }
    }
}
