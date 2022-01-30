using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public float rotationOffset;

    public GameObject bullet;
    public Sprite sprite;

    public int bursts;
    public AnimationCurve bulletsPerBurst;
    public AnimationCurve burstDelay;

    public string _x;
    public string _y;
    public string _rotation;
    public string _distance;
    public string _spawnDelay;
    public string _lifeTime;
    public string _moveEase;
    public string _path;
    public string _size;
    public string _hue;
    public string _saturation;
    public string _alpha;
    public float _damage;
    private float brightness;

    BulletVar x;
    BulletVar y;
    BulletVar rotation;
    BulletVar distance;
    BulletVar spawnDelay;
    BulletVar lifeTime;
    BulletVar moveEase;
    BulletVar path;
    BulletVar size;
    BulletVar hue;
    BulletVar saturation;
    BulletVar alpha;

    private int cb;
    private float nextBurstAt;

    private GameObject player;
    private LevelUpSystem stats;

    private float bonusDMG;
    private float bonusRNG;
    private float bonusSZE;

    private void OnEnable()
    {
        stats = FindObjectOfType<LevelUpSystem>();
        x = new BulletVar(_x);
        y = new BulletVar(_y);
        rotation = new BulletVar(_rotation);
        distance = new BulletVar(_distance);
        spawnDelay = new BulletVar(_spawnDelay);
        lifeTime = new BulletVar(_lifeTime);
        moveEase = new BulletVar(_moveEase);
        path = new BulletVar(_path);
        size = new BulletVar(_size);
        hue = new BulletVar(_hue);
        saturation = new BulletVar(_saturation);
        alpha = new BulletVar(_alpha);

        cb = 0;
        nextBurstAt = Time.time;

        if(GameObject.FindGameObjectWithTag("Player"))
        {
            player = GameObject.FindGameObjectWithTag("Player").transform.GetChild(Mathf.RoundToInt(Random.value)).gameObject;
            brightness = player.GetComponent<SpriteRenderer>().color.r;
            if(brightness > .5)
            {
                bonusDMG = 1f;
                bonusRNG = 1.5f;
            }
            else
            {
                bonusDMG = 1.5f;
                bonusRNG = 1f;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!player) return;
        if (cb < bursts)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            while (Time.time > nextBurstAt & cb < bursts)
            {
                float sProg = Map(cb, 0, bursts - 1, 0, 1);
                int bullets = (int)bulletsPerBurst.Evaluate(sProg);

                for (int b = 0; b < bullets; b++)
                {
                    float bProg = Map(b, 0, bullets - 1, 0, 1);
                    Instantiate(bullet).GetComponent<Bullet>().Setup(
                        player.transform.position.x + x.GetVar(sProg, bProg),
                        player.transform.position.y + y.GetVar(sProg, bProg),
                        Mathf.Atan2(player.transform.position.y - mousePos.y, mousePos.x - player.transform.position.x) + rotation.GetVar(sProg, bProg) * Mathf.Deg2Rad,
                        bonusRNG * distance.GetVar(sProg, bProg),
                        spawnDelay.GetVar(sProg, bProg),
                        lifeTime.GetVar(sProg, bProg),
                        moveEase.GetVar(sProg, bProg),
                        path.GetCurve(sProg, bProg),
                        size.GetCurve(sProg, bProg),
                        hue.GetCurve(sProg, bProg),
                        saturation.GetCurve(sProg, bProg),
                        brightness,
                        alpha.GetCurve(sProg, bProg),
                        sprite,
                        bonusDMG * _damage * Map(stats.damageStat, 1, 100, 1, 20)
                        );
                }

                cb++;
                nextBurstAt += burstDelay.Evaluate(Map(cb, 0, bursts - 1, 0, 1));
            }
        }
        else Destroy(gameObject);
    }

    private class BulletVar
    {
        MList[] mLists;

        public BulletVar(string data)
        {
            //10
            //(10,0)
            //[(10,0),(5,0),(10,0)]
            //[[(10,0),(5,0),(10,0)],[(5,0),(10,0),(5,0)]]

            int layers = 0;
            int curveSize = -1;
            foreach (char c in data)
            {
                if (c == '[') layers++;
                else if (curveSize <= 0 && c == ',') curveSize--;
                else if (curveSize <= 0 && c == ')') curveSize *= -1;
                else if (c == ']') break;
            }
            mLists = new MList[Mathf.Abs(curveSize)];

            for (int mIndex = 0; mIndex < mLists.Length; mIndex++)
            {
                if (layers == 0) mLists[mIndex] = new MList(GetLayer0Val(data, mIndex));
                else if (layers == 1) mLists[mIndex] = new MList(getLayer1Vals(data, mIndex));
                else mLists[mIndex] = new MList(getLayer2MLists(data, mIndex));
            }
        }

        private float GetLayer0Val(string data, int mIndex)
        {
            int cIndex = 0;
            string num = "";
            foreach (char c in data)
            {
                if (c == '(') continue;
                if (cIndex > mIndex || c == ')') break;
                if (c == ',') cIndex++;
                else if (cIndex == mIndex) num += c;
            }
            return float.Parse(num);
        }

        private float[] getLayer1Vals(string data, int mIndex)
        {
            string[] layer0Data = data.Replace("[", "").Replace("]", "").Replace("),(", "$").Split('$');
            float[] vals = new float[layer0Data.Length];
            for (int i = 0; i < vals.Length; i++)
            {
                vals[i] = GetLayer0Val(layer0Data[i], mIndex);
            }
            return vals;
        }

        private MList[] getLayer2MLists(string data, int mIndex)
        {
            string[] layer1Data = data.Substring(1, data.Length - 2).Replace("],[", "$").Split('$');
            MList[] mLists = new MList[layer1Data.Length];
            for (int i = 0; i < mLists.Length; i++)
            {
                mLists[i] = new MList(getLayer1Vals(layer1Data[i], mIndex));
            }
            return mLists;
        }

        public float GetVar(float sProgress, float bProgress)
        {
            return mLists[0].Get(sProgress, bProgress);
        }

        public AnimationCurve GetCurve(float sProgress, float bProgress)
        {
            if (mLists.Length == 1)
            {
                return new AnimationCurve(new Keyframe[] { new Keyframe(0, mLists[0].Get(sProgress, bProgress)), new Keyframe(1, mLists[0].Get(sProgress, bProgress)) });
            }
            Keyframe[] points = new Keyframe[mLists.Length];
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = new Keyframe(Map(i, 0, points.Length - 1, 0, 1), mLists[i].Get(sProgress, bProgress));
            }
            return new AnimationCurve(points);
        }

        private class MList
        {
            float val;
            float[] lVals;
            MList[] mVals;

            public MList(float val)
            {
                this.val = val;
            }

            public MList(float[] vals)
            {
                lVals = vals;
            }

            public MList(MList[] vals)
            {
                mVals = vals;
            }

            public float Get(float a, float b)
            {
                if (mVals != null)
                {
                    a *= mVals.Length - 1;
                    if (a == Mathf.Round(a)) return mVals[(int)a].Get(b, 0);
                    return Map(a, Mathf.Floor(a), Mathf.Ceil(a), mVals[(int)Mathf.Floor(a)].Get(b, 0), mVals[(int)Mathf.Ceil(a)].Get(b, 0));
                }
                if (lVals != null)
                {
                    a *= lVals.Length - 1;
                    if (a == Mathf.Round(a)) return lVals[(int)a];
                    return Map(a, Mathf.Floor(a), Mathf.Ceil(a), lVals[(int)Mathf.Floor(a)], lVals[(int)Mathf.Ceil(a)]);
                }
                return val;
            }
        }
    }

    public static float Map(float a, float b, float c, float d, float e)
    {
        if (b == c || d == e) return d;
        return d + ((a - b) / (c - b)) * (e - d);
    }
}
