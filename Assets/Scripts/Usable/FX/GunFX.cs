using UnityEngine;

[RequireComponent (typeof(Gun))]
public class GunFX : MonoBehaviour
{
    public GameObject shootGraphic;
    public GameObject reloadToggleObject;

    public AudioSource shootSound;
    public AudioSource reloadSound;

    private Gun gun;
    private float shootTime = 0f;

    void Start()
    {
        gun = GetComponent<Gun> ();
        
        gun.OnUse += OnShoot;
        gun.OnReload += OnReload;
    }

    void Update()
    {
        UpdateShootGraphic();
    }

    void UpdateShootGraphic()
    {
        if (!shootGraphic.activeSelf)
            return;

        while (shootTime >= 0f)
            shootTime -= Time.deltaTime;

        if (shootTime <= 0f)
            shootGraphic.SetActive(false);
    }

    void OnShoot()
    {
        shootGraphic?.SetActive (true);
        if(gun.GetCurAmmo() == 0)
            reloadToggleObject?.SetActive(false);
        
        shootSound?.Play();
    }

    void OnReload()
    {
        reloadSound?.Play();
        reloadToggleObject?.SetActive(true);
    }
}
