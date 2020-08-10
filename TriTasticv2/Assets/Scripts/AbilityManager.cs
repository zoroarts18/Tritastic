using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{

    public GameObject Player;
    public GameObject PowerUpVFX;

    public BackGroundMusicManagement BG;
    void Awake()
    {
        BG = GameObject.FindWithTag("BGMusic").GetComponent<BackGroundMusicManagement>();
        Player = GameObject.FindWithTag("Player");
    }
    
    void OnTriggerEnter2D(Collider2D c)
    {
        if(c.gameObject.tag == "Player")
        {
            

            if(this.gameObject.tag == "Boost")
            {
                if (BG.muted == false)
                {
                    FindObjectOfType<AudioManager>().Play("Power Up");
                }

                Instantiate(PowerUpVFX, c.gameObject.transform.position, Quaternion.identity);
                Debug.Log("Boost");
                Player.GetComponent<FollowFingerScript>().Dash();
                Destroy(this.gameObject);
            }
            else
            {
                if (BG.muted == false)
                {
                    FindObjectOfType<AudioManager>().Play("Power Up");
                }
                Instantiate(PowerUpVFX, c.gameObject.transform.position, Quaternion.identity);
                Player.GetComponent<FollowFingerScript>().ActivateShoot();
                Destroy(this.gameObject);
            }

            
        }
    }
}
