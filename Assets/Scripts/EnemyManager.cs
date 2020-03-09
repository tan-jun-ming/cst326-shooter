using System.Collections;
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
    private float bot_boundary = -88f;

    private float enemy_width = 12f;
    private float enemy_height = 8f;
    private float enemy_pad_x = 4f;
    private float enemy_pad_y = 8f;

    private int direction = 1;

    private float ufo_start_y = 88f;
    private int ufo_dir = 1;
    private bool ufo_active = false;
    private int ufo_throttle = 2;
    private int ufo_step_counter = 0;

    private GameObject active_ufo;

    private List<List<Enemy>> formation = new List<List<Enemy>>();
    private List<int> shootable = new List<int>();

    private int max_freeze = 70;
    private int freeze = 0;

    private int enemy_step_counter = 0;
    private bool turn = false;

    private int fire_cooldown_max = 60;
    private int fire_cooldown;

    private int ufo_cooldown_max = 1000;
    private int ufo_cooldown;

    private bool game_running = false;

    private GameManager gamemanager;

    // Start is called before the first frame update
    void Start()
    {

        gamemanager = (GameManager)GameObject.Find("GameManager").GetComponent(typeof(GameManager));
        left_boundary = gamemanager.get_left_boundary();
        right_boundary = gamemanager.get_right_boundary();
    }

    public void game_over()
    {
        game_running = false;
    }

    public void clear_formation()
    {
        //Clear arrays
        while (formation.Count > 0)
        {
            int i = formation.Count - 1;
            while (formation[i].Count > 0)
            {
                int u = formation[i].Count - 1;
                GameObject.Destroy(formation[i][u].gameObject);
                formation[i].RemoveAt(u);
            }
            formation.RemoveAt(i);
        }

        shootable.Clear();

    }
    public void restart_game()
    {
        game_running = true;
        fire_cooldown = fire_cooldown_max;
        ufo_cooldown = ufo_cooldown_max;

        formation_count = formation_width * formation_height;

        clear_formation();

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

        if (!game_running)
        {
            return;
        }

        if (freeze > 0)
        {
            freeze--;

            if (freeze == 0)
            {
                change_formation_animation_state(true);
            }
            return;
        }

        step_enemies();
        
        if (fire_cooldown > 0)
        {
            fire_cooldown--;
            if (fire_cooldown == 0)
            {
                fire_cooldown = fire_cooldown_max;
                fire();
            }
        }

        if (ufo_cooldown > 0 && !ufo_active && formation_count > 9)
        {
            ufo_cooldown--;
            if (ufo_cooldown == 0)
            {
                ufo_cooldown = ufo_cooldown_max;
                spawn_ufo();
            }
        }

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

        ufo_step_counter = ufo_throttle;


        if (ufo_dir == 0)
        {
            ufo_dir = -1;
            ufo_start_pos.x = right_boundary;
        }

        active_ufo = GameObject.Instantiate(enemy_ufo, ufo_start_pos, Quaternion.Euler(0, 0, 0), gameObject.transform);

        ((SpriteRenderer)active_ufo.transform.Find("anim").GetComponent(typeof(SpriteRenderer))).flipX = ufo_dir > 0;

        Enemy ufo = (Enemy)active_ufo.GetComponent(typeof(Enemy));
        ufo.formation_x = -1;
        ufo.formation_y = -1;
        ufo.manager = this;
        ufo.max_death_counter = max_freeze;
        ufo.points_worth = Random.Range(1, 4) * 50;

        ((SoundManager)GameObject.Find("SoundManager").GetComponent(typeof(SoundManager))).play_sound(SoundManager.SoundType.UfoEnter);

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

        ufo_step_counter = (ufo_step_counter + 1) % ufo_throttle;

        if (ufo_step_counter == 0)
        {
            ((Enemy)active_ufo.GetComponent(typeof(Enemy))).step(ufo_dir);
        }

    }

    public void report_death(Enemy victim)
    {
        int x = victim.formation_x;
        int y = victim.formation_y;

        int points = victim.points_worth;

        gamemanager.add_score(points);

        if (x < 0)
        {
            ufo_active = false;
            return;
        }

        formation_count--;
        set_freeze(max_freeze, x, y);

        shootable[x] = get_bottom_enemy(x);

        if (formation_count <= 0)
        {
            gamemanager.next_round();
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

    void fire()
    {
        List<int> possible_columns = new List<int>();

        for (int i=0; i<shootable.Count; i++)
        {
            if (shootable[i] >= 0)
            {
                possible_columns.Add(i);
            }
        }

        if (possible_columns.Count == 0)
        {
            return;
        }

        int shooting_column = possible_columns[Random.Range(0, possible_columns.Count)];
        Enemy shooter = formation[shootable[shooting_column]][shooting_column];

        shooter.fire();
    }

    public void set_freeze(int num, int x, int y)
    {
        freeze = num;
        change_formation_animation_state(false, x, y);

    }
    public void set_freeze(int num)
    {
        set_freeze(num, -1, -1);
    }

    void change_formation_animation_state(bool state, int x, int y)
    {
        for (int i = 0; i < formation.Count; i++)
        {
            for (int u=0; u<formation[i].Count; u++)
            {
                if (i != y || u != x)
                {
                    formation[i][u].animator.enabled = state;
                }
            }
        }
    }

    void change_formation_animation_state(bool state)
    {
        change_formation_animation_state(state, -1, -1);
    }

    public void confirm_breach()
    {
        gamemanager.force_game_over();
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

        ret.bot_boundary = bot_boundary;

        ret.max_death_counter = max_freeze;
        ret.y_step = enemy_height;

        ret.manager = this;

        return ret;
    }
}
