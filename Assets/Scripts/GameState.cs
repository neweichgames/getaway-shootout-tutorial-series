using System.Collections.Generic;

public class GameState
{
    private int[] playerScore;

    private List<int> finishedPlayers;

    private const int maxScore = 2;

    public GameState(int numPlayers)
    {
        playerScore = new int[numPlayers];
        finishedPlayers = new List<int>();
    }

    public void PlayerFinished(int playerID)
    {
        finishedPlayers.Add(playerID);
    }

    public void RoundOver()
    {
        foreach(int player in finishedPlayers)
        {
            if(finishedPlayers.Count == 1 || playerScore[player] < maxScore - 1)
                playerScore[player]++;
        }

        finishedPlayers.Clear();
    }

    public int GetWinner()
    {
        for(int i = 0; i < playerScore.Length; i++)
            if (playerScore[i] == maxScore)
                return i;
        
        return -1;
    }
}
