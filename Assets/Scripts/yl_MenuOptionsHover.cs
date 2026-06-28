using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class yl_MenuOptionsHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject arrow;
    public TMP_Text labelText;

    public Color normalColor = Color.white;
    public Color hoverColor = Color.cyan;

    void Start()
    {
        arrow.SetActive(false);
        labelText.color = normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        arrow.SetActive(true);
        labelText.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        arrow.SetActive(false);
        labelText.color = normalColor;
    }
}