using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI score_display;
    public TMPro.TextMeshProUGUI highscore_display;
    public TMPro.TextMeshProUGUI gameover_display;

    private int score = 50;
    private int highscore = 50;

    // Start is called before the first frame update
    void Start()
    {
        update_score();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void update_score()
    {
        update_text(score_display, pad_score(score));
        update_text(highscore_display, pad_score(highscore));
    }

    public void add_score(int new_score)
    {
        score += new_score;
        if (score > highscore)
        {
            highscore = score;
        }

        update_score();
    }

    public void set_highscore(int new_highscore)
    {
        highscore = new_highscore;
    }

    public int get_highscore()
    {
        return highscore;
    }

    string pad_score(int score)
    {
        return score.ToString().PadLeft(4, "0"[0]);
    }

    void update_text(TMPro.TextMeshProUGUI display, string text)
    {
        display.text = "<mspace=8px>" + text + "</mspace>";
    }
}
