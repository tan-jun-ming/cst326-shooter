﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private float left_boundary = -92f;
    private float right_boundary = 76f;

    private PlayerManager playermanager;

    public GameObject player;
    public EnemyManager enemymanager;
    public UIManager uimanager;
    public BarrierManager barriermanager;
    public Transform bulletholder;

    private int next_round_timer_max = 500;
    private int next_round_timer = 0;

    private int player_respawn_timer_max = 200;
    private int player_respawn_timer = 0;

    private int lives_max = 3;
    private int lives = 3;

    private Data data;

    // Start is called before the first frame update
    void Start()
    {
        do_restart();
        
    }

    void do_restart()
    {
        do_restart(true);
    }
    void do_restart(bool next_round)
    {
        if (next_round)
        {
            lives = lives_max;
            uimanager.set_lives(lives);
        }
        uimanager.restart_game(next_round);
        empty_bullets();
        instantiate_player();
        enemymanager.restart_game();
        barriermanager.initialize_barriers();
    }

    void instantiate_player()
    {
        GameObject new_player = GameObject.Instantiate(player, Vector3.up * -88f + Vector3.right * left_boundary, Quaternion.Euler(0, 0, 0));
        playermanager = (PlayerManager)new_player.GetComponent(typeof(PlayerManager));
    }

    // Update is called once per frame
    void Update()
    {
        if (player_respawn_timer > 0)
        {
            player_respawn_timer--;
            if (player_respawn_timer == 0)
            {
                if (lives > 0)
                {
                    instantiate_player();
                }
            }
        }

        if (player_respawn_timer == 0 && lives == 0)
        {
            if (Input.GetAxis("Fire1") > 0)
            {
                SceneManager.LoadScene("CreditsScene");
            }
        }

        if (next_round_timer > 0)
        {
            next_round_timer--;
            if (next_round_timer == 0)
            {
                do_restart(false);
            }
        }
    }

    public float get_left_boundary()
    {
        return left_boundary;
    }

    public float get_right_boundary()
    {
        return right_boundary;
    }

    public void add_score(int score)
    {
        uimanager.add_score(score);
    }

    public void force_game_over()
    {
        playermanager.kill(true);
        do_game_over();
    }

    public void next_round()
    {
        next_round_timer = next_round_timer_max;
        playermanager.silent_remove();
    }

    void empty_bullets()
    {
        foreach (Transform child in bulletholder)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void report_player_death()
    {
        lives--;
        uimanager.set_lives(lives);

        if (lives == 0)
        {
            do_game_over();

        } else
        {
            enemymanager.set_freeze(player_respawn_timer_max);
        }
        player_respawn_timer = player_respawn_timer_max;
        
    }

    void do_game_over()
    {
        lives = 0;
        uimanager.show_game_over();
        enemymanager.game_over();
    }
}
