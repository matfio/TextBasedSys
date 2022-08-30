using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextFieldWithEffect : MonoBehaviour
{
    [SerializeField]
    private float MinimumDelay;
    [SerializeField]
    private float MaximumDelay;

    private Text textField;
    private float delay;
    private string currentText;

    private Coroutine writingText;


    private void Awake()
    {
        textField = GetComponent<Text>();
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            StopCoroutine(writingText);
            writingText = null;
            textField.text = currentText;
        }
    }

    public void SetText(string text)
    {
        textField.text = "";
        if(writingText != null)
        {
            StopCoroutine(writingText);
        }

        currentText = text;
        writingText = StartCoroutine(setTextInternal());
    }

    private IEnumerator setTextInternal()
    {
        var length = currentText.Length;
        for (var i = 0; i < length; i++)
        {
            textField.text += currentText[i];

            delay = Random.Range(MinimumDelay, MaximumDelay);
            yield return new WaitForSeconds(delay);
        }

        writingText = null;
    }
}
