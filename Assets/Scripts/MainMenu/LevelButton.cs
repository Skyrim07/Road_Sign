using SKCell;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public SceneTitle scene;

    [SerializeField] public Button button;
    public void OnSelect()
    {
        SKAudioManager.instance.PlaySound("UI3");
        button.interactable = false;
        FlowManager.instance.LoadScene(scene);
    }
}
