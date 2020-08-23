using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletMovement : MonoBehaviour
{
    
    public float speed;
    private Rigidbody2D rb;

    public ControlManagerScript CS;
    public UIManager uiManager;

    public GameObject PlayerDeathVFX;

    public GameObject plus1;

    // Update is called once per frame
    void Start()
    {
        uiManager = GameObject.FindWithTag("UIManager").GetComponent<UIManager>();

        CS = GameObject.FindWithTag("Controller").GetComponent<ControlManagerScript>();
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(CS.GameMode == 2)
        {
            if (collision.gameObject.tag == "Blocks")
            {
                Instantiate(PlayerDeathVFX, transform.position, Quaternion.identity);
                Instantiate(plus1, collision.gameObject.transform.position, Quaternion.identity);
                uiManager.IncrementScore();

                Destroy(this.gameObject);
                Destroy(collision.gameObject);
            }
        }

        else
        {
            Debug.Log("Collision on Bullet");
            if (collision.gameObject.tag == "Blocks")
            {
                
                Destroy(this.gameObject);
                collision.gameObject.GetComponent<DestroyBlock>().TakeDamage();
            }

        }

    }
    public void Update()
    {
        transform.Translate(Vector2.up * 20 * Time.deltaTime);
        if (transform.position.y >= 10)
            Destroy(this.gameObject);
    }
}
