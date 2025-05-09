using System.Collections.Generic;
using UnityEngine;

public class Bullet : Weapon
{
    public GunLine line;

    public LayerMask ignoreMask = 1 << 2;

    public float healthDamage = 50f;
    public float rigidbodyForce = 5f;

    public float range = 25f;
    [Range(0f, 1f)]
    public float falloff = 1f;

    [Range(0f, 1f)]
    public float sharpness = 0.5f;

    /// <summary>
    /// The minimum power necessary to go through an object.
    /// </summary>
    [Range(0f, 1f)]
    public float stoppingPower;

    public class ShotData
    {
        public float dist;
        public float startPower;
        public float endPower;

        public ShotData(float dist, float startPower, float endPower)
        {
            this.dist = dist;
            this.startPower = startPower;
            this.endPower = endPower;
        }
    }

    float HitObject(RaycastHit2D hit, Vector2 dir, float power)
    {
        Rigidbody2D rigid = hit.collider.GetComponent<Rigidbody2D>();
        if (rigid != null)
            rigid.AddForceAtPosition(dir * rigidbodyForce * power, hit.point, ForceMode2D.Impulse);

        Health health = hit.collider.GetComponent<Health>();
        if (health != null)
            DamageHealth(health, healthDamage, hit.point, dir);

        ObjectProperties props = hit.collider.GetComponent<ObjectProperties>();
        if (props != null)
            return props.transparency;
        
        return 0;
    }

    ShotData[] Shoot(Vector2 pos, Vector2 dir)
    {
        RaycastHit2D[] rcs = Physics2D.RaycastAll(pos, dir, range, ~ignoreMask);
        List<ShotData> data = new List<ShotData>(rcs.Length + 1);

        float power = 1f;
        float prevDist = 0f;

        // Loop through all objects that our bullet raycast hit
        foreach(RaycastHit2D hit in rcs)
        {
            // Ignore our own player
            if (!IsObjectDamagable(hit.collider.gameObject))
                continue;

            // Calculate falloff power
            float newPower = power - (hit.distance - prevDist) * falloff / range;
            if(newPower <= 0f) 
                break;

            // Bullet has reached object with power ... add object to hit list
            data.Add(new ShotData(hit.distance, power, newPower));

            // Check if bullet can go through the object
            power = newPower * sharpness * HitObject(hit, dir, newPower);
            if (newPower <= stoppingPower)
                return data.ToArray();

            prevDist = hit.distance;
        }

        float endDist = Mathf.Min(prevDist + power * range / falloff, range);
        data.Add(new ShotData(endDist, power, power - (endDist - prevDist) * falloff / range));
        return data.ToArray();
    }

    public void Fire()
    {
        Fire(transform.position, transform.up);
    }

    public void Fire(Transform shootSpot)
    {
        Fire(shootSpot.position, shootSpot.up);
    }

    public void Fire(Vector2 pos, Vector2 dir)
    {
        Vector2 d = dir.normalized;
        ShotData[] data = Shoot(pos, d);

        GunLine gl = Instantiate(line.gameObject).GetComponent<GunLine>();
        gl.CreateLine(data, pos, d);
    }
}
