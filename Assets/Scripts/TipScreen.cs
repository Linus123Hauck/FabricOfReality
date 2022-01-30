using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipScreen : MonoBehaviour {

    float duration;
    public TextMesh txt;
    public SpriteRenderer background;
    public string[] tips;

    // Use this for initialization
    void OnEnable() {
        duration = 3;
        txt.GetComponent<MeshRenderer>().sortingOrder = 81;
        txt.text = "tip:\n\n" + tips[(int)Random.Range(0, tips.Length)].Replace("_","\n");
        if (GlobalVals._globalDifficulty == "!?!?") txt.text = "Tip:\n\ncry";
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        duration -= 0.01f;
        if(duration < 1)
        {
            background.color = new Color(0, 0, 0, duration);
            txt.color = new Color(1, 1, 1, duration);
            if (duration <= 0) Destroy(gameObject);
        }
	}
}
