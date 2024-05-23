using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class CategoriesUIInfo : MonoBehaviour
{
    [SerializeField] Button thisButton;
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

    void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += OnLocalizationChange;
    }

    void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocalizationChange;
    }

    void OnLocalizationChange(Locale newLocale = null)
    {
        Locale locale = LocalizationSettings.SelectedLocale;
        string localizedString = LocalizationSettings.StringDatabase.GetLocalizedString(categoryThis.categoryName, locale);
        categoryTitle.text = localizedString;
    }

    void Start()
    {
        OnLocalizationChange();
        categoryIcon.sprite = categoryThis.categoryIcon;
        categoryBackground.sprite = categoryThis.categoryBackground;
        string title = categoryTitle.text;
        pointsToUnlockCategory = categoryThis.pointsNeededToUnlock;
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
            categoryProgressBar.fillAmount = (float)points / 5;
        }
        else
        {
            thisButton.enabled = false;
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
        GameManager.Instance.questions = categoryThis.categoryQuestions;
        GameManager.Instance.LoadHeader(categoryIcon.sprite,categoryTitle.text);
        GameManager.Instance.LoadGameplay();
    }
}
