using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Scene
{
    public string id;

    [SerializeField]
    public DisplayText displayText;

    [SerializeField]
    public List<Command> commands;

    [SerializeField]
    public List<Action> actions;

    public Scene()
    {
        id = "rename Me";
        displayText = new DisplayText();
        commands = new List<Command>();
        actions = new List<Action>();
    }

    public void AddGotoAction(string sceneId)
    {
        var action = new Action();
        var command = new Command();
        command.type = Command.TYPE.GOTO_SCENE;
        command.commandText = sceneId;
        action.commands.Add(command);

        actions.Add(action);
    }
}