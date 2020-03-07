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
    private int kill_timer = 120;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        gamemanager = (GameManager)GameObject.Find("GameManager").GetComponent(typeof(GameManager));
        left_boundary = gamemanager.get_left_boundary();
        right_boundary = gamemanager.get_right_boundary();

        nozzle = (Nozzle)gameObject.transform.Find("Nozzle").GetComponent(typeof(Nozzle));

        animator = ((Animator)gameObject.transform.Find("anim").GetComponent(typeof(Animator)));
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
            bool fired = nozzle.fire();
            if (fired)
            {
                animator.SetTrigger("fire");
                shooting = true;
            }
        } else if (z == 0)
        {
            shooting = false;
        }
    }

    public void silent_remove()
    {
        GameObject.Destroy(gameObject);
    }

    public void kill()
    {
        kill(false);
    }

    public void kill(bool forced)
    {
        if (!dead)
        {
            dead = true;
            animator.SetTrigger("die");

            if (!forced)
            {
                gamemanager.report_player_death();
            }
        }
    }
}
