﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public GameObject[] enemies;
    public GameObject enemy_ufo;

    private int formation_width = 11;
    private int formation_height = 5;

    private int formation_count;

    private float left_boundary;
    private float right_boundary;
    private float top_boundary = 56f;
    private float bot_boundary = 100f;

    private float enemy_width = 12f;
    private float enemy_height = 8f;
    private float enemy_pad_x = 4f;
    private float enemy_pad_y = 8f;

    private int direction = 1;

    private float ufo_start_y = 88f;
    private int ufo_dir = 1;
    private bool ufo_active = false;
    private GameObject active_ufo;

    private List<List<Enemy>> formation = new List<List<Enemy>>();
    private List<int> shootable = new List<int>();

    private int max_freeze = 70;
    private int freeze = 0;

    private int enemy_step_counter = 0;
    private bool turn = false;

    private GameManager gamemanager;

    // Start is called before the first frame update
    void Start()
    {

        gamemanager = (GameManager)GameObject.Find("GameManager").GetComponent(typeof(GameManager));
        left_boundary = gamemanager.get_left_boundary();
        right_boundary = gamemanager.get_right_boundary();

        formation_count = formation_width * formation_height;
    }

    public void restart_game()
    {
        //Clear arrays
        while (formation.Count > 0)
        {
            int i = formation.Count - 1;
            while (formation[i].Count > 0)
            {
                int u = formation[i].Count - 1;
                GameObject.Destroy(formation[i][u]);
                formation[i].RemoveAt(u);
            }
            formation.RemoveAt(i);
        }

        shootable.Clear();

        for (int i = 0; i < formation_height; i++)
        {
            formation.Add(new List<Enemy>());
            for (int u = 0; u < formation_width; u++)
            {
                if (i == 0)
                {
                    shootable.Add(formation_height - 1);
                }
                formation[i].Add(initialize_enemy(i, u, calculate_enemy_type(i)));
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (ufo_active)
        {
            step_ufo();
        }

        if (Input.GetMouseButtonDown(0))
        {

            float distance = 25f;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, distance);

            if (hit)
            {
                if (hit.transform.CompareTag("Enemy"))
                {
                    ((Enemy)hit.transform.gameObject.GetComponent(typeof(Enemy))).kill();
                }
            }

        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            spawn_ufo();
        }

        if (freeze > 0)
        {
            freeze--;
            return;
        }

        step_enemies();

    }

    void step_enemies()
    {
        int counter = -1;
        bool stepped = false;

        for (int i = formation_height-1; i >= 0; i--)
        {
            for (int u = 0; u < formation_width; u++)
            {
                counter++;

                if (counter >= enemy_step_counter)
                {
                    enemy_step_counter = counter + 1;

                    Enemy enemy = formation[i][u];
                    if (!enemy.dead)
                    {
                        enemy.step(direction, turn);
                        stepped = true;
                        break;
                    }
                }
            }
            if (stepped)
            {
                break;
            }
        }

        if (!stepped)
        {
            enemy_step_counter = 0;

            if (turn)
            {
                turn = false;
            }

            for (int i = 0; i < formation_height; i++)
            {
                for (int u = 0; u < formation_width; u++)
                {
                    Enemy en = formation[i][u];

                    if (en.dead)
                    {
                        continue;
                    }
                    if (direction < 0)
                    {
                        if (en.get_pos().x <= left_boundary)
                        {
                            change_direction();
                            break;
                        }
                    }
                    else if (direction > 0)
                    {
                        if (en.get_pos().x >= right_boundary)
                        {
                            change_direction();
                            break;
                        }
                    }
                }
                
            }
        }
    }

    void change_direction()
    {
        turn = true;
        direction *= -1;
    }

    void spawn_ufo()
    {
        if (ufo_active)
        {
            return;
        }

        ufo_active = true;

        ufo_dir = Random.Range(0, 2);
        Vector3 ufo_start_pos = Vector3.up * ufo_start_y;
        ufo_start_pos.x = left_boundary;

        if (ufo_dir == 0)
        {
            ufo_dir = -1;
            ufo_start_pos.x = right_boundary;
        }

        active_ufo = GameObject.Instantiate(enemy_ufo, ufo_start_pos, Quaternion.Euler(0, 0, 0), gameObject.transform);
        Enemy ufo = (Enemy)active_ufo.GetComponent(typeof(Enemy));
        ufo.formation_x = -1;
        ufo.formation_y = -1;
        ufo.manager = this;
        ufo.max_death_counter = max_freeze;
        ufo.points_worth = Random.Range(1, 4) * 50;

    }

    void step_ufo()
    {
        if (!ufo_active)
        {
            return;
        }

        if ( (ufo_dir == -1 && active_ufo.transform.position.x <= left_boundary) ||
            (ufo_dir == 1 && active_ufo.transform.position.x >= right_boundary)
            )
        {
            GameObject.Destroy(active_ufo);
            ufo_active = false;
            return;
        }

        ((Enemy)active_ufo.GetComponent(typeof(Enemy))).step(ufo_dir);

    }
    public void report_death(Enemy victim)
    {
        int x = victim.formation_x - 1;
        int y = victim.formation_y - 1;

        int points = victim.points_worth;

        print(points + " Points!");

        if (x < 0)
        {
            ufo_active = false;
            return;
        }

        formation_count--;
        freeze = max_freeze;

        shootable[x] = get_bottom_enemy(x);

        if (formation_count <= 0)
        {
            print("Congrats everyone is dead.");
        }
    }

    int get_bottom_enemy(int column)
    {
        int ret = -1;

        for (int i=formation_height-1; i>=0; i--)
        {
            if (!formation[i][column].dead)
            {
                ret = i;
                break;
            }
        }

        return ret;


    }
    int calculate_enemy_type(int row)
    {
        int[] choices = { 0, 1, 1, 2, 2, 0, 0, 0, 0, 0, 0 };
        return choices[row]; // Maybe do proper calculations in the future
    }

    Enemy initialize_enemy(int row, int column, int type)
    {
        Vector3 new_coordinates = Vector3.zero;

        new_coordinates.x = left_boundary + column * (enemy_width + enemy_pad_x);
        new_coordinates.y = top_boundary - row * (enemy_height + enemy_pad_y);

        GameObject new_enemy = GameObject.Instantiate(enemies[type], new_coordinates, Quaternion.Euler(0, 0, 0), gameObject.transform);

        Enemy ret = (Enemy)new_enemy.GetComponent(typeof(Enemy));

        ret.formation_x = column;
        ret.formation_y = row;

        ret.max_death_counter = max_freeze;
        ret.y_step = enemy_height;

        ret.manager = this;

        return ret;
    }
}
