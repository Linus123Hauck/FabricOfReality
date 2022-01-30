using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour {

    float shakeUntil;
    float shakeIntensity;

	// Use this for initialization
	void OnEnable () {
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (shakeUntil >= Time.fixedTime)
        {
            transform.position = new Vector3(0, Random.Range(-shakeIntensity, shakeIntensity), -10);
        }
        else
        {
            transform.position = new Vector3(0, 0, -10);
        }
	}

    public void Shake(float time, float intensity)
    {
        if(shakeUntil < Time.fixedTime)
        {
            shakeUntil = Time.fixedTime + time;
            shakeIntensity = intensity;
        }
        else
        {
            shakeUntil = Mathf.Max(Time.fixedTime + time, shakeUntil);
            shakeIntensity += intensity;
            shakeIntensity = Mathf.Clamp(shakeIntensity, 0, 2);
        }
    }
}
