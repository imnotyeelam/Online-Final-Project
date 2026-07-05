using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class yl_InventoryHUD : MonoBehaviour
{
    public static yl_InventoryHUD Instance;

    public enum HeldSlot
    {
        Flashlight,
        Item
    }

    public Image flashlightBackground;
    public Image flashlightIcon;
    public Image itemBackground;
    public Image itemIcon;

    [Header("Slot Colors")]
    public Color activeColor = Color.white;
    public Color inactiveColor = new Color(1f, 1f, 1f, 0.3f);

    [Header("Controls")]
    public KeyCode switchKey = KeyCode.Q;

    [Header("State")]
    public HeldSlot currentSlot = HeldSlot.Flashlight;
    public bool hasQuestItem = false;
    public string currentItemName = "";

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ClearQuestItem();
    }

    void Update()
    {
        if (Input.GetKeyDown(switchKey))
        {
            TrySwitchSlot();
        }
    }

    public void TrySwitchSlot()
    {
        if (hasQuestItem)
        {
            SelectItem();
        }
        else
        {
            SelectFlashlight();
        }
    }

    public void PickUpQuestItem(Sprite itemSprite)
    {
        if (hasQuestItem)
        {
            Debug.Log("Already holding a quest item.");
            return;
        }

        hasQuestItem = true;

        itemIcon.sprite = itemSprite;
        itemIcon.enabled = true;

        SelectItem();
    }

    void UpdateSlotVisual()
    {
        flashlightIcon.color =
            currentSlot == HeldSlot.Flashlight ?
            activeColor :
            inactiveColor;

        flashlightBackground.color =
            currentSlot == HeldSlot.Flashlight ?
            activeColor :
            inactiveColor;

        itemIcon.color =
            currentSlot == HeldSlot.Item ?
            activeColor :
            inactiveColor;

        itemBackground.color =
            currentSlot == HeldSlot.Item ?
            activeColor :
            inactiveColor;
    }


    public void ClearQuestItem()
    {
        hasQuestItem = false;
        currentItemName = "";

        itemIcon.sprite = null;
        itemIcon.enabled = false;

        SelectFlashlight();
    }

    public void SelectFlashlight()
    {
        if (hasQuestItem)
        {
            Debug.Log("Cannot switch to flashlight while holding quest item.");
            return;
        }

        currentSlot = HeldSlot.Flashlight;

        UpdateSlotVisual();
    }

    public void SelectItem()
    {
        if (!hasQuestItem)
        {
            Debug.Log("No quest item in inventory.");
            return;
        }

        currentSlot = HeldSlot.Item;

        UpdateSlotVisual();
    }

    public bool IsHoldingFlashlight()
    {
        return currentSlot == HeldSlot.Flashlight;
    }

    public bool IsHoldingItem()
    {
        return currentSlot == HeldSlot.Item && hasQuestItem;
    }

    public string GetCurrentItemName()
    {
        return currentItemName;
    }
}