using TMPro;
using UnityEngine;

public class ScoreboardUI : MonoBehaviour
{
    public void Init(int[] scores)
    {
        GameObject scoreObj = transform.GetChild(0).gameObject;
        for (int i = 1; i < scores.Length; i++)
            Instantiate(scoreObj, transform);

        UpdateScores(scores);
    }

    public void UpdateScores(int[] scores)
    {
        for (int i = 0; i < scores.Length; i++)
            transform.GetChild(i).GetComponent<TMP_Text>().text = scores[i].ToString();
    }
}
