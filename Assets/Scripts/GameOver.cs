using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

    TextMesh text;
    SpriteRenderer background;
    float alpha = 0;

	// Use this for initialization
	void OnEnable () {
        text = transform.GetChild(0).GetComponent<TextMesh>();
        transform.GetChild(0).GetComponent<MeshRenderer>().sortingOrder = 1000;
        background = GetComponent<SpriteRenderer>();
        background.color = new Color(background.color.r, background.color.g, background.color.b, 0);
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        FindObjectOfType<SoundManager>().playSound("gameOver");
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (alpha < 1)
        {
            if (Input.GetMouseButton(0)) alpha += 0.03f;
            alpha += 0.003f;
            background.color = new Color(background.color.r, background.color.g, background.color.b, alpha);
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            text.text = "Game Over";
        }
        else if(Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            string extraText = "";
            if (GlobalVals._globalDifficulty == "Too Easy") extraText = "wait.. you actually died?";
            if (GlobalVals._globalDifficulty == "!?!?") extraText = "what were you expecting?";
            text.text = "Game Over\n\nYou made it to level " + FindObjectOfType<LevelUpSystem>().level + "!\n\n" + extraText + "\n\nClick to Continue";
        }
    }
}
