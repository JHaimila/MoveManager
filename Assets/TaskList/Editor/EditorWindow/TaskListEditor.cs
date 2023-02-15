using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;

namespace TaskList
{
    public class TaskListEditor : EditorWindow
    {
        VisualElement container;

        TextField taskText;
        Button addTaskBtn;
        ScrollView taskListScrollView;
        ObjectField savedTasksObjectField;
        Button loadTaskBtn;
        TaskListSO taskListSO;
        Button saveProgressBtn;
        ProgressBar taskProgressbar;
        ToolbarSearchField searchBox;
        Label noticeText;

        public const string path = "Assets/TaskList/Editor/EditorWindow/";

        [MenuItem("Tools/TaskList")]
        public static void ShowWindow()
        {
            TaskListEditor window = GetWindow<TaskListEditor>();
            window.titleContent = new GUIContent("Task List");
        }
        public void CreateGUI()
        {   
            container = rootVisualElement;
            
            // Adds a link to the xml file
            VisualTreeAsset original = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path+"TaskListEditor.uxml");
            container.Add(original.Instantiate());

            // Adds a link to the uss file
            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(path+"taskListEditor.uss");
            container.styleSheets.Add(styleSheet);

            // Connect variables to the elements in the xml
            taskText = container.Q<TextField>("taskText");
            taskListScrollView = container.Q<ScrollView>("taskList");
            savedTasksObjectField = container.Q<ObjectField>("savedTasks");
            savedTasksObjectField.objectType = typeof(TaskListSO);

            //Notice
            noticeText = container.Q<Label>("noticeText");
            UpdateNotification("");

            //Progress Bar
            taskProgressbar = container.Q<ProgressBar>("taskProgressbar");

            // Save Tasks Button
            saveProgressBtn = container.Q<Button>("saveProgressBtn");
            saveProgressBtn.clicked += SaveProgress;

            //Add task btn
            addTaskBtn = container.Q<Button>("addTaskBtn");
            addTaskBtn.clicked += AddTask;
            
            // Searchbox
            searchBox = container.Q<ToolbarSearchField>("searchBox");
            searchBox.RegisterValueChangedCallback(OnSearchTextChanged);

            //load Tasks btn
            loadTaskBtn = container.Q<Button>("loadTasksBtn");
            loadTaskBtn.clicked += LoadTasks;

            //Subscrble a key down event to Add task. Add task will the check if it was the enter key and add the task
            taskText.RegisterCallback<KeyDownEvent>(AddTask);

        }

        public void AddTask()
        {
            if(!string.IsNullOrEmpty(taskText.value))
            {
                taskListScrollView.Add(CreateTask(taskText.value));
                SaveTask(taskText.value);
                taskText.value = "";

                // returns focus back to the text field after a task has been added
                taskText.Focus();
                UpdateProgress();
                UpdateNotification("You have unsaved changes");
            }
        }

        public void AddTask(KeyDownEvent e)
        {
            if(Event.current.Equals(Event.KeyboardEvent("Return")))
            {
                AddTask();
            }
        }
        public void LoadTasks()
        {
            taskListSO = savedTasksObjectField.value as TaskListSO;

            if(taskListSO != null)
            {
                taskListScrollView.Clear();
                List<string> tasks = taskListSO.GetTasks();
                foreach(var task in tasks)
                {
                    taskListScrollView.Add(CreateTask(task));
                }
            }
            else
            {
                UpdateNotification("No task list selected");
            }
            UpdateProgress();
        }
        private TaskItem CreateTask(string taskText)
        {
            TaskItem taskItem = new TaskItem(taskText);
            taskItem.GetTaskLabel().text = taskText;
            taskItem.GetTaskToggle().RegisterValueChangedCallback(UpdateProgress);
            UpdateNotification("You have unsaved changes");
            return taskItem;
        }
        private void SaveTask(string task)
        {
            taskListSO.AddTask(task);
            EditorUtility.SetDirty(taskListSO);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            UpdateNotification("All changes have been saved!");
        }
        private void SaveProgress()
        {
            if(taskListSO != null)
            {
                List<string> tasks = new List<string>();

                foreach(TaskItem task in taskListScrollView.Children())
                {
                    if(!task.GetTaskToggle().value)
                    {
                        tasks.Add(task.GetTaskLabel().text);
                    }
                }
                taskListSO.AddTasks(tasks);
                EditorUtility.SetDirty(taskListSO);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                LoadTasks();
            }
        }
        private void UpdateProgress()
        {
            float completed = 0;
            float total = 0;
            foreach(TaskItem task in taskListScrollView.Children())
            {
                if(task.GetTaskToggle().value)
                {
                    completed++;
                }
                total++;
            }
            if(total > 0)
            {
                float progress = completed/total;
                taskProgressbar.value = progress;
                taskProgressbar.title = Mathf.Round(progress*100) +"%";
            }
            else
            {
                taskProgressbar.value = 1;
                taskProgressbar.title = "--%";
            }
        }
        private void UpdateProgress(ChangeEvent<bool> e)
        {
            UpdateNotification("You have unsaved chnages!");
            UpdateProgress();
        }
        private void OnSearchTextChanged(ChangeEvent<string> changeEvent)
        {
            string searchText = changeEvent.newValue.ToUpper();
            foreach(TaskItem task in taskListScrollView.Children())
            {
                string taskText = task.GetTaskLabel().text.ToUpper();
                if(!string.IsNullOrEmpty(searchText) && taskText.Contains(searchText))
                {
                    task.AddToClassList("highlight");
                }
                else
                {
                    task.RemoveFromClassList("highlight");
                }
            }
        }
        private void UpdateNotification(string text)
        {
            noticeText.text = text;
        }
    }
}


