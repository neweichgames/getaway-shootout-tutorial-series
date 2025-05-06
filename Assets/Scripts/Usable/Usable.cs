using System;
using UnityEngine;

public abstract class Usable : MonoBehaviour
{
    public event Action OnDepleted;

    private bool isDepleted;

    public virtual bool Use(Player user)
    {
        if(isDepleted)
            return false;

        if (UseObject(user))
        {
            Depleted();
            return true;
        }

        return false;
    }

    public virtual void CancelUse() { }

    protected abstract bool UseObject(Player user);

    protected void Depleted()
    {
        OnDepleted?.Invoke();
        isDepleted = true;
    }
}
