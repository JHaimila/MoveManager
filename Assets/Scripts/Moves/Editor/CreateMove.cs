using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class CreateMove : EditorWindow
{
    VisualElement container;

    TextField moveNameField;
    Button moveNameBtn;

    VisualElement iconImg;
    ObjectField iconImgField;
    Button iconImgBtn;

    ScrollView winsToScrollView;
    ObjectField winsToObjectField;
    Button winsToBtn;

    Button createBtn;
    Label errorText;

    private string newName = "";
    private Sprite newIcon = null;
    private List<MoveClass> newWinsToList = new List<MoveClass>();

    public const string path = "Assets/Scripts/Moves/Editor/";
    public const string SOPath = "Assets/Resources/Moves/";
    public static void ShowWindow()
    {
        CreateMove window = GetWindow<CreateMove>();
        window.titleContent = new GUIContent("Create Move");
    }
    public void CreateGUI()
    {
        container = rootVisualElement;
        // Adds a link to the xml file
        VisualTreeAsset original = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path+"CreateMove.uxml");
        container.Add(original.Instantiate());

        // Adds a link to the uss file
        // StyleSheet moveManagerStyleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(path+"MoveManager.uss");
        container.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(path+"MoveManager.uss"));
        container.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(path+"CreateMove.uss"));

        moveNameField = container.Q<TextField>("moveNameField");
        moveNameBtn = container.Q<Button>("moveNameBtn");
        moveNameBtn.clicked += ChangeMoveName;
        
        iconImg = container.Q<VisualElement>("iconImg");
        iconImgField = container.Q<ObjectField>("iconImgField");
        iconImgField.objectType = typeof(Sprite);
        iconImgBtn = container.Q<Button>("iconImgBtn");
        iconImgBtn.clicked += ChangeMoveIcon;

        winsToScrollView = container.Q<ScrollView>("winsToScrollView");
        winsToObjectField = container.Q<ObjectField>("winsToObjectField");
        winsToObjectField.objectType = typeof(MoveClass);
        winsToBtn = container.Q<Button>("winsToBtn");
        winsToBtn.clicked += AddMoveWinsTo;

        errorText = container.Q<Label>("errorText");
        errorText.text = "";
        createBtn = container.Q<Button>("createBtn");
        createBtn.clicked += CreateNewMove;

    }

    private void ChangeMoveName()
    {
        if(!string.IsNullOrEmpty(moveNameField.value))
        {
            newName = moveNameField.value;
        }
    }
    private void ChangeMoveIcon()
    {
        if(iconImgField.value != null)
        {
            newIcon = iconImgField.value as Sprite;
            iconImg.style.backgroundImage = new StyleBackground(newIcon);
        }
    }
    private void AddMoveWinsTo()
    {
        MoveClass newWinsTo2 = winsToObjectField.value as MoveClass;
        Debug.Log(newWinsTo2);
        if(winsToObjectField.value != null)
        {
            newWinsToList.Add(winsToObjectField.value as MoveClass);
            RefreshScrollView();
        }
    }
    private void RemoveWinsTo(MoveClass move)
    {
        if(move != null)
        {
            if(newWinsToList.Contains(move))
            {
                newWinsToList.Remove(move);
                RefreshScrollView();
            }
        }
    }
    private void RefreshScrollView()
    {
        winsToScrollView.Clear();
        foreach(var move in newWinsToList)
        {
            WinsToItem newItem = new WinsToItem(move.moveName);
            newItem.GetButton().clicked += delegate {RemoveWinsTo(move);};
            winsToScrollView.Add(newItem);
        }
    }
    private void SetErrorText(string message)
    {
        errorText.text = message;
    }
    private void ClearErrorText()
    {
        errorText.text = "";
    }
    private void ClearScreen()
    {
        newName = "";
        newIcon = null;
        newWinsToList = new List<MoveClass>();

        moveNameField.value = "";
        iconImg.style.backgroundImage = new StyleBackground();
        iconImgField.value = null;
        winsToObjectField.value = null;
        winsToScrollView.Clear();
        ClearErrorText();
    }
    private void CreateNewMove()
    {
        if(!string.IsNullOrEmpty(newName) && newIcon != null)
        {
            MoveClass newMove = ScriptableObject.CreateInstance<MoveClass>();
            newMove.beats = newWinsToList;
            newMove.moveName = newName;
            newMove.img = newIcon;
            AssetDatabase.CreateAsset(newMove, SOPath + newName + ".asset");
            AssetDatabase.SaveAssets();
            ClearScreen();
        }
        else
        {
            if(string.IsNullOrEmpty(newName) && newIcon == null)
            {
                SetErrorText("No name or icon set for the move");
            }
            else if(string.IsNullOrEmpty(newName))
            {
                SetErrorText("No name set for the move");
            }
            else if(newIcon == null)
            {
                SetErrorText("No Icon set for the move");
            }
        }
    }
    public Button GetCreateButton()
    {
        return createBtn;
    }
    // if(!string.IsNullOrEmpty(newMoveNameField.value))
        // {
        //     MoveClass newMove = ScriptableObject.CreateInstance<MoveClass>();
        //     newMove.beats = new List<MoveClass>();
        //     newMove.moveName = newMoveNameField.value;
        //     AssetDatabase.CreateAsset(newMove, SOPath + newMoveNameField.value + ".asset");
        //     AssetDatabase.SaveAssets();
        //     newMoveNameField.value = "";
        //     RefreshMovesList();
        //     RefreshMoveView();
            
        // }
}
