using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCard : MonoBehaviour {

    public float weight = 10;
    public Sprite bulletSprite;
    private GameObject player;
    public GameObject attackPattern;
    public Color color;
    public string text;
    public AudioClip sound;
    private LevelUpSystem stats;
    public ParticleSystem DestroyEffect;
    bool despawn;

	// Use this for initialization
	void OnEnable () {
        player = GameObject.FindGameObjectWithTag("Player");
        SpriteRenderer bulletSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        bulletSpriteRenderer.sprite = bulletSprite;
        bulletSpriteRenderer.color = color;
        TextMesh textMesh = transform.GetChild(1).GetComponent<TextMesh>();
        textMesh.text = text;
        textMesh.color = color;
        transform.GetChild(1).GetComponent<MeshRenderer>().sortingOrder = 7;
        GetComponent<SpriteRenderer>().color = color;
        stats = FindObjectOfType<LevelUpSystem>();
        despawn = false;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if(despawn)
        {
            transform.position = new Vector2(-55, 30);
            transform.localScale = new Vector3(transform.localScale.x - 1, transform.localScale.y - 1, 1);
            if(transform.localScale.x <= 0)
            {
                ParticleSystem effect = Instantiate(DestroyEffect, transform.position, Quaternion.identity);
                effect.startColor = new Color(color.r,color.g,color.b,0.6f);
                GameObject.FindGameObjectWithTag("DespawnSprite").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1,0.5f);
                player.GetComponent<PlayerMove>().Shoot(attackPattern);
                FindObjectOfType<SoundManager>().source.PlayOneShot(sound, 0.5f);
                Destroy(gameObject);
            }
        }
        else if(transform.localScale.x < 10)
        {
            transform.localScale = new Vector3(transform.localScale.x + 1, transform.localScale.y + 1, 1);
        }
        else
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + Map(stats.attackSpeedStat, 1, 100, 0.2f, 2));
            if (transform.position.y > 30)
            {
                transform.position = new Vector2(-55, 30);
                if (!player) return;
                despawn = true;
            }
        }
	}

    public static float Map(float a, float b, float c, float d, float e)
    {
        if (b == c || d == e) return d;
        return d + ((a - b) / (c - b)) * (e - d);
    }
}
