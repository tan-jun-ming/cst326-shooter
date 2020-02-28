using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private float left_boundary;
    private float right_boundary;

    private Nozzle nozzle;

    private GameManager gamemanager;

    private bool shooting = false;

    public bool dead = false;
    private int kill_timer = 40;

    private SpriteRenderer anim;
    private SpriteRenderer anim_pop_1;
    private SpriteRenderer anim_pop_2;

    private bool death_anim_step = false;

    // Start is called before the first frame update
    void Start()
    {
        gamemanager = (GameManager)GameObject.Find("GameManager").GetComponent(typeof(GameManager));
        left_boundary = gamemanager.get_left_boundary();
        right_boundary = gamemanager.get_right_boundary();

        nozzle = (Nozzle)gameObject.transform.Find("Nozzle").GetComponent(typeof(Nozzle));

        anim = (SpriteRenderer)gameObject.transform.Find("anim").GetComponent(typeof(SpriteRenderer));
        anim_pop_1 = (SpriteRenderer)gameObject.transform.Find("anim_pop_1").GetComponent(typeof(SpriteRenderer));
        anim_pop_2 = (SpriteRenderer)gameObject.transform.Find("anim_pop_2").GetComponent(typeof(SpriteRenderer));
    }

    // Update is called once per frame
    void Update()
    {
        if (dead)
        {
            kill_timer--;
            if (kill_timer == 0)
            {
                GameObject.Destroy(gameObject);
            }

            if (kill_timer % 4 == 0)
            {
                death_anim_step = !death_anim_step;

                anim_pop_1.enabled = death_anim_step;
                anim_pop_2.enabled = !death_anim_step;
            }

            return;
        }
        float x = Input.GetAxis("Horizontal");

        Vector3 new_pos = gameObject.transform.position;

        if (x < 0)
        {
            new_pos.x = Mathf.Max(new_pos.x - 1f, left_boundary);
        } else if (x > 0)
        {
            new_pos.x = Mathf.Min(new_pos.x + 1f, right_boundary);
        }

        gameObject.transform.position = new_pos;

        float z = Input.GetAxis("Fire1");
        if (z > 0 && !shooting)
        {
            nozzle.fire();
            shooting = true;
        } else if (z == 0)
        {
            shooting = false;
        }
    }

    public void kill()
    {
        if (!dead)
        {
            dead = true;
            anim.enabled = false;
            anim_pop_1.enabled = true;

            gamemanager.report_player_death();
        }
    }
}
