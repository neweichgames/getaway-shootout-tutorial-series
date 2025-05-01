using System.Collections;
using UnityEngine;

public abstract class AmmoUsable : Usable
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

    private void Start()
    {
        curClipAmmo = Mathf.Min(startAmmo, ammoClipSize);
        curExtraAmmo = startAmmo - curClipAmmo;
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
        if (curClipAmmo <= 0 || curCoolDown > 0 || (!isAutomatic && !hasCanceled))
            return false;

        if (!UseObject(user))
            return false;

        curClipAmmo--;
        curCoolDown = useCoolDown;
        hasCanceled = false;


        if (curClipAmmo <= 0)
        {
            if (curExtraAmmo == 0)
                Depleted();
            else
                StartReload();
        }

        return true;
    }

    void StartReload()
    {
        StartCoroutine(ReloadLoop());
    }

    void Reload()
    {
        int reloadAmount = Mathf.Min(ammoClipSize - curClipAmmo, curExtraAmmo);
        curExtraAmmo -= reloadAmount;
        curClipAmmo += reloadAmount;
    }

    IEnumerator ReloadLoop()
    {
        yield return new WaitForSeconds(clipReloadTime);

        Reload();
    }
}
