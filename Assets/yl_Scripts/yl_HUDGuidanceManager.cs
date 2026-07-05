using UnityEngine;
using TMPro;

public class yl_HUDGuidanceManager : MonoBehaviour
{
    public static yl_HUDGuidanceManager Instance;

    [Header("Text")]
    public TMP_Text guidanceText;
    public TMP_Text interactionHintText;

    private bool isGuidanceVisible = true;

    private void Awake()
    {
        Instance = this;

        HideInteractionHint();
        isGuidanceVisible = guidanceText.gameObject.activeSelf;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleGuidance();
        }
    }

    private void ToggleGuidance()
    {
        isGuidanceVisible = !isGuidanceVisible;

        guidanceText.gameObject.SetActive(isGuidanceVisible);
    }

    public void SetGuidance(string text)
    {
        guidanceText.text = text;
        if (!isGuidanceVisible)
        {
            isGuidanceVisible = true;
            guidanceText.gameObject.SetActive(true);
        }
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