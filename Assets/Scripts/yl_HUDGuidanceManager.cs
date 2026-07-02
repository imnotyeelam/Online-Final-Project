using UnityEngine;
using TMPro;

public class yl_HUDGuidanceManager : MonoBehaviour
{
    public static yl_HUDGuidanceManager Instance;

    [Header("Text")]
    public TMP_Text guidanceText;
    public TMP_Text interactionHintText;

    private void Awake()
    {
        Instance = this;

        HideInteractionHint();
    }

    public void SetGuidance(string text)
    {
        guidanceText.text = text;
    }

    public void AppendGuidance(string text)
    {
        guidanceText.text += "\n" + text;
    }

    public void ClearGuidance()
    {
        guidanceText.text = "";
    }

    public void ShowInteractionHint(string text)
    {
        interactionHintText.gameObject.SetActive(true);
        interactionHintText.text = text;
    }

    public void HideInteractionHint()
    {
        interactionHintText.gameObject.SetActive(false);
    }
}