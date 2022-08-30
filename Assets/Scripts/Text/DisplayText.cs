using UnityEngine;

[System.Serializable]
public class DisplayText
{
    public enum OUTPUT
    {
        PRIMARY
    }

    [SerializeField]
    public OUTPUT output;

    public string text;

    public DisplayText()
    {
        output = OUTPUT.PRIMARY;
    }
}