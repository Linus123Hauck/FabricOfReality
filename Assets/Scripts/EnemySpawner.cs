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
        difficultySetting = GlobalVals._globalDifficulty;

        switch (difficultySetting)
        {
            case "Too Easy":
                difficulty = 1;
                break;
            case "Easy":
                difficulty = 2;
                break;
            case "Normal":
                difficulty = 4;
                break;
            case "Hard":
                difficulty = 5;
                break;
            case "Harder":
                difficulty = 6;
                break;
            case "Hardest":
                difficulty = 7;
                break;
            case "Hardester":
                difficulty = 8;
                break;
            case "!?!?":
                difficulty = 10;
                break;
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        switch(difficultySetting)
        {
            case "Too Easy":
                difficulty += 0.001f;
                break;
            case "Easy":
                difficulty += 0.0014f;
                break;
            case "Normal":
                difficulty += 0.0015f;
                break;
            case "Hard":
                difficulty += 0.0016f;
                break;
            case "Harder":
                difficulty += 0.0017f;
                break;
            case "Hardest":
                difficulty += 0.0018f;
                break;
            case "Hardester":
                difficulty += 0.0019f;
                break;
            case "!?!?":
                difficulty += 0.002f;
                break;
        }


        int enemyCount = FindObjectsOfType<EnemyAI>().Length;
        if (enemyCount == 0) nextEnemyAt -= 0.9f;

        if(enemyCount < 30 & nextEnemyAt <= Time.fixedTime)
        {

            int amount = Mathf.Clamp((int)Random.Range(difficulty / 2f, difficulty / 1.5f), 1, 25);

            switch (difficultySetting)
            {
                case "Too Easy":
                    break;
                case "Easy":
                    break;
                case "Normal":
                    break;
                case "Hard":
                    amount = (int)(amount + 1.5f);
                    break;
                case "Harder":
                    amount = (int)(amount + 2f);
                    break;
                case "Hardest":
                    amount = (int)(amount + 2.5f);
                    break;
                case "Hardester":
                    amount = (int)(amount + 3f);
                    break;
                case "!?!?":
                    amount = (int)(amount + 5f);
                    break;
            }

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
