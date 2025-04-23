using UnityEngine;

public class PlayerPowerUp : MonoBehaviour
{

    private bool isUsing;
    private UsableController controller;

    void Start()
    {
        controller = GetComponentInChildren<UsableController>();
    }

    
    void Update()
    {
        if(controller != null && isUsing)
            controller.Use();
    }

    public void Use()
    {
        isUsing = true;
    }

    public void CancelUse()
    {
        if(controller != null)
            controller.CancelUse();
        
        isUsing = false;
    }

    private void OnDisable()
    {
        isUsing = false;   
    }

    public void CreatePowerUp(PowerUpItem powerUp)
    {
        controller = Instantiate(powerUp.powerUpObject, transform.GetChild(0).GetChild(1).GetChild(1)).GetComponent<UsableController>();
        controller.OnDepleted += OnPowerUpDepleted;
    }

    public bool HasPowerUp()
    {
        return controller != null;
    }

    void OnPowerUpDepleted()
    {
        controller.OnDepleted -= OnPowerUpDepleted;
        Destroy(controller.gameObject);
        controller = null;
    }
}
