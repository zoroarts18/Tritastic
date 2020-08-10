using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnerScript : MonoBehaviour
{
    public GameObject BoostItem;
    public GameObject ShootItem;

    public float TimeToSpawnItem = 2f;
    public float TimeBetweenItemSpawns = 2f;

    public GameObject UIManager;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(UIManager.GetComponent<UIManager>().score >= 10 && Time.time >= TimeToSpawnItem)
        {
            float FortuneNumber = Random.Range(0f, 1f);

           
            if(FortuneNumber < 0.05f )
            {

                
                Instantiate(BoostItem, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
                
            }
            TimeToSpawnItem = Time.time + TimeBetweenItemSpawns;



        }
    }
}
