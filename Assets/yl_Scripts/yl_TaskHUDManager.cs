using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class yl_TaskHUDManager : MonoBehaviour
{
    public static yl_TaskHUDManager Instance;

    [Header("UI")]
    public GameObject taskPanel;
    public TMP_Text taskText;

    [Header("Settings")]
    public float removeCompletedAfter = 5f;

    [System.Serializable]
    public class TaskData
    {
        public string text;
        public bool isCompleted;
        public bool shouldHide;
    }

    private List<TaskData> taskList = new List<TaskData>();

    void Awake()
    {
        Instance = this;
        ClearTasks();
    }

    public void SetTasks(params string[] newTasks)
    {
        taskList.Clear();

        foreach (string task in newTasks)
        {
            taskList.Add(new TaskData { text = task, isCompleted = false, shouldHide = false });
        }

        taskPanel.SetActive(true);
        UpdateTaskDisplay();
    }

    public void CompleteTask(int index)
    {
        if (index < 0 || index >= taskList.Count) return;

        taskList[index].isCompleted = true;
        UpdateTaskDisplay();

        StartCoroutine(RemoveCompletedTaskAfterDelay(taskList[index]));
    }

    IEnumerator RemoveCompletedTaskAfterDelay(TaskData task)
    {
        yield return new WaitForSeconds(removeCompletedAfter);

        if (task == null) yield break;

        task.shouldHide = true;
        UpdateTaskDisplay();

        bool anyVisible = false;
        foreach (var t in taskList)
        {
            if (!t.shouldHide) anyVisible = true;
        }

        if (!anyVisible)
        {
            taskPanel.SetActive(false);
        }
    }

    public void UpdateTask(int index, string newText)
    {
        if (index < 0 || index >= taskList.Count) return;

        taskList[index].text = newText;
        UpdateTaskDisplay();
    }

    public void ClearTasks()
    {
        taskList.Clear();

        if (taskText != null)
            taskText.text = "";

        if (taskPanel != null)
            taskPanel.SetActive(false);
    }

    void UpdateTaskDisplay()
    {
        string display = "TASK\n";

        for (int i = 0; i < taskList.Count; i++)
        {
            if (taskList[i].shouldHide) continue;

            if (taskList[i].isCompleted)
                display += "<color=#808080><s>" + taskList[i].text + "</s></color>\n";
            else
                display += taskList[i].text + "\n";
        }

        taskText.text = display;
    }
}