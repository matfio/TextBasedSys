using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SceneTool : EditorWindow
{
    private Sequence parentSequence;

    private Scene scene;

    [SerializeField]
    private Command[] commands;

    [SerializeField]
    private Action[] actions;

    public static void ShowWindow(Sequence parentSequence, string sceneId)
    {
        var tool = EditorWindow.GetWindow(typeof(SceneTool));
        ((SceneTool)tool).SetScene(parentSequence, sceneId);
    }

    public static void ShowWindow(Sequence parentSequence)
    {
        var tool = EditorWindow.GetWindow(typeof(SceneTool));
        ((SceneTool)tool).SetScene(parentSequence);
    }

    public void SetScene(Sequence parentSequence)
    {
        scene = new Scene();
        parentSequence.AddScene(scene);
        SetScene(parentSequence, scene.id);
    }

    public void SetScene(Sequence parentSequence, string sceneId)
    {
        this.parentSequence = parentSequence;
        this.scene = parentSequence.GetScene(sceneId);

        if (scene != null)
        {
            if (scene.commands != null)
            {
                commands = scene.commands.ToArray();
            }
            else
            {
                commands = new Command[0];
            }

            if (scene.actions != null)
            {
                actions = scene.actions.ToArray();
            }
            else
            {
                actions = new Action[0];
            }
        }
    }
    
    void OnGUI()
    {
        scene.id = EditorGUILayout.TextField(new GUIContent("Scene id:"), scene.id);
        scene.displayText.text = EditorGUILayout.TextArea(scene.displayText.text, GUILayout.Height(position.height - 400));
        scene.displayText.output = (DisplayText.OUTPUT)EditorGUILayout.EnumPopup("Text displayed:", scene.displayText.output);

        DisplayList("commands");

        DisplayList("actions");

        if (GUILayout.Button("Export to Json"))
        {
            parentSequence.SaveToResources();
        }
    }
    
    private void DisplayList(string property)
    {
        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);
        SerializedProperty stringsProperty = so.FindProperty(property);

        EditorGUILayout.PropertyField(stringsProperty, true); // True means show children
        so.ApplyModifiedProperties();
    }
}