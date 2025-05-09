using UnityEngine;

public class HealthIncrease : Usable
{
    public float amount = 100f;

    protected override bool UseObject(Player user)
    {
        user.GetComponent<Health>().IncreaseHealth(amount);
        Depleted();
        return true;
    }
}
