using System.Collections;
using UnityEngine;

public class PowerUpBox : MonoBehaviour
{
    private static PowerUpItem[] items;

    private PowerUpItem curItem;

    void Awake()
    {
        if(items == null)
            items = Resources.LoadAll<PowerUpItem>("PowerUps");
    }


    private void Start()
    {
        SetRandomItem();
    }

    public PowerUpItem GetPowerUpItem()
    {
        GetComponent<Collider2D>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
        StartCoroutine(RespawnLoop());

        return curItem;
    }

    void SetRandomItem()
    {
        curItem = items[Random.Range(0, items.Length)];
        transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = curItem.icon;
    }

    IEnumerator RespawnLoop()
    {
        yield return new WaitForSeconds(5f);

        SetRandomItem();
        GetComponent<Collider2D>().enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
