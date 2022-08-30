using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SequenceTool : EditorWindow
{
    private const int windowWidth = 300;
    private const int windowHeigth = 300;
    private const int maxStringLength = 200;
    private const int actionWidth = 80;

    private Sequence currentSequence;

    private List<Scene> scenes;
    private Dictionary<string, Rect> rects;

    private bool shouldDrawMouseLine;
    private Rect clickedCommandRect;
    private Command clickedCommand;
    private Vector2 scrollPosition;
    private Rect view = new Rect(0, 0, 800, 10000);
    private Rect window = new Rect(0, 50, 800, 600);

    public static void Show(Sequence sequence)
    {
        var editor = EditorWindow.GetWindow<SequenceTool>();
        editor.Init(sequence);
    }

    private void Init(Sequence sequence)
    {
        currentSequence = sequence;
        rects = new Dictionary<string, Rect>();
        scenes = currentSequence.GetAllScenes();
    }

    private void OnGUI()
    {
        scrollPosition = GUI.BeginScrollView(window, scrollPosition, view);
        //Draw scene windows
        BeginWindows();    
        Rect rt;
        Scene sc;

        var lengthScenes = scenes.Count;
        for (int j = 0; j < lengthScenes; j++)
        {
            sc = scenes[j];
            if (rects.ContainsKey(sc.id))
            {
                rt = rects[sc.id];
            }
            else
            {
                rt = new Rect(5, 50, windowWidth, windowHeigth);
                rects.Add(sc.id, rt);
            }

            rects[sc.id] = GUI.Window(j, rt, DrawWindowGUI, sc.id);
        }
        EndWindows();

        DrawGotoScenes();

        DrawMouseLine();

        GUI.EndScrollView();


        GUIStyle gsTest = new GUIStyle();
        gsTest.normal.background = new Texture2D(800, 50);

        GUILayout.BeginVertical(gsTest);
        //Title
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Box(currentSequence.id);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        //Scene options
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add scene"))
        {
            currentSequence.AddScene();
            scenes = currentSequence.GetAllScenes();
        }

        if (GUILayout.Button("Save"))
        {
            currentSequence.SaveToResources();
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    private void DrawWindowGUI(int id)
    {
        var scene = scenes[id];
        var actions = scene.actions;

        GUI.skin.label.wordWrap = true;
        GUI.DragWindow(new Rect(0,0, windowWidth, 30));

        var displayText = scene.displayText.text;
        if(displayText != null)
        {
            if (displayText.Length > maxStringLength)
            {
                displayText = displayText.Remove(maxStringLength);
                displayText += "...";
            }

            GUILayout.Label(displayText);
        }

        if (GUILayout.Button("Edit Scene"))
        {
            SceneTool.ShowWindow(currentSequence, scene.id);
        }

        if (GUILayout.Button("Add GOTO action"))
        {
            scene.AddGotoAction("sceneId");
        }
    }

    private void DrawGotoScenes()
    {
        Scene scene;
        Action action;
        Command command;
        Rect rect;

        for (int sceneId = 0; sceneId < scenes.Count; sceneId++)
        {
            scene = scenes[sceneId];
            rect = rects[scene.id];

            for (int i = 0; i < scene.actions.Count; i++)
            {
                action = scene.actions[i];
                for (int j = 0; j < action.commands.Count; j++)
                {
                    command = action.commands[j];
                    if (command.type == Command.TYPE.GOTO_SCENE)
                    {
                        var gotoSceneId = command.commandText;
                        //This Rect is on top of a SceneWindow.
                        //It needs to be drawn here because the curve need to go from window to window
                        var boxRect = new Rect(rect.x + ((actionWidth + 5) * i), rect.y + windowHeigth, actionWidth, 50);
                        GUI.Box(boxRect, gotoSceneId);

                        //Is mouse down onto some gotoScene?
                        Vector3 mousePosition = Event.current.mousePosition;
                        if (Event.current.type == EventType.MouseDown)
                        {
                            if(mousePosition.x >= boxRect.x && mousePosition.x <= boxRect.x + boxRect.width
                                && mousePosition.y >= boxRect.y && mousePosition.y <= boxRect.y + boxRect.height)
                            {
                                shouldDrawMouseLine = true;
                                clickedCommandRect = boxRect;
                                clickedCommand = command;
                            }
                        }

                        if (rects.ContainsKey(gotoSceneId))
                        {
                            DrawNodeCurve(boxRect, rects[gotoSceneId]);
                        }
                    }
                }
            }
        }
    }

    private void DrawMouseLine()
    {
        Vector3 mousePosition = Event.current.mousePosition;
        if (shouldDrawMouseLine)
        {
            Handles.DrawLine(new Vector3(clickedCommandRect.x + clickedCommandRect.width / 2, clickedCommandRect.y + clickedCommandRect.width / 2), mousePosition);
        };

        if (Event.current.type == EventType.MouseUp && shouldDrawMouseLine)
        {
            shouldDrawMouseLine = false;
            
            foreach(var keyPair in rects)
            {
                var rect = keyPair.Value;
                if (mousePosition.x >= rect.x && mousePosition.x <= rect.x + rect.width
                                && mousePosition.y >= rect.y && mousePosition.y <= rect.y + rect.height)
                {
                    clickedCommand.commandText = keyPair.Key;
                }
            }
        }
    }

    private void DrawNodeCurve(Rect start, Rect end)
    {
        Vector3 startPos = new Vector3(start.x + start.width / 2, start.y + start.height, 0);
        Vector3 endPos = new Vector3(end.x, end.y, 0);
        Vector3 startTan = startPos + Vector3.up * 50;
        Vector3 endTan = endPos + Vector3.left * 50;
        Color shadowCol = new Color(0, 0, 0, 0.06f);
        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 5);
    }
}
