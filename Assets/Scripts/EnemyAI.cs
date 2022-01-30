using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

    public GameObject gameOverScreen;
    public Transform playerLocation;
    public ParticleSystem deathBoom;
    public ParticleSystem playerDeathBoom;
    public ParticleSystem bulletCollideBoom;
    public ParticleSystem spawnEffect;
    public GameObject xp;
    public float difficulty = 1;
    private float health;
    private int actionID = 0; // 0 = wait, 1 = move, 2 = charge
    private float lastActionAt = 0;
    private float nextActionAt = 0;
    private float oldRot;
    private float newRot;
    private float vel;

    BoxCollider lineOfSight;

	// Use this for initialization
	void OnEnable () {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            playerLocation = GameObject.FindGameObjectWithTag("Player").transform;
            oldRot = Mathf.Atan2(playerLocation.position.y - transform.position.y, playerLocation.position.x - transform.position.x);
            oldRot += Random.Range(-0.2f, 0.2f);
            newRot = oldRot;
        }
        health = (1f + difficulty / 1.5f) / Map(FindObjectOfType<LevelUpSystem>().enemyHealth,1,100,1,10);
        Instantiate(spawnEffect, transform.position, Quaternion.EulerAngles(0, 0, newRot));
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if(playerLocation == null) return;
        Vector2 pos = transform.position;
        float rot = Map(Time.fixedTime, lastActionAt, nextActionAt, oldRot, newRot);

        pos.x += vel * Mathf.Cos(rot);
        pos.y += vel * Mathf.Sin(rot);

        while (Vector2.Distance(Vector2.zero, pos) > 40)
        {
            pos = Vector2.MoveTowards(pos, Vector2.zero, vel / 3f);
        }

        transform.position = pos;
        transform.rotation = Quaternion.EulerAngles(0,0,rot);

        if (Time.fixedTime > nextActionAt)
        {
            lastActionAt = Time.fixedTime;
            actionID = (actionID + 1) % 2;
            oldRot = newRot;
            if (actionID == 0)
            {
                nextActionAt = lastActionAt + Random.Range(0.5f, 1) * Map(Mathf.Clamp(difficulty,1,100),1,100,1.5f,0.05f);
                vel = 0;
                Vector2 playerPos = playerLocation.transform.position;
                newRot = Mathf.Atan2(playerPos.y - pos.y, playerPos.x - pos.x) + Random.Range(-0.2f, 0.2f); ;
            }
            else
            {
                vel = Mathf.Clamp(0.3f + (difficulty / 50f)  * Random.Range(0.5f, 1), 0.3f, 1.5f);
                nextActionAt = lastActionAt + Random.Range(0.5f, 1) * Map(Mathf.Clamp(difficulty, 1, 100), 1, 100, 1, 0.05f);
            }
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform == playerLocation)
        {
            if(collision.GetComponent<PlayerMove>().boosting)
            {
                health -= 3f + FindObjectOfType<LevelUpSystem>().boostStat / 3f;
                FindObjectOfType<SoundManager>().playSound("BoostHit");
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ScreenShake>().Shake(0.1f, 0.08f);
                if (health <= 0)
                {
                    Instantiate(deathBoom, transform.position, Quaternion.identity);
                    for (int i = Mathf.RoundToInt(Random.Range(3, 4)); i > 0; i--) Instantiate(xp, transform.position, Quaternion.identity);
                    GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ScreenShake>().Shake(0.5f, 0.15f);
                    FindObjectOfType<SoundManager>().playSound("explode");
                    Destroy(gameObject);
                }
            }
            else
            {
                Instantiate(playerDeathBoom, playerLocation.position, Quaternion.identity);
                playerLocation.gameObject.SetActive(false);
                Instantiate(gameOverScreen);
                Instantiate(deathBoom, transform.position, Quaternion.identity);
                Destroy(gameObject);
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ScreenShake>().Shake(3f, 1f);
            }
        }
        else if(collision.gameObject.GetComponent<Bullet>().enabled)
        {
            health -= collision.gameObject.GetComponent<Bullet>().damage;
            Instantiate(bulletCollideBoom, collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            if(health <= 0)
            {
                Instantiate(deathBoom, transform.position, Quaternion.identity);
                for(int i = Mathf.RoundToInt(Random.Range(2,3)); i > 0; i--) Instantiate(xp, transform.position, Quaternion.identity);
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ScreenShake>().Shake(0.5f, 0.15f);
                FindObjectOfType<SoundManager>().playSound("explode");
                Destroy(gameObject);
            }
            else GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ScreenShake>().Shake(0.1f, 0.05f);
        }
    }

    public static float Map(float a, float b, float c, float d, float e)
    {
        if (b == c || d == e) return d;
        return d + ((a - b) / (c - b)) * (e - d);
    }
}
