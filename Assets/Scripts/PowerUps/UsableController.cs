using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class UsableController : MonoBehaviour
{
    public event Action OnDepleted;

    public int maxUses = 5;
    public float coolDown = 0.1f;
    public bool isAutomatic;
    public float depletedDelay = 0.25f;

    private int curUses;
    private float curCoolDown;
    private bool hasCanceled = true;

    private Usable usable;

    private void Start()
    {
        usable = GetComponent<Usable>();
        curUses = maxUses;
    }

    void Update()
    {
        if(curCoolDown >= 0f)
            curCoolDown -= Time.deltaTime;

        if (Keyboard.current.spaceKey.isPressed)
            Use();

        if (Keyboard.current.spaceKey.wasReleasedThisFrame)
            CancelUse();
    }

    public void CancelUse()
    {
        hasCanceled = true;
    }

    public void Use()
    {
        if (curUses <= 0 || curCoolDown > 0 || (!isAutomatic && !hasCanceled))
            return;

        usable.Use();
        curUses--;
        curCoolDown = coolDown;
        hasCanceled = false;

        if (curUses <= 0)
            StartCoroutine(DepletedLoop());
    }

    IEnumerator DepletedLoop()
    {
        yield return new WaitForSeconds(depletedDelay);

        OnDepleted?.Invoke();
    }
}
