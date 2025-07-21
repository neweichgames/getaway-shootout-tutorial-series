using UnityEngine;
using UnityEngine.UI;

public class PowerUpUI : MonoBehaviour
{
    PlayerPowerUp playerPowerUp;
    Usable usable;

    void Start()
    {
        playerPowerUp.OnPowerUpCreate += OnCreate;
        playerPowerUp.OnPowerUpDeplete += OnDepletion;
    }

    public void Init(PlayerPowerUp playerPowerUp)
    {
        this.playerPowerUp = playerPowerUp;
    }

    void OnCreate(Usable usable)
    {
        this.usable = usable;
        usable.OnUse += OnUse;

        int num = 1;

        switch (usable)
        {
            case Gun gun:
                num = gun.startAmmo > gun.ammoClipSize ? Mathf.CeilToInt(gun.startAmmo / gun.ammoClipSize) : gun.startAmmo;
                break;
            // Add more usable types here
        }

        PowerUpItem item = playerPowerUp.GetPowerUpItem();
        InitSprites(item.ammoIcon, num);
        gameObject.SetActive(true);
    }

    void OnUse()
    {
        int num = 0;

        switch (usable)
        {
            case Gun gun:
                num = gun.startAmmo > gun.ammoClipSize ? gun.GetCurClip() : gun.GetCurAmmo();
                break;
                // Add more usable types here
        }

        UpdateSprites(num);
    }

    void OnDepletion()
    {
        usable.OnUse -= OnUse;
        gameObject.SetActive(false);
    }

    void InitSprites(Sprite spr, int num)
    {
        foreach(Transform child in transform)
            child.GetComponent<Image>().sprite = spr;

        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i < num);
            transform.GetChild(i).GetComponent<Image>().color = Color.white;
        }
            
    }

    void UpdateSprites(int num)
    {
        for (int i = num; i < transform.childCount; i++)
            transform.GetChild(i).GetComponent<Image>().color = Color.black;
    }


}
