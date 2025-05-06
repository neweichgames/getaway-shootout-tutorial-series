using System.Collections;
using UnityEngine;

public class PlayerPowerUp : MonoBehaviour
{
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
        if(usable != null && isUsing)
            usable.Use(ourPlayer);
    }

    public void Use()
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
    }

    public bool HasPowerUp()
    {
        return usable != null;
    }

    void OnPowerUpDepleted()
    {
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
