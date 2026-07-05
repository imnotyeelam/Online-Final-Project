using UnityEngine;

public class yl_InventoryTest : MonoBehaviour
{
    [Header("Test Item")]
    public Sprite testItemIcon;

    void Update()
    {
        // 按 Y 模拟拾取物品
        if (Input.GetKeyDown(KeyCode.Y))
        {
            yl_InventoryHUD.Instance.PickUpQuestItem(testItemIcon);

            Debug.Log("Test: Pick Up Item");
        }

        // 按 U 模拟交出物品
        if (Input.GetKeyDown(KeyCode.U))
        {
            yl_InventoryHUD.Instance.ClearQuestItem();

            Debug.Log("Test: Clear Item");
        }
    }
}