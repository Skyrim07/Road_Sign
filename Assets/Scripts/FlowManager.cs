using SKCell;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowManager : MonoBehaviour
{
    private void Start()
    {
        SKUtils.AddKeyDownAction(KeyCode.LeftAlt, () =>
        {
            SKConsole.Toggle();
        });
        SKUtils.AddKeyDownAction(KeyCode.Escape, () =>
        {
            if (RuntimeData.isPaused)
                UnPause();
            else
                Pause();
        });
    }

    public void Pause()
    {
        RuntimeData.isPaused = true;
        RuntimeData.timeScale = 0.0f;
        CommonReference.instance.pausePanel.SetState(true);
    }

    public void UnPause()
    {
        RuntimeData.isPaused = false;
        RuntimeData.timeScale = 1.0f;
        CommonReference.instance.pausePanel.SetState(false);
    }
}
