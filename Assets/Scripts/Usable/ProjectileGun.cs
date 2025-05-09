using UnityEngine;

public class ProjectileGun : Gun
{
    public GameObject projectile;
    public Transform shootSpot;

    public float projectileSpeed;
    public float projectileAngularSpeed;

    void Shoot(Player user)
    {
        GameObject proj = Instantiate(projectile, shootSpot.position, shootSpot.rotation);

        Vector2 baseVelocity = GetComponentInParent<Rigidbody2D>().linearVelocity;
        proj.GetComponent<Rigidbody2D>().linearVelocity = (Vector2)shootSpot.up * projectileSpeed + baseVelocity;
        proj.GetComponent<Rigidbody2D>().angularVelocity = projectileAngularSpeed;

        // Make sure to match if gun is flipped
        proj.transform.localScale = shootSpot.lossyScale;

        if (proj.GetComponent<Weapon>() != null)
            proj.GetComponent<Weapon>().SetOwner(user);
    }

    protected override bool UseObject(Player user)
    {
        Shoot(user);

        return true;
    }
}
