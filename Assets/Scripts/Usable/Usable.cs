using System;
using UnityEngine;

public abstract class Usable : MonoBehaviour
{
    public event Action OnDepleted;
    public event Action OnUse;

    protected Player user;

    private bool isDepleted;

    public bool Use(Player user)
    {
        if(isDepleted)
            return false;

        this.user = user;
        if (!Use())
            return false;

        OnUse?.Invoke();
        return true;
    }

    public virtual void CancelUse() { }

    protected abstract bool Use();

    protected void Deplete()
    {
        if (isDepleted)
            return;

        OnDepleted?.Invoke();
        isDepleted = true;
    }
}
