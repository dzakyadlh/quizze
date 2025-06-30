using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [Header("Quiz Scenes")]
    [SerializeField] GameObject MainMenuScene;
    [SerializeField] GameObject QuizScene;
    [SerializeField] GameObject QuizEndScene;

    [Header("Question")]
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] QuestionSO[] questions;

    [Header("Buttons")]
    [SerializeField] GameObject playButton;
    [SerializeField] GameObject[] answerButtons;
    // [SerializeField] GameObject nextButton;
    [SerializeField] GameObject backToMenuButton;
    [SerializeField] GameObject retryButton;

    [Header("Answer Sprites")]
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite incorrectAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;

    [Header("Score and Timer")]
    [SerializeField] GameObject scoreText;
    [SerializeField] Image timerImage;
    Timer timer;

    int currentQuestionIndex = 0;
    int correctAnswerCount = 0;
    bool isAnswerSelected = false;



    void Start()
    {
        timer = FindFirstObjectByType<Timer>();
        MainMenuScene.SetActive(true);
    }

    void Update()
    {
        timerImage.fillAmount = timer.fillFraction;
        if (timer.loadQuestionResult && !isAnswerSelected)
        {
            OnAnswerSelected(-1); // This will trigger the result display without selecting an answer
            timer.loadQuestionResult = false;

        }
        else if (timer.loadNextQuestion)
        {
            LoadNextQuestion();
            timer.loadNextQuestion = false;
        }
    }

    public void OnAnswerSelected(int index)
    {
        Debug.Log("Answer selected: " + index);
        if (index == -1) // This is for the case when the result is displayed without selecting an answer
        {
            questionText.text = "Time's up!";
            answerButtons[questions[currentQuestionIndex].GetCorrectAnswerIndex()].GetComponent<Image>().sprite = correctAnswerSprite;
            Debug.Log("Time's up! Correct answer displayed.");
        }
        if (index != -1)
        {
            isAnswerSelected = true;
            if (index == questions[currentQuestionIndex].GetCorrectAnswerIndex())
            {
                correctAnswerCount++;
                questionText.text = "Correct!";
                answerButtons[index].GetComponent<Image>().sprite = correctAnswerSprite;
                Debug.Log("Correct answer selected!");
            }
            else
            {
                questionText.text = "Wrong answer you donut!";
                answerButtons[index].GetComponent<Image>().sprite = incorrectAnswerSprite;
                answerButtons[questions[currentQuestionIndex].GetCorrectAnswerIndex()].GetComponent<Image>().sprite = correctAnswerSprite;
                Debug.Log("Incorrect answer selected.");
            }
            timer.CancelTimer();
        }
        SetButtonState(false);
        // nextButton.SetActive(true);
    }

    void DisplayQuestion()
    {
        questionText.text = questions[currentQuestionIndex].GetQuestion();
    }

    void DisplayAnswers()
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = questions[currentQuestionIndex].GetAnswer(i);
            }
            else
            {
                Debug.LogError("Button text component not found on button " + i);
            }
        }
    }

    void SetButtonState(bool state)
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            Button button = answerButtons[i].GetComponent<Button>();
            if (button != null)
            {
                button.interactable = state;
            }
            else
            {
                Debug.LogError("Button component not found on button " + i);
            }
        }
    }

    public void LoadNextQuestion()
    {
        currentQuestionIndex++;
        GetNextQuestion();
    }

    public void GetNextQuestion()
    {
        // nextButton.SetActive(false);
        isAnswerSelected = false;
        SetButtonState(true);
        SetDefaultButtonSprites();

        if (currentQuestionIndex < questions.Length)
        {
            DisplayQuestion();
            DisplayAnswers();
        }
        else
        {
            Debug.Log("No more questions available.");
            timer.EndTimer();
            timerImage.gameObject.SetActive(false);
            Debug.Log("Correct answers: " + correctAnswerCount);
            float score = (float)correctAnswerCount / questions.Length * 100f;
            Debug.Log("Score: " + score);
            scoreText.GetComponent<TextMeshProUGUI>().text = Mathf.Round(score).ToString();
            QuizScene.SetActive(false);
            QuizEndScene.SetActive(true);
        }
    }

    void SetDefaultButtonSprites()
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            Image buttonImage = answerButtons[i].GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.sprite = defaultAnswerSprite;
            }
            else
            {
                Debug.LogError("Image component not found on button " + i);
            }
        }
    }

    public void StartQuiz()
    {
        MainMenuScene.SetActive(false);
        QuizScene.SetActive(true);
        timerImage.gameObject.SetActive(true);
        timer.StartTimer();
        DisplayQuestion();
        DisplayAnswers();
    }

    public void BackToMenu()
    {
        QuizEndScene.SetActive(false);
        MainMenuScene.SetActive(true);
        currentQuestionIndex = 0;
        SetDefaultButtonSprites();
        questionText.text = "";
        SetButtonState(true);
    }

    public void RetryQuiz()
    {
        QuizEndScene.SetActive(false);
        QuizScene.SetActive(true);
        currentQuestionIndex = 0;
        SetDefaultButtonSprites();
        questionText.text = "";
        correctAnswerCount = 0;
        SetButtonState(true);
        DisplayQuestion();
        DisplayAnswers();
        timerImage.gameObject.SetActive(true);
        timer.StartTimer();
    }

}
