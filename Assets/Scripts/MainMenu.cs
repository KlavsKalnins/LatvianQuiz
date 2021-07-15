using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Category[] categories;
    [SerializeField] GameObject categoryTemplate;
    Animator animator;
    public Action<bool> isGameRunning;
    [SerializeField] bool isGameOn;
    [SerializeField] GameObject categoryPanel;
    public int score;
    float itemSize = 3;

    void Start()
    {
        animator = GetComponent<Animator>();
        CreateAllCategorieUI();
    }

    private void CreateAllCategorieUI()
    {
        for (int i = 0; i < categories.Length; i++)
        {
            GameObject categoryInst = Instantiate(categoryTemplate, transform.position, transform.rotation);
            categoryInst.transform.SetParent(categoryPanel.transform);
            categoryInst.transform.localScale = new Vector3(itemSize, itemSize, itemSize);
            CategoriesUIInfo cUII = categoryInst.GetComponent<CategoriesUIInfo>();
            cUII.categoryThis = categories[i];
            string cTitle = cUII.categoryTitle.text;
        }

    }

    public void RecalculateUI()
    {
        Debug.Log("Sceneload");
        SceneManager.LoadScene(0);
    }

    public void AddToScore(int a)
    {
        score += a;
        if (PlayerPrefs.GetInt("score") < score)
            PlayerPrefs.SetInt("score",score);
    }

    public void OpenUrl(string urlLink)
    {
        Application.OpenURL(urlLink);
    }
    
}
