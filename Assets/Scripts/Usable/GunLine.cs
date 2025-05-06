using UnityEngine;

public class GunLine : MonoBehaviour
{
    public LineRenderer lineObj;
    public float fadeTime = 0.25f;

    public void CreateLine(GunBullet.ShotData[] data, Vector2 startPos, Vector2 dir)
    {
        Vector2 prevPos = startPos;

        foreach (GunBullet.ShotData s in data)
        {
            Vector2 pos = startPos + dir * s.dist;
            LineRenderer l = Instantiate(lineObj, Vector2.zero, Quaternion.identity, transform).GetComponent<LineRenderer>();

            l.SetPosition(0, prevPos);
            l.SetPosition(1, pos);

            Gradient grad = new Gradient();
            grad.alphaKeys = new GradientAlphaKey[]
            {
                new GradientAlphaKey(s.startPower, 0f),
                new GradientAlphaKey(s.endPower, 1f)
            };
            
            l.colorGradient = grad;
            prevPos = pos;
        }
    }

    void Start()
    {
        Destroy(gameObject, fadeTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
