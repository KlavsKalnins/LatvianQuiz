using UnityEngine;

[System.Serializable]
public class Question
{
    public string fact;
    public int correctAnswer;
    public string a;
    public string b;
    public string c;
    public string d;
    public Sprite icon;

    public Question(string fact, int correctAnswer, string a, string b, string c, string d, Sprite icon)
    {
        this.fact = fact;
        this.correctAnswer = correctAnswer;
        this.a = a;
        this.b = b;
        this.c = c;
        this.d = d;
        this.icon = icon;
    }

}
