using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ActionInterpreter : MonoBehaviour
{
    [SerializeField]
    private RectTransform parentObject;

    [SerializeField]
    private Button buttonPrefab;

    private List<Button> buttons;

    // Use this for initialization
    private void Awake()
    {
        buttons = new List<Button>();
        GameDirector.OnActionCreationEvent += OnActionCreationEventReceived;
        GameDirector.OnNextSceneEvent += OnNextSceneEventReceived;
    }

    private void OnNextSceneEventReceived(Scene s)
    {
        Button tempBtn;
        for (int i = buttons.Count - 1; i >= 0; i--)
        {
            tempBtn = buttons[i];
            buttons.RemoveAt(i);
            Destroy(tempBtn.gameObject);
        }
    }

    private void OnActionCreationEventReceived(Action a, Func<Action, UnityAction> buttonAction)
    {
        if(a == null)
        {
            Debug.Log("Action is null");
            return;
        }

        var btn = Instantiate(buttonPrefab);
        var rectTransform = btn.GetComponent<RectTransform>();
        var btnText = btn.GetComponentInChildren<Text>();

        rectTransform.SetParent(parentObject);
        btnText.text = a.choiceText;
        rectTransform.anchoredPosition3D = Vector3.zero;
        rectTransform.localScale = Vector3.one;
        btn.onClick.AddListener(()=> { buttonAction(a); });

        buttons.Add(btn);
    }
}
