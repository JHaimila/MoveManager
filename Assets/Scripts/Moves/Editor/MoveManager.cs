using System.Collections;
using System.Collections.Generic;
using Codice.Client.BaseCommands;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine.UI;
using Button = UnityEngine.UIElements.Button;

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
    private Label errorText;
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

        errorText = container.Q<Label>("errorList");
        
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
            MoveListItem newMove = new MoveListItem(move.moveName, GetIncompatableMoves(move));
            newMove.GetButton().clicked += delegate {OnMoveClicked(move);};
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
        errorText.text = "";
        if(_activeMove != null)
        {
            ClearMoveView();
            UnRegisterEvents();
            moveName.value = _activeMove.moveName;
            moveIcon.style.backgroundImage = new StyleBackground(_activeMove.img);
            changeIcon.value = _activeMove.img;
            addToWinAgainstField.value = null;
            List<string> incompatableMoves = GetIncompatableMoves(_activeMove);
            if (incompatableMoves.Count > 0)
            {
                foreach (var incompatableMove in incompatableMoves)
                {
                    errorText.text += incompatableMove + ", ";
                }
            }
            else
            {
                errorText.text = "";
            }
            
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
        RefreshMovesList();
    }
    private void ChangeMoveName(FocusOutEvent focusOutEvent)
    {
        if(_activeMove != null)
        {
            _activeMove.moveName = moveName.value;
            SaveMove();
            RefreshMovesList();
        }
    }
    private void ChangeMoveIcon()
    {
        if (_activeMove == null) {return;}
        
        Sprite newIcon = changeIcon.value as Sprite;
        _activeMove.img = newIcon;
        moveIcon.style.backgroundImage = new StyleBackground(_activeMove.img);
        SaveMove();
    }
    private void AddWinAgainst()
    {
        if(_activeMove != null && addToWinAgainstField.value != null)
        {
            MoveClass newWinsAgainst = addToWinAgainstField.value as MoveClass;
            _activeMove.beats.Add(newWinsAgainst);
            SaveMove();
            RefreshMoveView();
            RefreshMovesList();
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
        if (_activeMove == null) { return;}
        
        foreach (var move in _moves)
        {
            if (move.beats.Contains(_activeMove))
            {
                move.beats.Remove(_activeMove);
            }
        }
        
        var path = AssetDatabase.GetAssetPath(_activeMove);
        AssetDatabase.DeleteAsset(path);
        RefreshMovesList();
        ClearMoveView();
    }

    private void RegisterEvents()
    {
        moveName.RegisterCallback<FocusOutEvent>(ChangeMoveName);
    }

    private void UnRegisterEvents()
    {
        moveName?.UnregisterCallback<FocusOutEvent>(ChangeMoveName);
    }
    
    public List<string> GetIncompatableMoves(MoveClass givenMove)
    {
        List<string> failedMoves = new List<string>();
        foreach (MoveClass move in _moves)
        {
            if (move == givenMove || givenMove.beats.Contains(move) || move.beats.Contains(givenMove))
            {
                continue;
            }
            failedMoves.Add(move.moveName);
        }
        return failedMoves;
    }
}
