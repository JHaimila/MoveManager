using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TaskList
{
    [CreateAssetMenu(menuName = "TaskList", fileName = "new Task List")]
    public class TaskListSO : ScriptableObject
    {
        [SerializeField] private List<string> tasks;

        public List<string> GetTasks(){return tasks;}
        public void AddTasks(List<string> newTasks)
        {
            tasks.Clear();
            tasks = newTasks;
        }
        public void AddTask(string task)
        {
            tasks.Add(task);
        }
    }
}

