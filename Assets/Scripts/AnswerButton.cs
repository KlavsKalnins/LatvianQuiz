using System.Collections;
using TMPro;
using UnityEngine;

public class AnswerButton : MonoBehaviour
{
    public static bool canPressButton = true;
    
    public int index;
    public TMP_Text buttonText;
    public Animator animator;

    public void SetPresentation(string text)
    {
        buttonText.text = text;
    }
    
    public void OnButtonPressed()
    {
        if (!canPressButton) return;
        canPressButton = false;
        animator.SetBool("Pressed", true);
        StartCoroutine(SetAnimBack());
        
        GameManager.Instance.ButtonAnswerPress(index);
    }
    
    IEnumerator SetAnimBack()
    {
        yield return new WaitForSeconds(2f);
        animator.SetBool("Pressed", false);
    }
}
