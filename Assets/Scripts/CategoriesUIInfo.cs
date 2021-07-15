using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CategoriesUIInfo : MonoBehaviour
{
    public GameObject gameManager;
    public Category categoryThis;
    public TMP_Text categoryTitle;
    public Image categoryIcon;
    public Image categoryBackground;
    public Image categoryProgressBar;
    public TMP_Text categoryPoints;
    public int points;
    [SerializeField] int pointsToUnlockCategory;
    public GameObject locked;
    public TMP_Text categoryInfoText;
    public static int allPointsTogether;

    void Start()
    {
        categoryTitle.text = categoryThis.GetCategoryName().ToString();
        categoryIcon.sprite = categoryThis.GetCategoryIcon();
        categoryBackground.sprite = categoryThis.GetCategoryBackground();
        string title = categoryTitle.text;
        pointsToUnlockCategory = categoryThis.GetPointsNeededToUnlock();
        points = PlayerPrefs.GetInt(title);
        categoryPoints.text = points.ToString() + "/5";
        // calculate progress bar from points
        // load saved data
        FindObjectOfType<MainMenu>().AddToScore(points);
        IsTheCategoryUnlocked();

    }

    private void IsTheCategoryUnlocked()
    {
        if (PlayerPrefs.GetInt("score") >= pointsToUnlockCategory)
        {
            locked.SetActive(false);
            categoryProgressBar.enabled = true;
            categoryPoints.enabled = true;
            categoryIcon.enabled = true;
            Debug.Log("mkm: " + points);
            categoryProgressBar.GetComponent<Image>().fillAmount = (float)points / 5;
        }
        else
        {
            GetComponent<Button>().enabled = false;
            locked.SetActive(true);
            categoryProgressBar.enabled = false;
            categoryPoints.enabled = false;
            categoryIcon.enabled = false;
            int pointsNeeded = pointsToUnlockCategory - PlayerPrefs.GetInt("score");
            categoryInfoText.text = "Vēl pietrūkst " + pointsNeeded + " pareizas atbildes";
        }
    }

    public void StartGame()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.questions = categoryThis.GetCategoryQuestions();
        gameManager.LoadHeader(categoryIcon.sprite,categoryTitle.text);
        gameManager.LoadGameplay();
    }
}
