using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    private Data data = null;

    public int highscore = 0;
    public int background = -1;

    public void Start()
    {

        if (data != null)
        {
            GameObject.Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        data = this;
    }
    
}
