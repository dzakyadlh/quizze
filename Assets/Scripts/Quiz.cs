using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading;

public class Quiz : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questionText;
    [SerializeField] QuestionSO question;
    [SerializeField] GameObject[] answerButtons;
    int correctAnswerIndex;
    [SerializeField] Sprite defaultAnswerSprite;
    [SerializeField] Sprite incorrectAnswerSprite;
    [SerializeField] Sprite correctAnswerSprite;


    void Start()
    {
        GetNextQuestion();
    }

    public void OnAnswerSelected(int index)
    {
        if (index < 0 || index >= answerButtons.Length)
        {
            Debug.LogError("Index out of bounds: " + index);
            return;
        }
        if (index == question.GetCorrectAnswerIndex())
        {
            questionText.text = "Correct!";
            answerButtons[index].GetComponent<Image>().sprite = correctAnswerSprite;
            Debug.Log("Correct answer selected!");
        }
        else
        {
            questionText.text = "Wrong answer you donut!";
            answerButtons[index].GetComponent<Image>().sprite = incorrectAnswerSprite;
            answerButtons[question.GetCorrectAnswerIndex()].GetComponent<Image>().sprite = correctAnswerSprite;
            Debug.Log("Incorrect answer selected.");
        }
        SetButtonState(false);
    }

    void DisplayQuestion()
    {
        questionText.text = question.GetQuestion();
    }

    void DisplayAnswers()
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = question.GetAnswer(i);
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

    void GetNextQuestion()
    {
        SetButtonState(true);
        SetDefaultButtonSprites();
        DisplayQuestion();
        DisplayAnswers();
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

}
