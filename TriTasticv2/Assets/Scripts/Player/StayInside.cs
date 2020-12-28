using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayInside : MonoBehaviour
{
    

    
    void Update()
    {
        //Spieler bleibt innerhalb eines bestimmten bereichs in der Main Kamera

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -3f, 3f),
            Mathf.Clamp(transform.position.y, -2.5f, 1f), transform.position.z);
    }
}
