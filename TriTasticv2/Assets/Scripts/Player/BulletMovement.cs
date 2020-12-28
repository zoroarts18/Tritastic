using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletMovement : MonoBehaviour
{
    [Header("Stats")]
    public float speed;

    [Header("Assign")]
    public GameManager gameManager;
    public GameObject PlayerDeathVFX;
    public GameObject plus1;

    void Start()
    {
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //-----------------------------------------------Shoot Gamemode:------------------------------------------------------------------------------------
        if(gameManager.GameMode == 2)
        {
            if (collision.gameObject.tag == "Blocks")
            {
                Instantiate(PlayerDeathVFX, transform.position, Quaternion.identity);
                Instantiate(plus1, new Vector3(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y , -1), Quaternion.identity);
                gameManager.IncrementScore();
                Destroy(this.gameObject);
                Destroy(collision.gameObject);
            }
        }

        //-----------------------------------------------Arcade Gamemode:------------------------------------------------------------------------------------
        else
        {
            if (collision.gameObject.tag == "Blocks")
            {
                Destroy(this.gameObject);
                collision.gameObject.GetComponent<DestroyBlock>().TakeDamage();
            }
        }
    }
    public void Update()
    {
        //---------------------------------------------Movement & Death----------------------------------------------
        transform.Translate(Vector2.up * 20 * Time.deltaTime);
        if (transform.position.y >= 10) Destroy(this.gameObject);

    }
}
