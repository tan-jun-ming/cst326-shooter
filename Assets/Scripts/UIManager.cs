using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI score_display;
    public TMPro.TextMeshProUGUI highscore_display;
    public TMPro.TextMeshProUGUI gameover_display;
    public TMPro.TextMeshProUGUI life_display;

    public SpriteRenderer life_1;
    public SpriteRenderer life_2;

    private int score = 0;
    private int highscore = 0;
    private int lives = 3;

    private Data data;

    // Start is called before the first frame update
    void Start()
    {
        data = (Data)GameObject.Find("Data").GetComponent(typeof(Data));
        highscore = data.highscore;
        restart_game();
    }

    public void restart_game()
    {
        restart_game(true);
    }
    public void restart_game(bool refresh_score)
    {

        if (refresh_score)
        {
            score = 0;
            update_score();
        }
        update_lives();
        hide_game_over();
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
            data.highscore = highscore;
        }

        update_score();
    }

    public void set_lives(int new_lives)
    {
        lives = new_lives;
        update_lives();
    }

    void update_lives()
    {
        update_text(life_display, lives.ToString());

        life_1.enabled = lives > 1;
        life_2.enabled = lives > 2;
    }

    public void show_game_over()
    {
        gameover_display.enabled = true;
    }

    void hide_game_over()
    {
        gameover_display.enabled = false;
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
