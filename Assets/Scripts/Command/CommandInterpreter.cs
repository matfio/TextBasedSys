using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandInterpreter : MonoBehaviour
{
    const string avatarGraphicsLocation = "AvatarGraphics/";

    [SerializeField]
    private Image imgComponent;

    public delegate void SceneBackgroundCommand(SceneBackgroundDirector.SceneBackgroundState state);
    public static event SceneBackgroundCommand OnSceneBackgroundCommand;

    public delegate void PopupCommand(string popupDescription);
    public static event PopupCommand OnPopupCommand;

    public delegate void GoToCommand(string sceneId);
    public static event GoToCommand OnGoToCommand;

    // Use this for initialization
    private void Awake()
    {
        GameDirector.OnCommand += OnCommandReceived;
    }

    private void OnCommandReceived(Command c)
    {
        if(c == null)
        {
            Debug.Log("Command is null");
            return;
        }

        switch(c.type)
        {
            case Command.TYPE.LOAD_IMAGE:
                ExecuteAvatarCommand(c.commandText);
                break;
            case Command.TYPE.GOTO_SCENE:
                ExecuteGoToCommand(c.commandText);
                break;
        }
    }

    private void ExecuteGoToCommand(string commandText)
    {
        if(OnGoToCommand != null)
        {
            OnGoToCommand(commandText);
        }
    }

    private void ExecuteAvatarCommand(string command)
    {
        var avatar = Resources.Load<Sprite>(avatarGraphicsLocation + command);
        imgComponent.sprite = avatar;
    }
}
