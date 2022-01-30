using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public Sprite[] enemySprites;
    public GameObject enemy;
    public float difficulty;

    public string difficultySetting;

    public float nextEnemyAt;

	// Use this for initialization
	void OnEnable () {
        nextEnemyAt = Time.fixedTime + 300f;
        difficulty = 1;
        difficultySetting = GlobalVals._globalDifficulty;

        switch (difficultySetting)
        {
            case "Too Easy":
                difficulty = 1;
                break;
            case "Easy":
                difficulty = 1;
                break;
            case "Normal":
                difficulty = 1;
                break;
            case "Hard":
                difficulty = 1.25f;
                break;
            case "Harder":
                difficulty = 1.5f;
                break;
            case "Hardest":
                difficulty = 1.75f;
                break;
            case "Hardester":
                difficulty = 2f;
                break;
            case "!?!?":
                difficulty = 4f;
                break;
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        switch(difficultySetting)
        {
            case "Too Easy":
                difficulty += 0.0001f;
                break;
            case "Easy":
                difficulty += 0.0006f;
                break;
            case "Normal":
                difficulty += 0.0008f;
                break;
            case "Hard":
                difficulty += 0.0009f;
                break;
            case "Harder":
                difficulty += 0.001f;
                break;
            case "Hardest":
                difficulty += 0.0011f;
                break;
            case "Hardester":
                difficulty += 0.0012f;
                break;
            case "!?!?":
                difficulty += 0.0015f;
                break;
        }

        if (FindObjectsOfType<EnemyAI>().Length == 0) nextEnemyAt -= 0.9f;

        if(nextEnemyAt <= Time.fixedTime)
        {
            int amount = Mathf.Clamp((int)Random.Range(difficulty / 2f, difficulty / 1.5f), 1, 75);    

            for (int i = 0; i < amount; i++)
            {
                float dir = Random.Range(0, Mathf.PI * 2f);

                Vector2 spawnPos = new Vector2(35f * Mathf.Sin(dir), 35f * Mathf.Cos(dir));
                GameObject newEnemy = Instantiate(enemy, spawnPos, Quaternion.identity);
                newEnemy.GetComponent<EnemyAI>().difficulty = difficulty;
                newEnemy.GetComponent<SpriteRenderer>().sprite = enemySprites[(int)Random.Range(0, enemySprites.Length)];
                newEnemy.SetActive(true);
                nextEnemyAt = Time.fixedTime + 4f + (100f / difficulty) * Random.Range(0.75f, 1);
            }
        }
    }
}
