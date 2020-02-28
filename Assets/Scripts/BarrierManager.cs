using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierManager : MonoBehaviour
{
    private float top_boundary = 96f;
    private float mid_boundary = -98f;
    private float bot_boundary = -104f;

    private float barrier_start_y = -64f;

    private int hor_boundary = 111;

    private GameManager gamemanager;

    public GameObject barrier;

    private bool[,] barrier_shape = new bool[,] {
        {false, false, false, false,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true, false, false, false, false},
        {false, false, false,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true, false, false, false},
        {false, false,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true, false, false},
        {false,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true, false},
        { true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true},
        { true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true},
        { true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true},
        { true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true},
        { true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true},
        { true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true},
        { true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true},
        { true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true,  true},
        { true,  true,  true,  true,  true,  true,  true, false, false, false, false, false, false, false,  true,  true,  true,  true,  true,  true,  true,  true},
        { true,  true,  true,  true,  true,  true, false, false, false, false, false, false, false, false, false,  true,  true,  true,  true,  true,  true,  true},
        { true,  true,  true,  true,  true, false, false, false, false, false, false, false, false, false, false, false,  true,  true,  true,  true,  true,  true},
        { true,  true,  true,  true,  true, false, false, false, false, false, false, false, false, false, false, false,  true,  true,  true,  true,  true,  true}
    };
    private int barrier_width = 22;
    private int barrier_height = 16;

    private float barrier_start_x = -83f;
    private float barrier_pad_x = 24f;
    private int barrier_count = 4;

    public void initialize_barriers()
    {
        foreach (Transform child in gameObject.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        for (int i = -hor_boundary; i<hor_boundary; i++)
        {
            Vector3 new_barrier_pos = Vector3.right * i;

            GameObject top_barrier = GameObject.Instantiate(barrier, new_barrier_pos + Vector3.up * top_boundary, Quaternion.Euler(0, 0, 0), gameObject.transform);
            GameObject mid_barrier = GameObject.Instantiate(barrier, new_barrier_pos + Vector3.up * mid_boundary, Quaternion.Euler(0, 0, 0), gameObject.transform);
            GameObject bot_barrier = GameObject.Instantiate(barrier, new_barrier_pos + Vector3.up * bot_boundary, Quaternion.Euler(0, 0, 0), gameObject.transform);

            top_barrier.tag = "UnbreakableBarrier";
            mid_barrier.tag = "UnbreakableBarrier";
            bot_barrier.tag = "UnbreakableBarrier";

            ((SpriteRenderer)top_barrier.transform.GetChild(0).GetComponent(typeof(SpriteRenderer))).enabled = false;
            ((SpriteRenderer)mid_barrier.transform.GetChild(0).GetComponent(typeof(SpriteRenderer))).enabled = false;
        }

        for (int c = 0; c < barrier_count; c++)
        {
            for (int i=0; i< barrier_height; i++)
            {
                for (int u=0; u<barrier_width; u++)
                {
                    if (barrier_shape[i, u])
                    {
                        Vector3 new_barrier_pos = new Vector3(barrier_start_x + u + ((barrier_width + barrier_pad_x) * c), barrier_start_y - i, 0);
                        GameObject.Instantiate(barrier, new_barrier_pos, Quaternion.Euler(0, 0, 0), gameObject.transform);
                    }
                }
            }
        }
    }
}
