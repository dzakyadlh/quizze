using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    [SerializeField] GameObject MainMenuScene;
    [SerializeField] GameObject QuizScene;
    [SerializeField] GameObject QuizEndScene;
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] QuestionSO[] questions;
    [SerializeField] GameObject playButton;
    [SerializeField] GameObject[] answerButtons;
    [SerializeField] GameObject nextButton;
    [SerializeField] GameObject backToMenuButton;
    [SerializeField] GameObject retryButton;
    [SerializeField] GameObject scoreText;
    int correctAnswerIndex;
    int currentQuestionIndex = 0;
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite incorrectAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;
    int correctAnswerCount = 0;


    void Start()
    {
        MainMenuScene.SetActive(true);
    }

    public void OnAnswerSelected(int index)
    {
        if (index < 0 || index >= answerButtons.Length)
        {
            Debug.LogError("Index out of bounds: " + index);
            return;
        }
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
        SetButtonState(false);
        nextButton.SetActive(true);
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
        nextButton.SetActive(false);
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
    }

}
