using System.Collections;
using UnityEngine;

public class FinishDoorFX : MonoBehaviour
{
    public Transform door;
    private Finish finish;
    void Start()
    {
        finish = GetComponent<Finish>();
        finish.OnPlayerEntered += OnPlayerEntered;
    }

    void OnPlayerEntered(Player player, bool first)
    {
        if (!first)
            return;

        StartCoroutine(DoorLoop());
    }

    IEnumerator DoorLoop()
    {
        float t = 0f;
        Vector2 startPos = door.transform.localPosition;

        while (t < 1f)
        {
            door.transform.localPosition = Vector2.Lerp(startPos, Vector2.zero, t);

            t += Time.deltaTime / finish.finishTime;
            yield return null;
        }

        door.transform.localPosition = Vector2.zero;
    }
}
