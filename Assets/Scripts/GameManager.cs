using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] GameObject viewGameObject;
    public Question[] questions;
    [SerializeField] bool isGameRunning;
    [SerializeField] Animator animator;
    [SerializeField] GameObject MainMenu;
    public float timeRemaining;

    // Header UI
    [SerializeField] Image headerIcon;
    [SerializeField] TMP_Text headerTitle;
    [SerializeField] TMP_Text headerQuestionCount;
    // UI
    public TMP_Text fact;
    public int correctAnswer;
    
    [SerializeField] private AnswerButton[] answerButtons;
    public Image timeRemainingImage;
    public Image icon;

    [SerializeField] Question currentQuestion;
    public int randomQuestion;
    public List<Question> unansweredQuestions;
    [SerializeField] int questionsAnswered;
    [SerializeField] int GameplayQuestionAnswerCount;

    [SerializeField] GameObject correctPanel;
    [SerializeField] GameObject wrongPanel;
    [SerializeField] bool pressedButton = false;
    int questionsGotRight;

    private void Awake()
    {
        Instance = this;
        timeRemainingImage.fillAmount = 1;
    }

    private void Update()
    {
        GameplayLogic();
    }

    public void ToggleGame(bool value)
    {
        isGameRunning = value;
        GameplayLogic();
    }

    private void GameplayLogic()
    {
        if (isGameRunning)
        {
            ChangeGameState();
            GameIsRunning();
            viewGameObject.SetActive(true);
        }
        else
        {
            isGameRunning = false;
            ChangeGameState();
        }
    }

    private void GameIsRunning()
    {
        if (!pressedButton)
            timeRemaining -= Time.deltaTime;
        timeRemainingImage.fillAmount = timeRemaining / 20;
        if (timeRemaining < 0)
        {
            NextQuestion();
        }
    }

    public void NextQuestion()
    {
        AnswerButton.canPressButton = true;
        correctPanel.SetActive(false);
        wrongPanel.SetActive(false);
        GameplayQuestionAnswerCount++;
        headerQuestionCount.text = GameplayQuestionAnswerCount + 1 + "/5";

        questionsAnswered++;
        if (questionsAnswered >= 5)
        {
            string title = headerTitle.text;
            if (PlayerPrefs.GetInt(title) < questionsGotRight)
                PlayerPrefs.SetInt(headerTitle.text, questionsGotRight);
            MainMenu.GetComponent<MainMenu>().RecalculateUI();
        }
        unansweredQuestions.Remove(currentQuestion);
        PickARandomQuestionFromList();
        UpdateUINewQuestion();
    }

    private void ChangeGameState()
    {
        animator.SetBool("gameIsRunning", isGameRunning);
        MainMenu.SetActive(!isGameRunning);
    }

    public void GoToMainMenu()
    {
        isGameRunning = false;
        questionsAnswered = 0;
        GameplayQuestionAnswerCount = 0;
        headerQuestionCount.text = GameplayQuestionAnswerCount + 1 + "/5";
        unansweredQuestions.Clear();
        animator.SetBool("gameIsRunning", isGameRunning);
        MainMenu.SetActive(!isGameRunning);
    }
    public void StartGame()
    {
        UpdateUINewQuestion();
        isGameRunning = true;
        viewGameObject.SetActive(true);

        ChangeGameState();
    }

    private void UpdateUINewQuestion()
    {
        timeRemainingImage.fillAmount = 1;
        timeRemaining = 20;
        fact.text = currentQuestion.fact;
        correctAnswer = currentQuestion.correctAnswer;
        icon.sprite = currentQuestion.icon;
        
        answerButtons[0].SetPresentation(currentQuestion.a);
        answerButtons[1].SetPresentation(currentQuestion.b);
        answerButtons[2].SetPresentation(currentQuestion.c);
        answerButtons[3].SetPresentation(currentQuestion.d);
        pressedButton = false;
    }

    public void LoadHeader(Sprite hIcon, string hTitle)
    {
        headerIcon.sprite = hIcon;
        headerTitle.text = hTitle;
        headerQuestionCount.text = GameplayQuestionAnswerCount + 1 + "/5";
    }

    public void LoadGameplay()
    {
        unansweredQuestions = questions.ToList();

        PickARandomQuestionFromList();
        StartGame();
    }
    private void PickARandomQuestionFromList()
    {
        if (unansweredQuestions == null || unansweredQuestions.Count == 0)
        {
            Debug.LogWarning("Not enough questions");
            GoToMainMenu();
        } else
        {
            randomQuestion = UnityEngine.Random.Range(0, unansweredQuestions.Count);
            // randomQuestion = 0;
            currentQuestion = unansweredQuestions[randomQuestion];
        }
    }

    public void ButtonAnswerPress(int button)
    {
        if (pressedButton) return;
        pressedButton = true;
        StartCoroutine(ConclusionToPressed(button));
    }

    IEnumerator ConclusionToPressed(int button)
    {
        yield return new WaitForSeconds(2f);
        if (correctAnswer == button)
        {
            questionsGotRight++;
            correctPanel.SetActive(true);
            ConfettiManager.Instance.PlayParticle();
        }
        else
        {
            wrongPanel.SetActive(true);
        }
    }
}
