using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Action
{
    public string choiceText;

    [SerializeField]
    public List<Command> commands;

    public Action()
    {
        commands = new List<Command>();
    }
}
