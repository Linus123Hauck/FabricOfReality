using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {

    bool paused;
    SpriteRenderer sr;
    public MeshRenderer text;

	// Use this for initialization
	void Start () {
        paused = false;
        sr = GetComponent<SpriteRenderer>();
        text.sortingOrder = 102;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
            if (paused)
            {
                Time.timeScale = 0;
                sr.enabled = true;
                text.enabled = true;
                foreach (AudioSource aS in FindObjectsOfType<AudioSource>())
                {
                    aS.Pause();
                }
            }
            else
            {
                Time.timeScale = 1;
                sr.enabled = false;
                text.enabled = false;
                foreach (AudioSource aS in FindObjectsOfType<AudioSource>())
                {
                    aS.UnPause();
                }
            }
        }
	}
}
