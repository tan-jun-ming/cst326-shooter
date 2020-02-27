using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nozzle : MonoBehaviour
{
    public float direction = 0f;
    public int cooldown = 0;

    public GameObject[] bullets;

    private int current_cooldown = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (current_cooldown > 0)
        {
            current_cooldown--;
        }
    }

    public void fire()
    {
        if (current_cooldown > 0)
        {
            return;
        }

        current_cooldown = cooldown;

        GameObject bullet = bullets[Random.Range(0, bullets.Length)];
        GameObject new_bullet = GameObject.Instantiate(bullet, gameObject.transform.position, Quaternion.Euler(0, 0, 0));
        ((Bullet)new_bullet.GetComponent(typeof(Bullet))).direction = direction;
    }
}
