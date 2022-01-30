using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {


    public AudioSource source;
    public AudioClip[] explode;
    public AudioClip[] xp;
    public AudioClip gameOver;
    public AudioClip levelUp;
    public AudioClip upgrade;
    public AudioClip boostHit;

    // Use this for initialization
    void OnEnable () {
    }
    

    public void playSound(string name)
    {
        switch (name)
        {
            case "explode":
                source.PlayOneShot(explode[(int)Random.Range(0, explode.Length)], 1f);
                break;
            case "xp":
                source.PlayOneShot(xp[(int)Random.Range(0, xp.Length)], 1f);
                break;
            case "gameOver":
                source.Stop();
                source.PlayOneShot(gameOver, 1f);
                break;
            case "levelUp":
                source.PlayOneShot(levelUp, 1f);
                break;
            case "upgrade":
                source.PlayOneShot(upgrade, 1f);
                break;
            case "BoostHit":
                source.PlayOneShot(boostHit, 1f);
                break;
        }
    }
}
