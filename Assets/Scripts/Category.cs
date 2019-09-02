using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Category Config")]
public class Category : ScriptableObject
{
    [SerializeField] string categoryName;
    [SerializeField] Sprite categoryBackground;
    [SerializeField] Sprite categoryIcon;
    [SerializeField] Sprite categoryProgressBar;
    [SerializeField] int pointsNeededToUnlock;
    [SerializeField] Question[] categoryQuestions;

    // questions

    public string GetCategoryName() { return categoryName; }
    public Sprite GetCategoryBackground() { return categoryBackground; }
    public Sprite GetCategoryIcon() { return categoryIcon; }
    public Sprite GetCategoryProgressBar() { return categoryProgressBar; }
    public int GetPointsNeededToUnlock() { return pointsNeededToUnlock; }
    public Question[] GetCategoryQuestions() { return categoryQuestions; }

}
