using UnityEngine;

public class yl_PickupItem : MonoBehaviour
{
    public Sprite itemIcon;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            yl_InventoryHUD.Instance.PickUpQuestItem(itemIcon);

            Destroy(gameObject);
        }
    }
}