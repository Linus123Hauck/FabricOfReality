using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    public GameObject boostParticles;
    float boostCooldown;
    bool boostActive;
    public bool boosting;
    private LevelUpSystem stats;

	// Use this for initialization
	void OnEnable () {
        stats = FindObjectOfType<LevelUpSystem>();
        boostCooldown = 0;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        Vector2 playerDir = Vector2.zero;
        if (Input.GetKey("w"))
        {
            playerDir.y += 1;
        }
        if (Input.GetKey("s"))
        {
            playerDir.y -= 1;
        }
        if (Input.GetKey("d"))
        {
            playerDir.x += 1;
        }
        if (Input.GetKey("a"))
        {
            playerDir.x -= 1;
        }

        float moveSpeed = Map(stats.speedStat, 1, 100, 1, 25);

        playerDir.Normalize();
        playerDir *= .3f + moveSpeed / 50f;
        if (Input.GetKey(KeyCode.Space) & boostActive & boostCooldown <= 10)
        {
            playerDir *= Map(stats.boostStat, 1, 100, 3, 4);
            boostCooldown += Map(stats.boostStat, 1, 100, 0.7f, 0.1f);
            Instantiate(boostParticles, transform.position, Quaternion.identity);
            boosting = true;
        }
        else
        {
            boosting = false;
            if (boostCooldown > 0)
            {
                boostActive = false;
                boostCooldown -= 0.1f;
            }
            else boostActive = !Input.GetKey(KeyCode.Space);
        }

        transform.position = new Vector2(transform.position.x + playerDir.x, transform.position.y + playerDir.y);
        

        while (Vector2.Distance(Vector2.zero, transform.position) > 35)
        {
            transform.position = Vector2.MoveTowards(transform.position, Vector2.zero, moveSpeed / 9f);
        }

        float rot = 5f;
        rot += Mathf.Abs(playerDir.x) * 20f;
        rot += Mathf.Abs(playerDir.y) * 20f;

        transform.Rotate(new Vector3(0, 0, rot));

	}

    public void Shoot(GameObject attackPattern)
    {
        Instantiate(attackPattern).GetComponent<BulletSpawner>().enabled = true;
    }

    public static float Map(float a, float b, float c, float d, float e)
    {
        if (b == c || d == e) return d;
        return d + ((a - b) / (c - b)) * (e - d);
    }

}
