using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public bool friendlyFire;
    protected Player owner;

    public void SetOwner(Player owner)
    {
        this.owner = owner;
    }

    public Player GetOwner()
    {
        return owner;
    }

    public bool IsObjectDamagable(GameObject obj)
    {
        return friendlyFire || owner == null || !owner.gameObject.Equals(obj);
    }

    protected void DamageHealth(Health health, float amount)
    {
        health.Damage(new Health.DamageInfo(amount, owner, this));
    }

    protected void DamageHealth(Health health, float amount, Vector2 hitPoint, Vector2 hitDirection)
    {
        health.Damage(new Health.DamageInfo(amount, owner, this, new Health.HitInfo(hitPoint, hitDirection)));
    }
}
