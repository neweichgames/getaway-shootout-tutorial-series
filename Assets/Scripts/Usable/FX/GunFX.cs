using UnityEngine;

/// <summary>
/// Class to create gun effects. This should most likely change to how you effects you want on your gun.
/// </summary>
[RequireComponent (typeof(Gun))]
public class GunFX : MonoBehaviour
{
    public GameObject shootGraphic;
    public GameObject reloadGraphic;

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

        if(shootTime >= 0f)
            shootTime -= Time.deltaTime;
        else
            shootGraphic.SetActive(false);
    }

    void OnShoot()
    {
        if(shootGraphic != null)
        {
            shootGraphic.SetActive(true);
            shootTime = 0.1f;
        }
            
        
        if(reloadGraphic != null && gun.GetCurAmmo() == 0)
            reloadGraphic.SetActive(false);
        
        if(shootSound != null)
            shootSound.Play();
    }

    void OnReload()
    {
        if(reloadSound != null)
            reloadSound.Play();
        
        if(reloadGraphic != null)
            reloadGraphic.SetActive(true);
    }
}
