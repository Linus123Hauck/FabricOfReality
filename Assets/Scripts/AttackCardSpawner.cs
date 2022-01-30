using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCardSpawner : MonoBehaviour {

    public List<AttackCard> cards;
    float nextCardAt = 0;
    private LevelUpSystem stats;

    public SpriteRenderer spawnerSprite;
    public SpriteRenderer despawnerSprite;

	// Use this for initialization
	void OnEnable () {
        nextCardAt = Time.fixedTime;
        stats = FindObjectOfType<LevelUpSystem>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if(nextCardAt <= Time.fixedTime)
        {
            spawnerSprite.color = new Color(1, 1, 1, .5f);
            float conveyorSpeed = Map(stats.attackSpeedStat, 1, 100, 1, 10);
            AttackCard newCard = Instantiate(ChooseCard());
            nextCardAt += 1f / conveyorSpeed;
        }
        else if(spawnerSprite.color.a > 0.1)
        {
            spawnerSprite.color = new Color(1, 1, 1, spawnerSprite.color.a - 0.05f);
        }
        if (despawnerSprite.color.a > 0.1)
        {
            despawnerSprite.color = new Color(1, 1, 1, despawnerSprite.color.a - 0.05f);
        }
    }

    AttackCard ChooseCard()
    {
        float totalWeight = 0;
        float addWeight = Map(stats.attackOddsStat, 1, 100, 0, 50);
        foreach(AttackCard card in cards)
        {
            totalWeight += card.weight + addWeight;
        }
        totalWeight *= Random.value;
        cards.Sort((a, b) => 1 - 2 * Random.Range(0, 1));
        foreach (AttackCard card in cards)
        {
            totalWeight -= card.weight + addWeight;
            if (totalWeight < 0) return card;
        }
        return cards[0];
    }

    public static float Map(float a, float b, float c, float d, float e)
    {
        if (b == c || d == e) return d;
        return d + ((a - b) / (c - b)) * (e - d);
    }
}
