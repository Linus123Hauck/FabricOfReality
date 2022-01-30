using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour {

    bool choiceMade;
    SpriteRenderer bg;
    public MeshRenderer levelUpText;
    public GameObject attackDisplay;
    public GameObject statsDisplay;

    public List<AttackCard> cards;
    string[] statUpgrades = new string[] {"Damage Up","Speed Up","Better Attack Odds","Faster Attacks","More XP","Better Boost","Weaker Enemies" };

    // Use this for initialization
    void OnEnable () {
        bg = GetComponent<SpriteRenderer>();
        bg.color = new Color(0, 1, 1, 0);
        levelUpText.sortingOrder = 51;
        cID = ChooseCardID();
        sID = (int)Random.Range(0, statUpgrades.Length);
        FindObjectOfType<SoundManager>().playSound("levelUp");
    }

    int cID;
    int sID;

    float dark = 0;
	// Update is called once per frame
	void Update () {
        if(FindObjectOfType<GameOver>())
        {
            Time.timeScale = 1;
            return;
        }

        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > 0) dark -= 0.05f;
        else dark += 0.05f;
        dark = Mathf.Clamp(dark, 0, 1);

        if (!choiceMade)
        {
            Time.timeScale /= 1.05f;
            bg.color = new Color(dark, dark, dark, (1f - Time.timeScale) / 1.5f);
            levelUpText.GetComponent<TextMesh>().color = new Color(1-dark, 1-dark, 1-dark, 1f - Time.timeScale);

            if (Time.timeScale < 0.01 & !attackDisplay.activeInHierarchy)
            {
                attackDisplay.GetComponent<DisplayCard>().card = cards[cID];
                attackDisplay.GetComponent<DisplayCard>().cardName = cards[cID].name;
                attackDisplay.SetActive(true);
                statsDisplay.GetComponent<StatsUpgradeDisplay>().text = statUpgrades[sID];
                statsDisplay.SetActive(true);
            }

            if(Time.timeScale < 0.01 & Input.GetMouseButtonDown(0) & attackDisplay.activeInHierarchy)
            {
                choiceMade = true;
                FindObjectOfType<SoundManager>().playSound("upgrade");
                Destroy(statsDisplay);
                Destroy(attackDisplay);
                if(Camera.main.ScreenToWorldPoint(Input.mousePosition).x > 0)
                {
                    // Stats
                    LevelUpSystem stats = FindObjectOfType<LevelUpSystem>();
                    switch (sID)
                    {
                        case 0:
                            stats.damageStat = Mathf.Clamp(stats.damageStat + 5, 1, 100);
                            break;
                        case 1:
                            stats.speedStat = Mathf.Clamp(stats.speedStat + 5, 1, 100);
                            break;
                        case 2:
                            stats.attackOddsStat = Mathf.Clamp(stats.attackOddsStat + 5, 1, 100);
                            break;
                        case 3:
                            stats.attackSpeedStat = Mathf.Clamp(stats.attackSpeedStat + 5, 1, 100);
                            break;
                        case 4:
                            stats.xpGainStat = Mathf.Clamp(stats.xpGainStat + 5, 1, 100);
                            break;
                        case 5:
                            stats.boostStat = Mathf.Clamp(stats.boostStat + 5, 1, 100);
                            break;
                        case 6:
                            stats.enemyHealth = Mathf.Clamp(stats.enemyHealth + 5, 1, 100);
                            break;
                    }
                }
                else
                {
                    // Card
                    FindObjectOfType<AttackCardSpawner>().cards.Add(attackDisplay.GetComponent<DisplayCard>().card);
                }
            }
        }
        else
        {
            Time.timeScale = Mathf.Clamp(Time.timeScale + 0.09f, 0f, 1f);
            if(Time.timeScale >= 1f)
            {
                Destroy(gameObject);
            }
            bg.color = new Color(dark, dark, dark, (1f - Time.timeScale) / 1.5f);
            levelUpText.GetComponent<TextMesh>().color = new Color(1 - dark, 1 - dark, 1 - dark, 1f - Time.timeScale);

        }
    }

    int ChooseCardID()
    {
        float totalWeight = 0;
        foreach (AttackCard card in cards)
        {
            totalWeight += card.weight;
        }
        totalWeight *= Random.value;
        int id = 0;
        foreach (AttackCard card in cards)
        {
            totalWeight -= card.weight;
            if (totalWeight < 0) return id;
            id++;
        }
        return 0;
    }

    public static float Map(float a, float b, float c, float d, float e)
    {
        if (b == c || d == e) return d;
        return d + ((a - b) / (c - b)) * (e - d);
    }
}
