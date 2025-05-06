using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    public float startTime = 5f;
    public bool playOnEnable = true;

    public UnityEvent OnFinish;

    private float curTime;
    private bool isPlaying;

    void OnEnable()
    {
        if (playOnEnable)
            PlayTimer();
    }

    void Update()
    {
        if (isPlaying)
        {
            curTime -= Time.deltaTime;
            
            if (curTime <= 0f)
                Finished();
        }
    }

    public void PauseTimer()
    {
        isPlaying = false;
    }

    public void ResumeTimer()
    {
        isPlaying = true;
    }

    public void PlayTimer()
    {
        curTime = startTime;
        ResumeTimer();
    }

    void Finished()
    {
        isPlaying = false;
        curTime = 0f;

        OnFinish?.Invoke();
    }
}
