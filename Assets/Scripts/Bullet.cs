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

    public Nozzle nozzle;

    private bool dead = false;
    private int kill_counter = 30;
    private bool to_explode = false;

    private float top_boundary_y = 87f;
    private float bot_boundary_y = -96f;

    // Update is called once per frame
    void Update()
    {
        if (dead)
        {
            if (kill_counter > 0)
            {
                kill_counter--;
                if (kill_counter == 0)
                {
                    if (to_explode)
                    {
                        explode();
                    }
                    destroy_bullet();
                }
            }
            return;
        }

        if (frames > 1)
        {
            frame_counter = (frame_counter + 1) % frames;
            set_animation_frame();
        }

        if (gameObject.transform.position.y > top_boundary_y || gameObject.transform.position.y < bot_boundary_y)
        {
            pop(true);
        }
    }

    void FixedUpdate()
    {
        if (!dead)
        {
            Vector2 new_pos = (Vector2)(gameObject.transform.position + Vector3.up * (direction * speed));

            ((Rigidbody2D)gameObject.GetComponent(typeof(Rigidbody2D))).MovePosition(new_pos);

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
        List<Collider2D> hits = new List<Collider2D>();

        Vector2 stick_corner_a = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + -direction);
        Vector2 stick_corner_b = new Vector2(gameObject.transform.position.x + 3, gameObject.transform.position.y + 3 * -direction);

        hits.AddRange(Physics2D.OverlapAreaAll(stick_corner_a, stick_corner_b, Physics2D.DefaultRaycastLayers, 0, 0));

        Transform anim_pop = gameObject.transform.Find("anim_pop");
        SpriteRenderer pop_anim = (SpriteRenderer)anim_pop.GetComponent(typeof(SpriteRenderer));

        Texture2D explosion = pop_anim.sprite.texture;

        float left = anim_pop.position.x - (explosion.width / 2);
        float top = anim_pop.position.y - (explosion.height / 2);

        for (int i = 0; i < explosion.height; i++)
        {
            for (int u = 0; u < explosion.width; u++)
            {
                Vector2 origin = new Vector2(left + u + 0.5f, top + i + 0.5f);

                if (explosion.GetPixel(u, i).a == 1)
                {
                    Collider2D hit = Physics2D.OverlapCircle(origin, 0.2f, Physics2D.DefaultRaycastLayers, 0, 0);

                    if (hit != null)
                    {
                        hits.Add(hit);
                    }

                }
            }

        }

        foreach (Collider2D hit in hits){
            Transform barrier_hit = hit.transform;

            if (barrier_hit.CompareTag("UnbreakableBarrier"))
            {
                ((SpriteRenderer)barrier_hit.GetChild(0).GetComponent(typeof(SpriteRenderer))).enabled = false;
            }
            else if (barrier_hit.CompareTag("Barrier"))
            {
                GameObject.Destroy(hit.gameObject);
            }
        }
    }
    void pop(bool explode)
    {
        dead = true;
        to_explode = explode;

        frame_counter = -1;
        set_animation_frame();

        ((Collider2D)gameObject.GetComponent(typeof(Collider2D))).enabled = false;

        if (to_explode)
        {
            Transform anim_pop = gameObject.transform.Find("anim_pop");
            SpriteRenderer pop_anim = (SpriteRenderer)anim_pop.GetComponent(typeof(SpriteRenderer));
            pop_anim.enabled = true;
        }

    }

    void destroy_bullet()
    {
        nozzle.report_bullet_death();
        GameObject.Destroy(gameObject);
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
            kill_counter = 60;
            pop(false);

        } else if (collider.transform.CompareTag("Player"))
        {
            ((PlayerManager)collider.transform.gameObject.GetComponent(typeof(PlayerManager))).kill();
            pop(false);

        } else if (collider.transform.CompareTag("Bullet") || collider.transform.CompareTag("Barrier") || collider.transform.CompareTag("UnbreakableBarrier"))
        {
            pop(true);
        }
    }


}
