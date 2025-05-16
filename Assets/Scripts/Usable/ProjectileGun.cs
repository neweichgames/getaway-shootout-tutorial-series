using UnityEngine;

public class ProjectileGun : Gun
{
    public Transform shootSpot;
    public GameObject projectile;

    public float projectileSpeed;
    public float projectileAngularSpeed;
    public bool inheritVelocity;

    protected override void Fire()
    {
        GameObject proj = Instantiate(projectile, shootSpot.position, shootSpot.rotation);

        proj.GetComponent<Rigidbody2D>().linearVelocity = (Vector2)shootSpot.up * projectileSpeed;
        if (inheritVelocity)
            proj.GetComponent<Rigidbody2D>().linearVelocity += GetComponentInParent<Rigidbody2D>().linearVelocity;
        proj.GetComponent<Rigidbody2D>().angularVelocity = projectileAngularSpeed;

        // Make sure to match if gun is flipped
        proj.transform.localScale = shootSpot.lossyScale;

        if (proj.GetComponent<Weapon>() != null)
            proj.GetComponent<Weapon>().SetOwner(user);
    }
}
