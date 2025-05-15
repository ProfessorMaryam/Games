using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    public AudioSource playerChannel;
    public AudioClip playerHurt;
    public AudioClip playerDie;
    public AudioClip playerPass;

    public AudioSource ghostChannel;
    //public AudioClip ghostHurt;
    public AudioClip ghostAttack;
    public AudioClip ghostDie;
    public AudioClip ghostWalk;
    public AudioClip ghostRun;

    public AudioSource BossChannel;
    //public AudioClip BossHurt;
    public AudioClip BossDie;
    public AudioClip BossAttack;
    public AudioClip BossWalk;
    public AudioClip BossRise;
    public AudioClip BossFire;
    public AudioClip BossCover;
    public AudioClip BGuitarHit;
    public AudioClip BGuitarStruck;

    public AudioSource spawnChannel;
    public AudioClip spawnSound;

    public AudioSource ObjectChannel;
    public AudioClip mgSound;
    public AudioClip crashSound;
    public AudioClip playMeSound;
    public AudioClip contraSound;
    public AudioClip collectSound;
    public AudioClip trophySound;

    public AudioSource MoonChannel;

    public AudioSource finaleChannel;
    public AudioClip myFinaleSong;

    public AudioSource TargetChannel;
    

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
