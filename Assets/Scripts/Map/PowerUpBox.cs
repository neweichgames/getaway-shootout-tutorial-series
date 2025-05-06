using System.Collections;
using UnityEngine;

public class PowerUpBox : MonoBehaviour
{
    public PowerUpItem overrideItem;

    private static PowerUpItem[] items;

    private PowerUpItem curItem;

    void Awake()
    {
        if(items == null)
            items = Resources.LoadAll<PowerUpItem>("PowerUps");
    }


    private void Start()
    {
        SetItem();
    }

    public PowerUpItem GetPowerUpItem()
    {
        GetComponent<Collider2D>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
        StartCoroutine(RespawnLoop());

        return curItem;
    }

    void SetItem()
    {
        if(overrideItem == null)
            curItem = items[Random.Range(0, items.Length)];
        else
            curItem = overrideItem;

        transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = curItem.icon;
    }

    IEnumerator RespawnLoop()
    {
        yield return new WaitForSeconds(5f);

        SetItem();
        GetComponent<Collider2D>().enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
