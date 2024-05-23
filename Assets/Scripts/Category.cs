using UnityEngine;
[CreateAssetMenu(menuName ="Category Config")]

public class Category : ScriptableObject
{
    public string categoryName;
    public Sprite categoryBackground;
    public Sprite categoryIcon;
    public int pointsNeededToUnlock;
    public Question[] categoryQuestions;
}