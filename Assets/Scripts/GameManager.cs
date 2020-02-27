using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private float left_boundary = -92f;
    private float right_boundary = 76f;

    private PlayerManager playermanager;

    public GameObject player;
    public EnemyManager enemymanager;

    // Start is called before the first frame update
    void Start()
    {
        do_restart();
        
    }

    void do_restart()
    {
        instantiate_player();
        enemymanager.restart_game();
    }

    void instantiate_player()
    {
        GameObject new_player = GameObject.Instantiate(player, Vector3.up * -88f, Quaternion.Euler(0, 0, 0));
        playermanager = (PlayerManager)new_player.GetComponent(typeof(PlayerManager));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float get_left_boundary()
    {
        return left_boundary;
    }

    public float get_right_boundary()
    {
        return right_boundary;
    }
}
