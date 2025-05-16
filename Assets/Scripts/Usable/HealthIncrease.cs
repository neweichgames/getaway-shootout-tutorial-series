using UnityEngine;

public class HealthIncrease : Usable
{
    public float amount = 100f;

    protected override bool Use()
    {
        user.GetComponent<Health>().IncreaseHealth(amount);
        Deplete();
        return true;
    }
}
