using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayCard : MonoBehaviour {

    public AttackCard card;
    public string cardName;

    // Use this for initialization
    void OnEnable () {
        SpriteRenderer bulletSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        bulletSpriteRenderer.sprite = card.bulletSprite;
        bulletSpriteRenderer.color = card.color;
        TextMesh textMesh = transform.GetChild(1).GetComponent<TextMesh>();
        textMesh.text = card.text;
        textMesh.color = card.color;
        transform.GetChild(1).GetComponent<MeshRenderer>().sortingOrder = 52;
        transform.GetChild(2).GetComponent<MeshRenderer>().sortingOrder = 52;
        transform.GetChild(2).GetComponent<TextMesh>().text = cardName;
        GetComponent<SpriteRenderer>().color = card.color;
    }

    float nameAlpha = 0;

	// Update is called once per frame
	void Update () {
        if (transform.localScale.x < 0.075f)
        {
            transform.localScale = new Vector3(transform.localScale.x + 0.0075f, transform.localScale.y + 0.0075f, 1);
        }
        else
        {
            Vector2 cameraPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float dist = Vector2.Distance(cameraPos, transform.position);
            if (dist < 20)
            {
                float s = Map(dist, 0, 20, 0.3f, 0.075f);
                transform.localScale = new Vector3(s, s, 1);
            }
            else transform.localScale = new Vector3(0.075f, 0.075f, 1);
            if (dist < 10)
            {
                nameAlpha += 0.07f;
            }
            else nameAlpha -= 0.07f;
            nameAlpha = Mathf.Clamp(nameAlpha, 0, 1);
            transform.GetChild(2).GetComponent<TextMesh>().color = new Color(0, 0, 0, nameAlpha);
        }
    }


    public static float Map(float a, float b, float c, float d, float e)
    {
        if (b == c || d == e) return d;
        return d + ((a - b) / (c - b)) * (e - d);
    }
}
