using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    SpriteRenderer sprite;

    float timeSpawned; // double use as delay
    float moveEase;
    float despawnTime;
    float distance;
    float cosRot;
    float sinRot;
    Vector2 startPos;
    Vector2 prevPos;
    AnimationCurve path;
    AnimationCurve size;
    AnimationCurve hue;
    AnimationCurve saturation;
    float brightness;
    AnimationCurve alpha;

    public float damage;

    public void Setup(float x, float y, float rotation, float distance, float spawnDelay, float lifeTime, float moveEase,
        AnimationCurve path, AnimationCurve size, AnimationCurve hue, AnimationCurve saturation, float brightness, AnimationCurve alpha, Sprite sprite, float damage)
    {
        this.startPos = new Vector2(x, y);
        this.distance = distance;
        this.moveEase = moveEase;
        this.cosRot = Mathf.Cos(rotation);
        this.sinRot = Mathf.Sin(rotation);

        this.timeSpawned = Time.time + spawnDelay;
        this.despawnTime = timeSpawned + lifeTime;

        this.sprite = gameObject.GetComponent<SpriteRenderer>();
        this.sprite.sprite = sprite;

        this.path = path;
        this.size = size;
        this.hue = hue;
        this.saturation = saturation;
        this.brightness = brightness;
        this.alpha = alpha;
        this.damage = damage;
    }

    private void FixedUpdate()
    {
        if (path == null || timeSpawned > Time.time) return;
        if (despawnTime <= Time.time) Destroy(gameObject);
        else
        {
            float progress = Map(Time.time, timeSpawned, despawnTime, 0, 1);

            float mEase = Ease(progress, moveEase);
            float x = distance * mEase;
            float y = path.Evaluate(mEase);
            Vector2 pos = new Vector2(startPos.x + x * cosRot + y * sinRot, startPos.y + y * cosRot - x * sinRot);
            if (Vector2.Distance(pos, Vector2.zero) > 35) Destroy(gameObject);
            float rot = Mathf.Atan2(pos.y - prevPos.y, pos.x - prevPos.x);
            transform.SetPositionAndRotation(pos, Quaternion.EulerAngles(0, 0, rot));
            prevPos = pos;

            float size = this.size.Evaluate(progress);
            transform.localScale = new Vector3(size, size, 1);
            Color rgb = Color.HSVToRGB(hue.Evaluate(progress), saturation.Evaluate(progress), brightness);
            sprite.color = new Color(rgb.r, rgb.g, rgb.b, alpha.Evaluate(progress));
        }
    }

    public float Ease(float v, float ease) // used to multiply with values, ease between 0 and 1. Ease 0 = linear
    {
        if (ease == 0) return v;
        if (ease > 0) return Mathf.Pow(v, ease + 1);
        return Mathf.Pow(v, 1f / -(ease - 1));
    }

    public static float Map(float a, float b, float c, float d, float e)
    {
        if (b == c || d == e) return d;
        return d + ((a - b) / (c - b)) * (e - d);
    }
}
