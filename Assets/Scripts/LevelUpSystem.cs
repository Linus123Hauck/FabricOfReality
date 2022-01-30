using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpSystem : MonoBehaviour {

    public float damageStat;
    public float speedStat;
    public float attackOddsStat;
    public float attackSpeedStat;
    public float xpGainStat;
    public float boostStat;
    public float enemyHealth;

    public GameObject levelUpScreen;

    public TextMesh text;
    public float xp;
    float xpNeeded;
    public int level;

	// Use this for initialization
	void OnEnable () {
        level = 1;
        xp = 0;
        xpNeeded = 5;
        text.GetComponent<MeshRenderer>().sortingOrder = 41;
        text.text = "1";
    }
	
	// Update is called once per frame
	void FixedUpdate () {
      
        while(xp > xpNeeded)
        {
            level++;
            xp -= xpNeeded;
            xpNeeded += 5f + xpNeeded / 10f;
            text.text = level + "";

            damageStat = Mathf.Clamp(damageStat + .5f, 1, 100);
            speedStat = Mathf.Clamp(speedStat + .5f, 1, 100);
            attackOddsStat = Mathf.Clamp(attackOddsStat + .5f, 1, 100);
            attackSpeedStat = Mathf.Clamp(attackSpeedStat + .5f, 1, 100);
            xpGainStat = Mathf.Clamp(xpGainStat + .5f, 1, 100);
            boostStat = Mathf.Clamp(boostStat + .5f, 1, 100);
            enemyHealth = Mathf.Clamp(enemyHealth + .5f, 1, 100);

            Instantiate(levelUpScreen);
        }
        transform.localScale = new Vector3(Map(xp, 0, xpNeeded, 0, 30), 1, 1);
    }

    public static float Map(float a, float b, float c, float d, float e)
    {
        if (b == c || d == e) return d;
        return d + ((a - b) / (c - b)) * (e - d);
    }
}
