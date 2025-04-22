using System.Collections.Generic;
using UnityEngine;

public class Gun : Usable
{
    public GunLine line;

    public Transform shootSpot;
    public LayerMask ignoreMask;

    public float healthDamage = 50f;
    public float rigidbodyForce = 5f;

    public float range = 25f;
    public float falloff = 1f;

    public float sharpness = 0.5f;

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
        Health health = hit.collider.GetComponent<Health>();
        if (health != null)
            health.Damage(healthDamage * power);

        Rigidbody2D rigid = hit.collider.GetComponent<Rigidbody2D>();
        if (rigid != null)
            rigid.AddForceAtPosition(shootSpot.up * rigidbodyForce * power, hit.point, ForceMode2D.Impulse);

        // More interactions here
        ObjectProperties props = hit.collider.GetComponent<ObjectProperties>();
        if (props != null)
            return props.transparency;
        
        return 0;
    }

    ShotData[] Shoot()
    {
        RaycastHit2D[] rcs = Physics2D.RaycastAll(shootSpot.position, shootSpot.up, range, ~ignoreMask);
        List<ShotData> data = new List<ShotData>(rcs.Length + 1);

        float power = 1f;
        float prevDist = 0f;

        foreach(RaycastHit2D hit in rcs)
        {
            float newPower = power - (hit.distance - prevDist) * falloff / range;
            if(newPower <= stoppingPower) 
                break;

            data.Add(new ShotData(hit.distance, power, newPower));

            power = newPower * sharpness * HitObject(hit, power);
            if (newPower <= stoppingPower)
                return data.ToArray();

            prevDist = hit.distance;
        }

        data.Add(new ShotData(prevDist + (power - stoppingPower) * range / falloff, power, stoppingPower));
        return data.ToArray();
    }

    public override void Use()
    {
        ShotData[] data = Shoot();
        
        GunLine gl = Instantiate(line.gameObject).GetComponent<GunLine>();
        gl.CreateLine(data, shootSpot);
    }
}
