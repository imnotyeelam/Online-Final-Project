using UnityEngine;

public class yl_GuidanceExample : MonoBehaviour
{
    void Start()
    {
        yl_HUDGuidanceManager.Instance.SetGuidance(

@"[V] Hold to use Walkie Talkie

[Q] Switch Inventory

[TAB] View Tasks

[ESC] Pause");

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            yl_HUDGuidanceManager.Instance.ShowInteractionHint(

"Press [E] to Open Door");
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            yl_HUDGuidanceManager.Instance.HideInteractionHint();
        }
    }
}