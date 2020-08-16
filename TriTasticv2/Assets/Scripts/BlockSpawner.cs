using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockSpawner : MonoBehaviour
{
    public UIManager uiManager;

    public GameObject BoostItem;

    public GameObject ShootItem;

    public GameObject Ring;
    public GameObject Enemy;

    public Transform[] spawnPoints;    

    public GameObject blockPrefab;

    private float timeToSpawn = .5f;

    public float timeBetweenSpawn;

    public GameObject controller;

    public float currentSpeed;

    public float TimeToSpawnItem = 2f;
    public float TimeBetweenItemSpawns = 2f;

    
    public GameObject Player;

    public void Update()
    {
        if (Time.time >= timeToSpawn)
        {
        //wenn Game is Played , dann Spawnt block!!!

            if (controller.GetComponent<ControlManagerScript>().GameIsPlayed == true)
            { 
                    SpawnBlocks();
                    timeToSpawn = Time.time + timeBetweenSpawn;
            }
        }
    }

    void SpawnBlocks()
    {
        //ein zufälliger Spawner bleib ohne block die anderen 2 spawnen einen block
        if(Player.GetComponent<FollowFingerScript>().isDead != true)
        {
            int randomIndex = Random.Range(0, spawnPoints.Length);

            for (int i = 0; i < spawnPoints.Length; i++)
            {
                if(controller.GetComponent<ControlManagerScript>().GameMode == 1)
                {
                    if (randomIndex == i)
                    {
                        //Ring wird gespawnt!
                        GameObject RingInGame = Instantiate(Ring, spawnPoints[i].position, Quaternion.identity);
                        RingInGame.tag = "Rings";
                        RingInGame.GetComponent<DestroyBlock>().uiManager = uiManager;
                        RingInGame.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -currentSpeed);
                    }

                    else
                        TimeToSpawnItem = Time.time + TimeBetweenItemSpawns;
                }

                if(controller.GetComponent<ControlManagerScript>().GameMode == 2)
                {
                    if (randomIndex != i)
                    {
                        //Block wird gespawnt!
                        GameObject EnemyInGame = Instantiate(Enemy , spawnPoints[i].position, Quaternion.identity);
                        EnemyInGame.tag = "Blocks";
                        EnemyInGame.GetComponent<DestroyBlock>().uiManager = uiManager;
                        EnemyInGame.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -currentSpeed);
                    }

                    else
                    {
                        TimeToSpawnItem = Time.time + TimeBetweenItemSpawns;
                    }

                }

                if(controller.GetComponent<ControlManagerScript>().GameMode == 0)
                {
                    if (randomIndex != i)
                    {
                        //Block wird gespawnt!
                        GameObject block = Instantiate(blockPrefab, spawnPoints[i].position, Quaternion.identity);
                        block.tag = "Blocks";
                        block.GetComponent<DestroyBlock>().uiManager = uiManager;
                        block.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -currentSpeed);
                    }

                   

                

                    else
                    {
                        if (uiManager.GetComponent<UIManager>().score >= 10 && Time.time >= TimeToSpawnItem)
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
