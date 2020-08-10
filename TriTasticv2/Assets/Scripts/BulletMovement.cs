using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    
    public float speed;
    private Rigidbody2D rb;

    





    // Update is called once per frame
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision on Bullet");
        if (collision.gameObject.tag == "Blocks")
        {
            Destroy(this.gameObject);
            collision.gameObject.GetComponent<DestroyBlock>().TakeDamage();
        }
            
    }
    public void Update()
    {
        transform.Translate(Vector2.up * 20 * Time.deltaTime);
        if (transform.position.y == 6)
            Destroy(this.gameObject);
    }
}
