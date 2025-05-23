using UnityEngine;

[RequireComponent (typeof(Bullet))]
public class BulletGun : Gun
{
    public Transform shootSpot;
    private Bullet bullet;

    protected override void Start()
    {
        base.Start();

        bullet = GetComponent<Bullet>();
    }

    protected override void Fire()
    {   
        bullet.Fire(shootSpot, user);
    }
}
