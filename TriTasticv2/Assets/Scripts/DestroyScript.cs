using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyScript : MonoBehaviour
{
    [Header("Over Time")]
    public bool destroyOverTime;
    public float timeToDestroy;

    [Header("Over Distance")]
    public bool destroyOverDistance;
    public float distanceToDestroy;
    void Start()
    {
        if (destroyOverTime)
            Destroy(this.gameObject, timeToDestroy);
    }

    
    void Update()
    {
        if(destroyOverDistance)
        {
            if (transform.position.y < distanceToDestroy)
                Destroy(this.gameObject);
        }
    }
}
