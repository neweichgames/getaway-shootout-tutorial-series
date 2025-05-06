using UnityEngine;

public class ImpactWeapon : Weapon
{
    public float healthDamage = 50f;

    void HitObject(GameObject other)
    {
        Health h = other.GetComponent<Health>();
        if (h != null)
            DamageHealth(h, healthDamage);
    }

    void DisableWeapon(GameObject other)
    {
        GetComponent<Collider2D>().enabled = false;

        Rigidbody2D r = GetComponent<Rigidbody2D>();
        if (r != null)
            Destroy(r);

        // Set parent to items holder if a player ... otherwise set parent to collision object
        if (other.GetComponent<Player>() != null)
            transform.SetParent(other.transform.GetChild(0).GetChild(2));
        else
            transform.SetParent(other.transform);

        Destroy(gameObject, 10f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsObjectDamagable(collision.gameObject))
            return;

        // Sink impact into object slightly
        transform.position += (Vector3)collision.contacts[0].normal * (-0.1f);

        DisableWeapon(collision.gameObject);

        HitObject(collision.gameObject);
    }
}
