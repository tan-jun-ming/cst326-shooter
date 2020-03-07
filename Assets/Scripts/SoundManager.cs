using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum SoundType {PlayerShoot, PlayerDie, EnemyShoot, EnemyDie, UfoEnter };

    public AudioClip playershoot;
    public AudioClip playerdie;
    public AudioClip enemyshoot;
    public AudioClip enemydie;
    public AudioClip ufoenter;

    public void play_sound(SoundType soundtype)
    {
        AudioSource source = (AudioSource)Camera.main.gameObject.GetComponent(typeof(AudioSource));
        AudioClip to_play = null;

        switch (soundtype)
        {
            case SoundType.PlayerShoot: to_play = playershoot; break;
            case SoundType.PlayerDie: to_play = playerdie; break;
            case SoundType.EnemyShoot: to_play = enemyshoot; break;
            case SoundType.EnemyDie: to_play = enemydie; break;
            case SoundType.UfoEnter: to_play = ufoenter; break;
        }

        source.PlayOneShot(to_play, 1f);
    }
}
