using UnityEngine;
using UnityEngine.Events;

public class Explosive : Weapon
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

    public bool destoryOnExplode;
    public UnityEvent OnExplode;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public void Explode()
    {
        // Check explosion for all health objects in game - could be optimized in the future if need be
        // Also, rigidbodies aren't checked currently since that would be a lot more objects to check, this could be
        // optimized in the future to make this check
        foreach (Health h in FindObjectsByType<Health>(FindObjectsSortMode.None))
        {
            if(!IsObjectDamagable(h.gameObject))
                continue;

            // Use position from Targetable class if object has the component ... otherwise just use center of object
            // Sort of abuses the Targetable class since this is not what it is for, so possibly refactor this in future
            if (h.GetComponent<Targetable>() != null)
                CheckExplosion(h.gameObject, h.GetComponent<Targetable>().GetTargetPosition());
            else
                CheckExplosion(h.gameObject, h.transform.position);
        }

        OnExplode?.Invoke();

        if(destoryOnExplode)
            Destroy(gameObject);
    }

    void HitTarget(RaycastHit2D hit, Vector2 dir, float power)
    {
        //Add explosion knockback
        if (hit.collider.GetComponent<Rigidbody2D>() != null)
            hit.collider.GetComponent<Rigidbody2D>().AddForceAtPosition(dir * rigidbodyForce * power, transform.position, ForceMode2D.Impulse);

        Health h = hit.collider.GetComponent<Health>();
        if (h != null)
            h.Damage(healthDamage * power);
    }

    void CheckExplosion(GameObject target, Vector2 targetPos)
    {
        Vector2 diff = targetPos - (Vector2)transform.position;
        
        float power = 1f;
        float prevDist = 0f;

        // Skim off all objects that we know are outside explosion distance
        if (diff.magnitude < range)
        {
            // Make raycast to object to see if we have a clear path to explode
            RaycastHit2D[] rcs = Physics2D.RaycastAll(transform.position, diff.normalized, range, ~ignoreMask);

            foreach (RaycastHit2D hit in rcs)
            {
                if (hit.collider.gameObject.Equals(gameObject))
                    continue;

                // Calculate falloff power
                float newPower = power - (hit.distance - prevDist) * falloff / range;
                if (newPower <= 0f)
                    break;

                if (hit.collider.gameObject.Equals(target))
                {
                    HitTarget(hit, diff.normalized, power);
                    return;
                }

                // Check if bullet can go through the object
                power = newPower * sharpness * (hit.collider.GetComponent<ObjectProperties>()?.transparency ?? 0f);
                if (newPower <= stoppingPower)
                    return;

                prevDist = hit.distance;
            }
        }
    }
}
