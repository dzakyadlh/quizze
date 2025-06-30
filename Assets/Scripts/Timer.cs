using UnityEngine;

public class Timer : MonoBehaviour
{

    [SerializeField] float timeForQuestion = 5f;
    [SerializeField] float timeForResult = 3f;
    bool startTimer = false;
    public bool isQuestionTime;
    public bool loadQuestionResult;
    public bool loadNextQuestion;
    public float fillFraction;
    float timerValue;

    // Update is called once per frame
    void Update()
    {
        if (startTimer)
        {
            UpdateTimer();
        }
    }

    public void StartTimer()
    {
        timerValue = timeForQuestion;
        isQuestionTime = true;
        startTimer = true;
        loadQuestionResult = false;
        loadNextQuestion = false;
    }

    public void CancelTimer()
    {
        isQuestionTime = false;
        timerValue = timeForResult;
        loadQuestionResult = false;
    }

    public void EndTimer()
    {
        startTimer = false;
    }

    void UpdateTimer()
    {
        timerValue -= Time.deltaTime;
        if (isQuestionTime)
        {
            if (timerValue > 0)
            {
                fillFraction = timerValue / timeForQuestion;
            }
            else
            {
                isQuestionTime = false;
                timerValue = timeForResult;
                loadQuestionResult = true;
            }
        }
        else
        {
            if (timerValue > 0)
            {
                fillFraction = timerValue / timeForResult;
            }
            else
            {
                isQuestionTime = true;
                timerValue = timeForQuestion;
                loadNextQuestion = true;
            }
        }
    }
}
