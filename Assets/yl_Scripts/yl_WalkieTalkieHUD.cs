using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class yl_WalkieTalkieHUD : MonoBehaviour
{
    [Header("UI")]
    public Image walkieIcon;
    public TMP_Text statusText;

    [Header("Key")]
    public KeyCode talkKey = KeyCode.V;

    [Header("Colors")]
    public Color inactiveColor = new Color(0.4f, 0.4f, 0.4f, 0.5f);
    public Color activeColor = new Color(0f, 1f, 1f, 1f);

    void Start()
    {
        SetWalkieActive(false);
    }

    void Update()
    {
        bool isTalking = Input.GetKey(talkKey);
        SetWalkieActive(isTalking);
    }

    void SetWalkieActive(bool active)
    {
        if (walkieIcon != null)
            walkieIcon.color = active ? activeColor : inactiveColor;

        if (statusText != null)
        {
            statusText.text = active ? "ACTIVE" : "OFFLINE";
            statusText.color = active ? activeColor : inactiveColor;
        }
    }
}