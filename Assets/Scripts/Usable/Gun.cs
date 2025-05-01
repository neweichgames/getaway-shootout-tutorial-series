using System.Collections.Generic;
using UnityEngine;

public class Gun : AmmoUsable
{
    public GunLine line;

    public Transform shootSpot;
    public LayerMask ignoreMask;

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

    float HitObject(RaycastHit2D hit, float power)
    {
        Rigidbody2D rigid = hit.collider.GetComponent<Rigidbody2D>();
        if (rigid != null)
            rigid.AddForceAtPosition(shootSpot.up * rigidbodyForce * power, hit.point, ForceMode2D.Impulse);

        Health health = hit.collider.GetComponent<Health>();
        if (health != null)
            health.Damage(healthDamage * power);

        ObjectProperties props = hit.collider.GetComponent<ObjectProperties>();
        if (props != null)
            return props.transparency;
        
        return 0;
    }

    ShotData[] Shoot(Player ourPlayer)
    {
        RaycastHit2D[] rcs = Physics2D.RaycastAll(shootSpot.position, shootSpot.up, range, ~ignoreMask);
        List<ShotData> data = new List<ShotData>(rcs.Length + 1);

        float power = 1f;
        float prevDist = 0f;

        // Loop through all objects that our bullet raycast hit
        foreach(RaycastHit2D hit in rcs)
        {
            // Ignore our own player
            Player hitPlayer = hit.transform.GetComponent<Player>();
            if (hitPlayer != null && hitPlayer.Equals(ourPlayer))
                continue;

            // Calculate falloff power
            float newPower = power - (hit.distance - prevDist) * falloff / range;
            if(newPower <= 0f) 
                break;

            // Bullet has reached object with power ... add object to hit list
            data.Add(new ShotData(hit.distance, power, newPower));

            // Check if bullet can go through the object
            power = newPower * sharpness * HitObject(hit, power);
            if (newPower <= stoppingPower)
                return data.ToArray();

            prevDist = hit.distance;
        }

        float endDist = Mathf.Min(prevDist + power * range / falloff, range);
        data.Add(new ShotData(endDist, power, power - (endDist - prevDist) * falloff / range));
        return data.ToArray();
    }

    protected override bool UseObject(Player user)
    {
        ShotData[] data = Shoot(user);
        
        GunLine gl = Instantiate(line.gameObject).GetComponent<GunLine>();
        gl.CreateLine(data, shootSpot);

        return true;
    }
}
