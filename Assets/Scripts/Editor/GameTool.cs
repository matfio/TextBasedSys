using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GameTool : EditorWindow
{
    public static List<Sequence> sequences;

    [MenuItem("M'azing/Game Tool")]
    private static void ShowGameTool()
    {
        UpdateSequencesList();
        EditorWindow.GetWindow(typeof(GameTool));     
    }

    private static void UpdateSequencesList()
    {
        if(sequences != null)
        {
            sequences.Clear();
        }
        else
        {
            sequences = new List<Sequence>();
        }
        
        var resources = Resources.LoadAll("Sequences/");
        var length = resources.Length;
        Sequence tempSeq;

        for (int i = 0; i < length; i++)
        {
            tempSeq = Sequence.LoadFromJson(resources[i].ToString());
            if (tempSeq != null)
            {
                sequences.Add(tempSeq);
            }
        }

        //if empty, create one sequence
        if(sequences.Count <= 0)
        {
            AddNewSequence();
        }
    }

    private void OnGUI()
    {
        var lengthSeq = sequences.Count;
        for (int i = 0; i < lengthSeq; i++)
        {
            GUILayout.Label(sequences[i].id);
            if (GUILayout.Button("  Modify"))
            {
                SequenceTool.Show(sequences[i]);
            }
        }

        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Sequence"))
        {
            AddNewSequence();
        }

        if (GUILayout.Button("Refresh Sequences"))
        {
            UpdateSequencesList();
        }
        GUILayout.EndHorizontal();
    }

    private static void AddNewSequence()
    {
        var newSequence = new Sequence();
        newSequence.id = (sequences.Count + 1).ToString();
        sequences.Add(newSequence);
    }
}