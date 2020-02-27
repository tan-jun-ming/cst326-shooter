using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private float left_boundary;
    private float right_boundary;

    private Nozzle nozzle;

    private GameManager gamemanager;


    // Start is called before the first frame update
    void Start()
    {
        gamemanager = (GameManager)GameObject.Find("GameManager").GetComponent(typeof(GameManager));
        left_boundary = gamemanager.get_left_boundary();
        right_boundary = gamemanager.get_right_boundary();

        nozzle = (Nozzle)gameObject.transform.Find("Nozzle").GetComponent(typeof(Nozzle));
    }

    // Update is called once per frame
    void Update()
    {
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
        if (z > 0)
        {
            nozzle.fire();
        }
    }
}
