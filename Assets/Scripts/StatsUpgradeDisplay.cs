using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsUpgradeDisplay : MonoBehaviour {

    public string text;

	// Use this for initialization
	void OnEnable () {
        GetComponent<MeshRenderer>().sortingOrder = 52;
        GetComponent<TextMesh>().text = text;
    }

    float alpha;

	// Update is called once per frame
	void Update () {

        if(transform.localScale.x < 0.005f)
        {
            transform.localScale = new Vector3(transform.localScale.x + 0.0005f, transform.localScale.y + 0.0005f, 1);
        }
        else
        {
            Vector2 cameraPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float dist = Vector2.Distance(cameraPos, transform.position);
            if (dist < 20)
            {
                float s = Map(dist, 0, 20, 0.01f, 0.005f);
                transform.localScale = new Vector3(s, s, 1);
            }
            else transform.localScale = new Vector3(0.005f, 0.005f, 1);
            if (dist < 10)
            {
                alpha += 0.07f;
            }
            else alpha -= 0.07f;
            alpha = Mathf.Clamp(alpha, 0.2f, 1);
            GetComponent<TextMesh>().color = new Color(1, 1, 1, alpha);
        }
    }


    public static float Map(float a, float b, float c, float d, float e)
    {
        if (b == c || d == e) return d;
        return d + ((a - b) / (c - b)) * (e - d);
    }
}
