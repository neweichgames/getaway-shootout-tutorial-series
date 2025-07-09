using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Class for controlling player power ups.
/// </summary>
public class PlayerPowerUp : MonoBehaviour
{
    public event Action<Usable> OnPowerUpCreate;
    public event Action OnPowerUpDeplete;

    private Player ourPlayer;
    private bool isUsing;
    
    private PowerUpItem powerUpItem;
    private Usable usable;
    private PlayerBody body;

    private void Start()
    {
        ourPlayer = GetComponent<Player>();
        body = GetComponent<PlayerBody>();
    }

    void Update()
    {
        if (isUsing && usable != null)
            usable.Use(ourPlayer);
    }

    public void OneTimeUse()
    {
        if (usable == null)
            return;

        usable.Use(ourPlayer);
        usable.CancelUse();
    }

    // Note: This method potentially delays Use by 1 frame ... Change function to call use the first time here
    // or change structure to remove polling inputs
    public void StartUse()
    {
        if(usable != null)
            isUsing = true;
    }

    public void CancelUse()
    {
        if(usable != null)
            usable.CancelUse();
        
        isUsing = false;
    }

    private void OnDisable()
    {
        isUsing = false;
        ResetPowerUp();
    }

    /// <summary>
    /// Create new power up object for player to hold from power up item.
    /// </summary>
    /// <param name="powerUp">Power up item for player to create power up object.</param>
    public void CreatePowerUp(PowerUpItem powerUp)
    {
        GameObject usableObj = Instantiate(powerUp.powerUpObject, body.handItemHolder);
        usable = usableObj.GetComponent<Usable>();

        if (usable == null)
        {
            Debug.LogError("Error: Power up item must have component of type Usable!");
            Destroy(usableObj);
            return;
        }

        usable.OnDepleted += OnPowerUpDepleted;
        GetComponent<TargetFinder>().ActivateFinder(powerUp.findTarget);
        
        powerUpItem = powerUp;
        OnPowerUpCreate?.Invoke(usable);
    }

    public bool HasPowerUp()
    {
        return usable != null;
    }

    public Usable GetPowerUp()
    {
        return usable;
    }

    void OnPowerUpDepleted()
    {
        OnPowerUpDeplete?.Invoke();
        if(gameObject.activeInHierarchy)
            StartCoroutine(ResetPowerUpLoop());
    }

    IEnumerator ResetPowerUpLoop()
    {
        yield return new WaitForSeconds(powerUpItem.destroyDelay);

        if (usable != null)
        {
            usable.OnDepleted -= OnPowerUpDepleted;
            Destroy(usable.gameObject);

            ResetPowerUp();
        }   
    }

    void ResetPowerUp()
    {
        usable = null;
        powerUpItem = null;
        isUsing = false;
        GetComponent<TargetFinder>().ActivateFinder(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (HasPowerUp() == false && collision.GetComponent<PowerUpBox>() != null)
            CreatePowerUp(collision.GetComponent<PowerUpBox>().GetPowerUpItem());
    }
}
