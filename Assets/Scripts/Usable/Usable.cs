using System;
using UnityEngine;

public abstract class Usable : MonoBehaviour
{
    public event Action OnDepleted;

    public virtual bool Use(Player user)
    {
        return UseObject(user);
    }

    public virtual void CancelUse() { }

    protected abstract bool UseObject(Player user);

    protected void Depleted()
    {
        OnDepleted?.Invoke();
    }
}
