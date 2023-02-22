using System.Collections;
using System.Collections.Generic;
using Codice.Client.BaseCommands;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class MoveManager : EditorWindow
{
    VisualElement container;
    List<MoveClass> _moves = new List<MoveClass>();
    MoveClass _activeMove;
    List<string> changesMade = new List<string>();

    // Moves scroll elements
    ScrollView movesList;

    // Move View elements
    TextField moveName;
    Button changeNameBtn;
    VisualElement moveIcon;
    ObjectField changeIcon;
    Button changeIconBtn;
    ScrollView winsAgainstList;
    ObjectField addToWinAgainstField;
    Button addToWinAgainstBtn;

    Button deleteMoveBtn;
    Button newMoveBtn;
    Button refreshMoveListBtn;
    public const string path = "Assets/Scripts/Moves/Editor/";
    public const string SOPath = "Assets/Resources/Moves/";
    [MenuItem("Tools/Move Manager")]
    public static void ShowWindow()
    {
        MoveManager window = GetWindow<MoveManager>();
        window.titleContent = new GUIContent("Move Manager");
    }

    public void CreateGUI()
    {
        container = rootVisualElement;
        _moves = new List<MoveClass>();
        GetMoves();
            
        // Adds a link to the xml file
        VisualTreeAsset original = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path+"Movemanager.uxml");
        container.Add(original.Instantiate());

        // Adds a link to the uss file
        StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(path+"MoveManager.uss");
        container.styleSheets.Add(styleSheet);

        // Move list
        movesList = container.Q<ScrollView>("movesList");
        RefreshMovesList();

        // Move View
        moveName = container.Q<TextField>("moveName");
        // changeNameBtn = container.Q<Button>("changeNameBtn");
        // changeNameBtn.clicked += ChangeMoveName;
        moveIcon = container.Q<VisualElement>("moveIcon");
        changeIcon = container.Q<ObjectField>("changeIcon");
        changeIcon.objectType = typeof(Sprite);
        changeIconBtn = container.Q<Button>("changeIconBtn");
        changeIconBtn.clicked += ChangeMoveIcon;
        
        winsAgainstList = container.Q<ScrollView>("winsAgainstList");
        addToWinAgainstField = container.Q<ObjectField>("addToWinAgainstField");
        addToWinAgainstField.objectType = typeof(MoveClass);
        addToWinAgainstBtn = container.Q<Button>("addToWinAgainstBtn");
        addToWinAgainstBtn.clicked += AddWinAgainst;

        deleteMoveBtn = container.Q<Button>("deleteMoveBtn");
        deleteMoveBtn.clicked += DeleteMove;

        newMoveBtn = container.Q<Button>("newMoveBtn");
        newMoveBtn.clicked += CreateMoveOnClick;

        refreshMoveListBtn = container.Q<Button>("refreshMoveList");
        refreshMoveListBtn.clicked += RefreshMovesList;
        ClearMoveView();

        
    }

    public void GetMoves()
    {
        _moves.Clear();
        Object[] objects = Resources.LoadAll("Moves", typeof(MoveClass));
        foreach(var obj in objects)
        {
            _moves.Add((MoveClass)obj);
        }
    }
    public void RefreshMovesList()
    {
        movesList.Clear();
        GetMoves();
        foreach(MoveClass move in _moves)
        {
            Button newMove = new Button();
            newMove.text = move.moveName;
            newMove.clicked += delegate {OnMoveClicked(move);};
            movesList.Add(newMove);
        }

    }
    public void OnMoveClicked(MoveClass move)
    {
        if( move != null)
        {
            _activeMove = move;
            RefreshMoveView();
        }
    }
    public void RefreshMoveView()
    {
        if(_activeMove != null)
        {
            Debug.Log("UnRegisterEvents();");
            ClearMoveView();
            UnRegisterEvents();
            moveName.value = _activeMove.moveName;
            moveIcon.style.backgroundImage = new StyleBackground(_activeMove.img);
            changeIcon.value = _activeMove.img;
            addToWinAgainstField.value = null;
            if(_activeMove.beats.Count > 0)
            {
                foreach(MoveClass winMove in _activeMove.beats)
                {
                    WinsToItem newMove = new WinsToItem(winMove.moveName);
                    newMove.GetLabel().text = winMove.moveName;
                    newMove.GetButton().clicked += delegate{RemoveWinsTo(winMove, newMove.GetButton());};
                    winsAgainstList.Add(newMove);
                }
            }
            Debug.Log("RegisterEvents();");
            RegisterEvents();
        }
    }

    private void ClearMoveView()
    {
        moveName.value = "";
        winsAgainstList.Clear();
        moveIcon.style.backgroundImage = new StyleBackground();
        // changeIcon = null;
        // addToWinAgainstField = null;
    }
    public void RemoveWinsTo(MoveClass move, Button btn)
    {
        _activeMove.beats.Remove(move);
        btn.clicked -= delegate{RemoveWinsTo(move, btn);};
        SaveMove();
        RefreshMoveView();
    }
    private void ChangeMoveName(FocusOutEvent focusOutEvent)
    {
        if(_activeMove != null)
        {
            string prevName = _activeMove.moveName;
            _activeMove.moveName = moveName.value;
            changesMade.Add("Name changed from "+prevName+" to "+_activeMove.moveName);
            SaveMove();
            // RefreshMoveView();
            RefreshMovesList();
        }
    }
    private void ChangeMoveIcon()
    {
        string prevImg = "";
        if(_activeMove.img != null)
        {
            prevImg = _activeMove.img.name;
        }
        
        Sprite newIcon = changeIcon.value as Sprite;
        _activeMove.img = newIcon;
        moveIcon.style.backgroundImage = new StyleBackground(_activeMove.img);
        changesMade.Add("Changed the move icon from "+prevImg+" to "+newIcon.name);
        SaveMove();
        // RefreshMoveView();
    }
    private void AddWinAgainst()
    {
        if(_activeMove != null && addToWinAgainstField.value != null)
        {
            MoveClass newWinsAgainst = addToWinAgainstField.value as MoveClass;
            _activeMove.beats.Add(newWinsAgainst);
            changesMade.Add("Added wins to: "+newWinsAgainst.moveName);
            SaveMove();
            RefreshMoveView();
        }
    }
    private void SaveMove()
    {
        EditorUtility.SetDirty(_activeMove);
        AssetDatabase.SaveAssetIfDirty(_activeMove);
        AssetDatabase.Refresh();
    }

    private void CreateMoveOnClick()
    {
        CreateMove.ShowWindow();
    }
    private void DeleteMove()
    {

        var path = AssetDatabase.GetAssetPath(_activeMove);
        AssetDatabase.DeleteAsset(path);
        RefreshMovesList();
        ClearMoveView();
    }

    private void RegisterEvents()
    {
        moveName.RegisterCallback<FocusOutEvent>(ChangeMoveName);
        // changeIcon?.UnregisterCallback<FocusOutEvent>(ChangeMoveIcon);
    }

    private void UnRegisterEvents()
    {
        moveName?.UnregisterCallback<FocusOutEvent>(ChangeMoveName);
        // changeIcon?.UnregisterCallback<FocusOutEvent>(ChangeMoveIcon);
    }
}
