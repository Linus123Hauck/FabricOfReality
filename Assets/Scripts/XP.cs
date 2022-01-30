using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XP : MonoBehaviour {

    Transform playerPos;
    bool collect;
    float despawnTime;
    private LevelUpSystem stats;

	// Use this for initialization
	void OnEnable () {
        transform.position = new Vector2(transform.position.x + Random.Range(-1, 1), transform.position.y + Random.Range(-1, 1));
        transform.localScale = new Vector3(Random.Range(0.4f, 0.7f), Random.Range(0.4f, 0.7f));
        collect = false;
        despawnTime = 1000;
        stats = FindObjectOfType<LevelUpSystem>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        despawnTime--;
        transform.Rotate(new Vector3(0, 0, despawnTime / 100f));
		if(collect)
        {
            transform.position = Vector2.Lerp(transform.position, playerPos.position, 0.25f);
            if(Vector2.Distance(playerPos.position, transform.position) < 1)
            {
                FindObjectOfType<SoundManager>().playSound("xp");
                stats.xp += Map(stats.xpGainStat,1,100,1,20);
                Destroy(gameObject);
            }
        }
        if(despawnTime < 0) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collect = true;
            playerPos = collision.gameObject.transform;
            GetComponent<CircleCollider2D>().enabled = false;
        }
    }

    public static float Map(float a, float b, float c, float d, float e)
    {
        if (b == c || d == e) return d;
        return d + ((a - b) / (c - b)) * (e - d);
    }
}
