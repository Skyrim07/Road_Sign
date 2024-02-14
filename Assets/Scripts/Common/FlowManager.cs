using SKCell;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlowManager : SKMonoSingleton<FlowManager>
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
        CommonReference.instance.pause_TitleText.textAnimator.PlayTypeWriter();
    }

    public void UnPause()
    {
        RuntimeData.isPaused = false;
        RuntimeData.timeScale = 1.0f;
        CommonReference.instance.pausePanel.SetState(false);
    }

    public void OnCollisionHappens()
    {
        RuntimeData.timeScale = 0;
        UIManager.instance.SetState_FailPanel(true);
    }

    public void RestartLevel()
    {
        RuntimeData.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
