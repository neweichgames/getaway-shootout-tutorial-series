using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;

    private float curHealth;
    private bool isDead;
    private bool isInvincible;

    public event Action OnDeath;

    void Start()
    {
        curHealth = maxHealth;
    }

    public void Damage(float damage)
    {
        if (isDead || isInvincible)
            return;

        curHealth -= damage;

        if (curHealth <= 0)
            Die();
    }

    public void Die()
    {
        if(isDead) 
            return;

        curHealth = 0f;
        isDead = true;

        OnDeath?.Invoke();
    }

    public void SetMaxHealth()
    {
        curHealth = maxHealth;
        isDead = false;
    }

    public void IncreaseHealth(float amount, bool capHealth = true)
    {
        curHealth += amount;
        
        if(capHealth)
            curHealth = Mathf.Min(curHealth, maxHealth);
    }

    public void SetInvincible(bool invincible)
    {
        isInvincible = invincible;
    }
}
