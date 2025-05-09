using System.Collections;
using UnityEngine;

public abstract class MultiUsable : Usable
{
    public int maxUses = 5;
    public float coolDown = 0.1f;
    public bool isAutomatic;

    private int curUses;
    private float curCoolDown;
    private bool hasCanceled = true;

    private void Start()
    {
        curUses = maxUses;
    }

    void Update()
    {
        if (curCoolDown >= 0f)
            curCoolDown -= Time.deltaTime;
    }

    public override void CancelUse()
    {
        hasCanceled = true;
    }

    public override bool Use(Player user)
    {
        if (curUses <= 0 || curCoolDown > 0 || (!isAutomatic && !hasCanceled))
            return false;

        if(!base.Use(user)) 
            return false;

        curUses--;
        curCoolDown = coolDown;
        hasCanceled = false;

        
        if (curUses <= 0)
            Depleted();

        return true;
    }
}
