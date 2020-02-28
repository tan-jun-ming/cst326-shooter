using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool is_ufo;
    public int points_worth;

    [HideInInspector]
    public EnemyManager manager;

    public bool dead = false;

    [HideInInspector]
    public int formation_x;
    [HideInInspector]
    public int formation_y;

    private bool animation_step = false;

    private SpriteRenderer anim_1;
    private SpriteRenderer anim_2;
    private SpriteRenderer anim_pop;
    private Collider2D col;

    [HideInInspector]
    public int max_death_counter;
    private int death_counter = -1;

    [HideInInspector]
    public float y_step;

    private Nozzle nozzle;


    // Start is called before the first frame update
    void Start()
    {

        anim_1 = ((SpriteRenderer)gameObject.transform.Find("anim_1").GetComponent(typeof(SpriteRenderer)));

        if (!is_ufo)
        {
            nozzle = (Nozzle)gameObject.transform.Find("Nozzle").GetComponent(typeof(Nozzle));
            nozzle.shooter = gameObject.GetInstanceID();
            anim_2 = ((SpriteRenderer)gameObject.transform.Find("anim_2").GetComponent(typeof(SpriteRenderer)));
        }

        anim_pop = ((SpriteRenderer)gameObject.transform.Find("anim_pop").GetComponent(typeof(SpriteRenderer)));
        col = (Collider2D)gameObject.GetComponent(typeof(Collider2D));
    }

    // Update is called once per frame
    void Update()
    {
        if (death_counter > 0)
        {
            death_counter--;
            if (death_counter == 0)
            {
                anim_pop.enabled = false;

                gameObject.transform.position = Vector3.one * -1000f;
                //GameObject.Destroy(gameObject);
            }
        }
    }

    public void step(int direction)
    {
        step(direction, false);
    }

    public void step(int direction, bool step_down)
    {
        if (dead)
        {
            return;
        }

        Vector3 to_translate = Vector3.right * direction;

        if (step_down)
        {
            to_translate.y = -y_step;
        }

        gameObject.transform.Translate(to_translate);
        step_anim();

        if (gameObject.transform.position.y <= -64f)
        {
            za_hando();
        }
    }

    public Vector3 get_pos()
    {
        return gameObject.transform.position;
    }

    public void step_anim()
    {
        if (!is_ufo)
        {
            animation_step = !animation_step;

            anim_1.enabled = !animation_step;
            anim_2.enabled = animation_step;
        }
    }

    void za_hando()
    {
        
        Vector2 top_left = new Vector2(gameObject.transform.position.x - 1, gameObject.transform.position.y);
        Vector2 bottom_right = new Vector2(gameObject.transform.position.x + 13, gameObject.transform.position.y - 8);

        Collider2D[] hit = Physics2D.OverlapAreaAll(top_left, bottom_right, Physics2D.DefaultRaycastLayers, 0, 0);

        foreach (Collider2D col in hit)
        {
            Transform barrier_hit = col.transform;

            if (barrier_hit.CompareTag("UnbreakableBarrier"))
            {
                ((SpriteRenderer)barrier_hit.GetChild(0).GetComponent(typeof(SpriteRenderer))).enabled = false;
            }
            else if (barrier_hit.CompareTag("Barrier"))
            {
                GameObject.Destroy(barrier_hit.gameObject);
            }
        }

    }

    public void fire()
    {
        if (!dead)
        {
            nozzle.fire();
        }
    }

    public void kill()
    {
        dead = true;
        col.enabled = false;

        anim_1.enabled = false;
        if (!is_ufo)
        {
            anim_2.enabled = false;
        }
        anim_pop.enabled = true;

        death_counter = max_death_counter;
        manager.report_death(this);
    }
}
