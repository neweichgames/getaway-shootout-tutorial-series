using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    private Health health;

    public void Init(Health health)
    {
        this.health = health;
        this.health.OnHealthChanged += OnHealthChanged;
    }

    private void OnDestroy()
    {
        this.health.OnHealthChanged -= OnHealthChanged;
    }

    void OnHealthChanged(float curHealth)
    {
        float opacity = Mathf.Clamp01(1 - curHealth / health.maxHealth);

        Color c = GetComponent<Image>().color;
        GetComponent<Image>().color = new Color(c.r, c.g, c.b, opacity);
    }

    //private void Update()
    //{
    //    // Do heart beat animation here
    //}
}
