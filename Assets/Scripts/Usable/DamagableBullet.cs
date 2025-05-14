using System;
using System.Collections.Generic;
using UnityEngine;

public class DamagableBullet : Weapon, Bullet
{
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

    public event Action<Bullet.Data> OnFire;

    void HitObject(RaycastHit2D hit, Vector2 dir, float power)
    {
        Rigidbody2D rigid = hit.collider.GetComponent<Rigidbody2D>();
        if (rigid != null)
            rigid.AddForceAtPosition(dir * rigidbodyForce * power, hit.point, ForceMode2D.Impulse);

        Health health = hit.collider.GetComponent<Health>();
        if (health != null)
            DamageHealth(health, healthDamage * power, hit.point, dir);
    }

    Bullet.LineData[] Shoot(Vector2 pos, Vector2 dir)
    {
        RaycastHit2D[] rcs = Physics2D.RaycastAll(pos, dir, range, ~ignoreMask);
        List<Bullet.LineData> data = new List<Bullet.LineData>(rcs.Length + 1);

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

            // Bullet has reached object with power ... hit object and add object to hit list
            HitObject(hit, dir, newPower);
            data.Add(new Bullet.LineData(hit.distance, power, newPower, hit.transform));

            // Check if bullet can go through the object
            ObjectProperties props = hit.collider.GetComponent<ObjectProperties>();
            power = newPower * sharpness * (props == null ? 0f : props.transparency);
            if (newPower <= stoppingPower)
                return data.ToArray();

            prevDist = hit.distance;
        }

        float endDist = Mathf.Min(prevDist + power * range / falloff, range);
        data.Add(new Bullet.LineData(endDist, power, power - (endDist - prevDist) * falloff / range, null));
        return data.ToArray();
    }

    public void Fire(Vector2 pos, Vector2 dir, Player owner)
    {
        SetOwner(owner);
        Vector2 d = dir.normalized;
        Bullet.Data data = new Bullet.Data(pos, dir, Shoot(pos, d));
        OnFire?.Invoke(data);
    }
}
