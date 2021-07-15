using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public Question[] questions;
    [SerializeField] bool isGameRunning;
    Animator animator;
    [SerializeField] GameObject MainMenu;
    public float timeRemaining;

    // Header UI
    [SerializeField] Image headerIcon;
    [SerializeField] TMP_Text headerTitle;
    [SerializeField] TMP_Text headerQuestionCount;
    // UI
    public TMP_Text fact;
    public int correctAnswer;
    public TMP_Text a;
    public TMP_Text b;
    public TMP_Text c;
    public TMP_Text d;
    public Image timeRemainingImage;
    public Image icon;

    [SerializeField] Question currentQuestion;
    public int randomQuestion;
    public List<Question> unansweredQuestions;
    [SerializeField] int questionsAnswered;
    [SerializeField] int GameplayQuestionAnswerCount;

    [SerializeField] GameObject correctPanel;
    [SerializeField] GameObject wrongPanel;
    bool pressedButton = false;
    int questionsGotRight;

    private void Awake()
    {
        int gameStatusCount = FindObjectsOfType<GameManager>().Length;
        if (gameStatusCount > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
        timeRemainingImage.GetComponent<Image>().fillAmount = 1;
    }
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        GameplayLogic();
        KeyboardSwitchBetweenGameStates();
    }

    private void GameplayLogic()
    {
        if (isGameRunning)
        {
            ChangeGameState();
            GameIsRunning();
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
        timeRemainingImage.GetComponent<Image>().fillAmount = timeRemaining / 20;
        if (timeRemaining < 0)
        {
            NextQuestion();
        }
    }

    public void NextQuestion()
    {
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

    private void KeyboardSwitchBetweenGameStates()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            isGameRunning = false;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            isGameRunning = true;
        }
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
    }
    private void UpdateUINewQuestion()
    {
        timeRemainingImage.GetComponent<Image>().fillAmount = 1;
        timeRemaining = 20;
        fact.text = currentQuestion.fact;
        Debug.Log("2");
        correctAnswer = currentQuestion.correctAnswer;
        icon.sprite = currentQuestion.icon;
        a.text = currentQuestion.a;
        b.text = currentQuestion.b;
        c.text = currentQuestion.c;
        d.text = currentQuestion.d;
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
        unansweredQuestions = questions.ToList<Question>();

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
            currentQuestion = unansweredQuestions[randomQuestion];

        }
    }
    public void ButtonAnswerPress(int button)
    {
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
        }
        else
        {
            wrongPanel.SetActive(true);
        }
    }
}
