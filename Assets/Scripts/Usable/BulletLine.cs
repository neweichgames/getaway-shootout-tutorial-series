using UnityEngine;

public class BulletLine : MonoBehaviour
{
    public LineRenderer lineObj;
    public float fadeTime = 0.25f;

    public void CreateLine(Bullet.Data data)
    {
        Vector2 prevPos = data.startPosition;

        foreach (Bullet.LineData s in data.lineData)
        {
            Vector2 pos = data.startPosition + data.direction * s.endDist;
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
