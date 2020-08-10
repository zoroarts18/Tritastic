using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CoinSpawner : MonoBehaviour
{
    public Transform[] CoinspawnPoints;
    public GameObject CoinPrefab;
    private float timeToSpawnCoin = 2f;
    public float timeBetweenSpawnCoin = 1f;




    void Update()
    {
        if (Time.time >= timeToSpawnCoin)
        {
            SpawnBlocks();
            timeToSpawnCoin = Time.time + timeBetweenSpawnCoin;
        }
    }

    void SpawnBlocks()
    {
        //ein zufälliger Spawner bleib ohne block die anderen 2 spawnen einen block

        int randomIndex = Random.Range(0, CoinspawnPoints.Length);

        for (int i = 0; i < CoinspawnPoints.Length; i++)
        {
            if (randomIndex != i)
            {
                //Block wird gespawnt!

                Instantiate(CoinPrefab, CoinspawnPoints[i].position, Quaternion.identity);
            }
        }

    }
}

