using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    public Text dText;
    public ScreenShake camShake;
    public SpriteRenderer bgLight;
    public AudioClip chooseSound;

    public void Start()
    {
        Cursor.visible = true;
        camShake = FindObjectOfType<ScreenShake>();
    }

    public void Update()
    {
        float shake = Mathf.Pow(difficulty / 4f, 2f);
        dText.transform.localPosition = new Vector2(7 + Random.Range(-shake, shake), -126 + Random.Range(-shake, shake));

        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void startButton()
    {
        FindObjectOfType<AudioSource>().PlayOneShot(chooseSound);
        SceneManager.LoadScene(1);
    }

    int difficulty = 2;
    string[] dNames = new string[] { "Too Easy", "Easy", "Normal", "Hard", "Harder", "Hardest", "Hardester", "!?!?" };

    public void harder()
    {
        difficulty += 1;
        if (difficulty > 6)
        {
            difficulty = 7;
            dText.color = Color.red;
        }
        else dText.color = Color.white;
        dText.text = dNames[difficulty];
        GlobalVals._globalDifficulty = dNames[difficulty];
        camShake.Shake(0.1f, difficulty / 7f);
        bgLight.color = new Color(bgLight.color.r, bgLight.color.g, bgLight.color.b, difficulty / 70f);
        FindObjectOfType<AudioSource>().pitch = 0.5f + difficulty / 7f;
        FindObjectOfType<AudioSource>().PlayOneShot(chooseSound);
    }

    public void easier()
    {
        difficulty -= 1;
        if (difficulty < 1)
        {
            difficulty = 0;
            dText.color = Color.cyan;
        }
        else dText.color = Color.white;
        dText.text = dNames[difficulty];
        GlobalVals._globalDifficulty = dNames[difficulty];
        camShake.Shake(0.1f, difficulty / 7f);
        bgLight.color = new Color(bgLight.color.r, bgLight.color.g, bgLight.color.b, difficulty / 70f);
        FindObjectOfType<AudioSource>().pitch = 0.5f + difficulty / 7f;
        FindObjectOfType<AudioSource>().PlayOneShot(chooseSound);
    }
}
