using UnityEngine;

/// <summary>
/// Class to create bullet effects. This should most likely change to how you want the bullets to look.
/// </summary>
[RequireComponent (typeof(Bullet))]
public class BulletFX : MonoBehaviour
{
    public GameObject bulletLine;
    public GameObject defaultHitParticles;

    void Start()
    {
        GetComponent<Bullet>().OnFire += OnFire;
    }

    void CreateParticles(Bullet.Data data)
    {
        //float startDist = 0f;
        Quaternion rotation = Quaternion.Euler(defaultHitParticles.transform.eulerAngles.x, defaultHitParticles.transform.eulerAngles.y, Mathf.Atan2(-data.direction.y, -data.direction.x) * Mathf.Rad2Deg);
        foreach (Bullet.LineData segment in data.lineData)
        {
            if(segment.hitTransform != null)
            {
                ObjectProperties props = segment.hitTransform.GetComponent<ObjectProperties>();
                if (props == null || props.hitParticles)
                {
                    GameObject p = Instantiate(defaultHitParticles, data.startPosition + data.direction * segment.endDist, rotation);
                    // Possibly in future use SetAmount instead of SetMultiplier and pass in data corresponding the amount of damage it would have applied
                    ParticleMultiplier multiplier = p.GetComponentInChildren<ParticleMultiplier>();
                    if (multiplier != null)
                        multiplier.SetMultiplier(segment.endPower);
                }
            }
        }
    }

    void OnFire(Bullet.Data data)
    {
        BulletLine gl = Instantiate(bulletLine).GetComponent<BulletLine>();
        gl.CreateLine(data);
        
        if(defaultHitParticles != null)
            CreateParticles(data);
    }
}
