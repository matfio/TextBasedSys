using UnityEngine;
using UnityEngine.UI;

public class TextInterpreter : MonoBehaviour
{
    [SerializeField]
    private TextFieldWithEffect primaryField;

    private void Awake()
    {
        GameDirector.OnTextCommand += OnTextCommandReceived;
    }

    private void OnTextCommandReceived(DisplayText dt)
    {
        if(dt.output == DisplayText.OUTPUT.PRIMARY)
        {
            primaryField.SetText(dt.text);
        }
    }
}