using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nozzle : MonoBehaviour
{
    public float direction = 0f;
    public int bullet_limit = 0;
    private int bullet_count = 0;

    public float bullet_speed = 2f;

    public GameObject[] bullets;

    [HideInInspector]
    public int shooter;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void fire()
    {
        if (bullet_limit > 0 && bullet_count >= bullet_limit)
        {
            return;
        }

        GameObject bullet = bullets[Random.Range(0, bullets.Length)];
        GameObject new_bullet = GameObject.Instantiate(bullet, gameObject.transform.position, Quaternion.Euler(0, 0, 0));

        Bullet bullet_script = (Bullet)new_bullet.GetComponent(typeof(Bullet));
        bullet_script.direction = direction;
        bullet_script.shooter = shooter;
        bullet_script.nozzle = this;
        bullet_script.speed = bullet_speed;

        bullet_count++;
    }

    public void report_bullet_death()
    {
        bullet_count--;
    }
}
