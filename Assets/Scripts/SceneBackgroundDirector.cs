using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneBackgroundDirector : MonoBehaviour
{
    public enum SceneBackgroundState
    {
        BASE,
        ITALY,
        DENMARK
    };

    [SerializeField]
    public Sprite italy;

    [SerializeField]
    public Sprite denmark;

    [SerializeField]
    public SceneBackgroundState sceneState;

    private Image imgComponent;

    // Use this for initialization
    void Start() {
        imgComponent = transform.GetComponent<Image>();
        CommandInterpreter.OnSceneBackgroundCommand += OnSceneBackgroundCommandReceived;
    }

    private void OnSceneBackgroundCommandReceived(SceneBackgroundState state)
    {
        sceneState = state;

        switch (sceneState)
        {
            case SceneBackgroundState.ITALY:
                EnableImageIfDisabled();
                imgComponent.sprite = italy;
                break;
            case SceneBackgroundState.DENMARK:
                EnableImageIfDisabled();
                imgComponent.sprite = denmark;
                break;
            case SceneBackgroundState.BASE:
                EnableBlackBackground();
                break;
        }
    }

    private void EnableImageIfDisabled()
    {
        if(!imgComponent.IsActive())
        {
            imgComponent.enabled = true;
        }
    }

    private void EnableBlackBackground()
    {
        var tr = transform.parent.Find("BlackFilling");
        if (tr != null)
        {
            tr.gameObject.SetActive(true);
        }
    }
}