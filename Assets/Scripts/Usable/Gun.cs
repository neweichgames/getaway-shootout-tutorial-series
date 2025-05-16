using System;
using System.Collections;
using UnityEngine;

public abstract class Gun : Usable
{
    public int startAmmo = 5;
    public int ammoClipSize = 5;

    public float useCoolDown = 0.1f;
    public float clipReloadTime = 0f;

    public bool isAutomatic;

    private int curClipAmmo;
    private int curExtraAmmo;
    private float curCoolDown;
    private bool hasCanceled = true;

    public event Action OnReload;

    protected virtual void Start()
    {
        curClipAmmo = Mathf.Min(startAmmo, ammoClipSize);
        curExtraAmmo = startAmmo - curClipAmmo;
    }

    void Update()
    {
        if (curCoolDown >= 0f)
            curCoolDown -= Time.deltaTime;
    }

    protected abstract void Fire();

    public override void CancelUse()
    {
        hasCanceled = true;
    }

    protected override bool Use()
    {
        if (curClipAmmo <= 0 || curCoolDown > 0 || (!isAutomatic && !hasCanceled))
            return false;

        Fire();

        curClipAmmo--;
        curCoolDown = useCoolDown;
        hasCanceled = false;


        if (curClipAmmo <= 0)
        {
            if (curExtraAmmo == 0)
                Deplete();
            else
                StartReload();
        }

        return true;
    }

    void StartReload()
    {
        StartCoroutine(ReloadLoop());
    }

    protected virtual void Reload()
    {
        int reloadAmount = Mathf.Min(ammoClipSize - curClipAmmo, curExtraAmmo);
        curExtraAmmo -= reloadAmount;
        curClipAmmo += reloadAmount;

        OnReload?.Invoke();
    }

    IEnumerator ReloadLoop()
    {
        yield return new WaitForSeconds(clipReloadTime);

        Reload();
    }

    public int GetCurAmmo()
    {
        return curClipAmmo;
    }
}
