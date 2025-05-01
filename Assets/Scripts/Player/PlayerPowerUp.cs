using UnityEngine;

public class PlayerPowerUp : MonoBehaviour
{
    private Player ourPlayer;
    private bool isUsing;
    private Usable usable;

    private void Start()
    {
        ourPlayer = GetComponent<Player>();
    }

    void Update()
    {
        if(usable != null && isUsing)
            usable.Use(ourPlayer);
    }

    public void Use()
    {
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
        GameObject usableObj = Instantiate(powerUp.powerUpObject, transform.GetChild(0).GetChild(1).GetChild(1));
        usable = usableObj.GetComponent<Usable>();

        if (usable == null)
        {
            Debug.LogError("Error: Power up item must have component of type Usable!");
            Destroy(usableObj);
            return;
        }

        usable.OnDepleted += OnPowerUpDepleted;
        GetComponent<TargetFinder>().ActivateFinder(powerUp.findTarget);
    }

    public bool HasPowerUp()
    {
        return usable != null;
    }

    void OnPowerUpDepleted()
    {
        usable.OnDepleted -= OnPowerUpDepleted;
        Destroy(usable.gameObject);
        ResetPowerUp();
    }

    void ResetPowerUp()
    {
        usable = null;
        GetComponent<TargetFinder>().ActivateFinder(false);
    }
}
