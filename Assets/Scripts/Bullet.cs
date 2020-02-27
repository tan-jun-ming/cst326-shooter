using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float direction = 0f;
    public float speed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 new_pos = (Vector2)(gameObject.transform.position + Vector3.up * (direction * speed));

        ((Rigidbody2D)gameObject.GetComponent(typeof(Rigidbody2D))).MovePosition(new_pos);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.transform.CompareTag("Enemy"))
        {
            ((Enemy)collider.transform.gameObject.GetComponent(typeof(Enemy))).kill();
            GameObject.Destroy(gameObject);
        }
    }

}
