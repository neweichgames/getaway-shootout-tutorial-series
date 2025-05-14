using System;
using UnityEngine;

public abstract class Usable : MonoBehaviour
{
    public event Action OnDepleted;
    public event Action OnUse;

    private bool isDepleted;

    public virtual bool Use(Player user)
    {
        if(isDepleted)
            return false;

        if (UseObject(user))
        {
            OnUse?.Invoke();
            return true;
        }
        
        return false;
    }

    public virtual void CancelUse() { }

    protected abstract bool UseObject(Player user);

    protected void Deplete()
    {
        OnDepleted?.Invoke();
        isDepleted = true;
    }
}
