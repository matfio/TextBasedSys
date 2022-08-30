using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameDirector : MonoBehaviour
{
    public delegate void CommandEvent(Command c);
    public static event CommandEvent OnCommand;

    public delegate void ActionCreationEvent(Action a, Func<Action, UnityAction> buttonAction);
    public static event ActionCreationEvent OnActionCreationEvent;

    public delegate void TextEvent(DisplayText dt);
    public static event TextEvent OnTextCommand;

    public delegate void NextSceneEvent(Scene s);
    public static event NextSceneEvent OnNextSceneEvent;

    private List<Sequence> sequences;
    private int currentSequenceIndex;
    private int currentSceneIndex;

    // Use this for initialization
    void Awake()
    {
        CommandInterpreter.OnGoToCommand += OnGoToCommandReceived;

        currentSequenceIndex = 0;
        currentSceneIndex = -1;
        sequences = new List<Sequence>();

        var resources = Resources.LoadAll("Sequences/");
        var length = resources.Length;

        for (int i = 0; i < length; i++)
        {
            sequences.Add(Sequence.LoadFromJson(resources[i].ToString()));
        }
    }

    private void Start()
    {
        GoToScene("firstScene");
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void GoToScene(string sceneId)
    {
        var currentScene = sequences[currentSequenceIndex].GetScene(sceneId);

        if(OnNextSceneEvent != null)
        {
            OnNextSceneEvent(currentScene);
        }

        ExecuteCommands(currentScene.commands);

        CreateChoices(currentScene.actions);

        DisplayText currentDisplayText = currentScene.displayText;
        if (currentDisplayText != null)
        {
            OnTextCommand(currentDisplayText);
        }
    }

    private void CreateChoices(List<Action> actions)
    {
        var length = actions.Count;
        for (int i = 0; i < length; i++)
        {
            if(OnActionCreationEvent != null)
            {
                OnActionCreationEvent(actions[i], OnButtonClicked);
            }
        }
    }

    private void ExecuteCommands(List<Command> commands)
    {
        if (commands == null)
        {
            return;
        }

        var length = commands.Count;
        for (int i = 0; i < length; i++)
        {
            OnCommand(commands[i]);
        }
    }

    private UnityAction OnButtonClicked(Action a)
    {
        if(a.commands == null || a.commands.Count == 0)
        {
            Debug.Log("This action has no commands");
        }

        ExecuteCommands(a.commands);
        return null;
    }


    private void OnGoToCommandReceived(string sceneId)
    {
        GoToScene(sceneId);
    }
}
