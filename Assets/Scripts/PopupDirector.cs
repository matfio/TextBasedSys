using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupDirector : MonoBehaviour
{
    [SerializeField]
    private GameObject popup;

    [SerializeField]
    private Button[] buttons;

	// Use this for initialization
	void Start ()
    {
        buttons = popup.GetComponentsInChildren<Button>(true);
        CommandInterpreter.OnPopupCommand += OnPopupCommandReceived;
    }

    private void OnPopupCommandReceived(string popupDescription)
    {
        Text textField;
        string[] commands = popupDescription.Split(' ');

        popup.SetActive(true);

        for(int i = 1; i < commands.Length; i++)
        {
            if(i - 1 < buttons.Length)
            { 
                textField = buttons[i - 1].GetComponentInChildren<Text>();
                if(textField)
                {
                    textField.text = commands[i];
                    textField.gameObject.SetActive(true);
                    textField.transform.parent.gameObject.SetActive(true);
                }
            }
        }
    }
}
