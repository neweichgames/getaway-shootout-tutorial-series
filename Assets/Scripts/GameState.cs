using System;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    private int[] playerScore;
    public int[] playerOrder;
    public int curMap;

    private List<int> finishedPlayers;

    private const int maxScore = 3;

    public event Action<int[]> OnScoreChanged;

    public GameState(int numPlayers)
    {
        playerScore = new int[numPlayers];
        finishedPlayers = new List<int>();
        curMap = -1;

        playerOrder = new int[numPlayers];
        for (int i = 0; i < numPlayers; i++)
            playerOrder[i] = i;
    }

    public void PlayerFinished(int playerID)
    {
        finishedPlayers.Add(playerID);
    }

    public void RoundOver(int[] playerOrder)
    {
        foreach(int player in finishedPlayers)
        {
            if(finishedPlayers.Count == 1 || playerScore[player] < maxScore - 1)
                playerScore[player]++;
        }
        OnScoreChanged?.Invoke(playerScore);

        finishedPlayers.Clear();
        this.playerOrder = playerOrder;
    }

    public int GetWinner()
    {
        for(int i = 0; i < playerScore.Length; i++)
            if (playerScore[i] == maxScore)
                return i;
        
        return -1;
    }

    public int[] GetScores()
    {
        return playerScore;
    }
}
