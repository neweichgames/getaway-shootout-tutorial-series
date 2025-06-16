using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    public float finishTime = 1f;

    public event Action<Player, bool> OnPlayerEntered;
    public event Action OnPlayersFinished;

    HashSet<Player> finishedPlayers = new HashSet<Player>();

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        
        if (player == null)
            return;

        PlayerEntered(player);
    }

    void PlayerEntered(Player player)
    {
        if (finishedPlayers.Contains(player))
            return;

        bool firstPlayer = finishedPlayers.Count == 0;

        finishedPlayers.Add(player);

        player.Deactivate();

        OnPlayerEntered?.Invoke(player, firstPlayer);
        if(firstPlayer)
            StartCoroutine(PlayerEnteredLoop());
    }

    IEnumerator PlayerEnteredLoop()
    {
        yield return new WaitForSeconds(finishTime);
        PlayersFinished();
    }

    void PlayersFinished()
    {
        OnPlayersFinished?.Invoke();
    }
}
