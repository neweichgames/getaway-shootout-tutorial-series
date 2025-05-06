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
}
