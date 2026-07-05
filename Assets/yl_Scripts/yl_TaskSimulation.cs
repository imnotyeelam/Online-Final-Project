using System.Collections;
using UnityEngine;

public class yl_TaskSimulation : MonoBehaviour
{
    IEnumerator Start()
    {
        // ????
        yl_TaskHUDManager.Instance.SetTasks(
            "Find the Fuse",
            "Restore Power (0/3)",
            "Collect Engine Parts (0/2)",
            "Escape the Planet"
        );

        yield return new WaitForSeconds(5);

        // ???????
        yl_TaskHUDManager.Instance.CompleteTask(0);

        yield return new WaitForSeconds(3);

        // ???????
        yl_TaskHUDManager.Instance.UpdateTask(1, "Restore Power (1/3)");

        yield return new WaitForSeconds(2);

        yl_TaskHUDManager.Instance.UpdateTask(1, "Restore Power (2/3)");

        yield return new WaitForSeconds(2);

        yl_TaskHUDManager.Instance.UpdateTask(1, "Restore Power (3/3)");
        yl_TaskHUDManager.Instance.CompleteTask(1);

        yield return new WaitForSeconds(3);

        yl_TaskHUDManager.Instance.UpdateTask(2, "Collect Engine Parts (1/2)");

        yield return new WaitForSeconds(2);

        yl_TaskHUDManager.Instance.UpdateTask(2, "Collect Engine Parts (2/2)");
        yl_TaskHUDManager.Instance.CompleteTask(2);

        yield return new WaitForSeconds(3);

        yl_TaskHUDManager.Instance.CompleteTask(3);
    }
}