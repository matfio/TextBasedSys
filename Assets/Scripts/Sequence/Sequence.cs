using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class Sequence
{ 
    public string id;

    private Dictionary<string, Scene> scenes;

    //Only used when saving to JSON
    [SerializeField]
    private List<Scene> serializableSceneList;

    public Sequence()
    {
        scenes = new Dictionary<string, Scene>();
    }

    public void AddScene()
    {
        var scene = new Scene();
        scene.id = "New Scene " + GetAllScenes().Count;
        AddScene(scene);
    }

    public void AddScene(Scene scene)
    {
        if(!scenes.ContainsKey(scene.id))
        {
            scenes.Add(scene.id, scene);
        }
        else
        {
            Debug.Log("Scene called " + scene.id + " already exists");
        }
    }

    public Scene GetScene(string sceneId)
    {
        Scene outValue;
        scenes.TryGetValue(sceneId, out outValue);
        return outValue;
    }

    public List<Scene> GetAllScenes()
    {
        return new List<Scene>(scenes.Values);
    }

    public Dictionary<string, Scene> GetScenesDict()
    {
        return scenes;
    }

    public void SaveToResources()
    {
        File.WriteAllText(Application.dataPath + "/Resources/Sequences/" + id + ".json", SaveToString());

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    public string SaveToString()
    {
        serializableSceneList = new List<Scene>();
        foreach(var pair in scenes)
        {
            serializableSceneList.Add(pair.Value);
        }

        return JsonUtility.ToJson(this);
    }

    public static Sequence LoadFromJson(string inputJson)
    {
        var sequence = JsonUtility.FromJson<Sequence>(inputJson);
        Scene scene;

        for(int i = 0; i < sequence.serializableSceneList.Count; i++)
        {
            scene = sequence.serializableSceneList[i];
            sequence.AddScene(scene);
        }

        sequence.serializableSceneList.Clear();
        sequence.serializableSceneList = null;

        return sequence;
    }
}



