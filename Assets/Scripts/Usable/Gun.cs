using UnityEngine;

[RequireComponent (typeof(GunBullet))]
public class Gun : AmmoUsable
{
    public Transform shootSpot;
    private GunBullet gun;

    protected override void Start()
    {
        base.Start();

        gun = GetComponent<GunBullet>();
    }

    protected override bool UseObject(Player user)
    {
        gun.SetOwner(user);
        gun.Fire(shootSpot);
        return true;
    }
}
