using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int frames = 0;
    private int frame_counter = 0;

    public float direction = 0f;
    public float speed = 2f;

    public int shooter;

    private bool dead = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dead)
        {
            return;
        }

        Vector2 new_pos = (Vector2)(gameObject.transform.position + Vector3.up * (direction * speed));

        ((Rigidbody2D)gameObject.GetComponent(typeof(Rigidbody2D))).MovePosition(new_pos);

        if (frames > 1)
        {
            frame_counter = (frame_counter + 1) % frames;
            set_animation_frame();
        }
    }

    void set_animation_frame()
    {
        for (int i = 0; i < frames; i++)
        {
            SpriteRenderer sprite = (SpriteRenderer)gameObject.transform.Find("anim_" + (i + 1)).GetComponent(typeof(SpriteRenderer));
            sprite.enabled = i == frame_counter;
        }
    }
    void explode()
    {
        dead = true;
        frame_counter = -1;
        set_animation_frame();

        ((Collider2D)gameObject.GetComponent(typeof(Collider2D))).enabled = false;
        ((SpriteRenderer)gameObject.transform.Find("anim_pop").GetComponent(typeof(SpriteRenderer))).enabled = true;

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.GetInstanceID() == shooter)
        {
            return;
        }
        if (collider.transform.CompareTag("Enemy"))
        {
            ((Enemy)collider.transform.gameObject.GetComponent(typeof(Enemy))).kill();
            GameObject.Destroy(gameObject);
        } else if (collider.transform.CompareTag("Player"))
        {
            ((PlayerManager)collider.transform.gameObject.GetComponent(typeof(PlayerManager))).kill();
            GameObject.Destroy(gameObject);
        } else if (collider.transform.CompareTag("Bullet"))
        {
            explode();
        }
    }

}
