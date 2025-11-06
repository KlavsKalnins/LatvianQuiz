using UnityEngine;

[CreateAssetMenu]
public class OpenExternalLink : ScriptableObject
{
    public string url;

    public void OpenLink()
    {
        Application.OpenURL(url);
    }
}
