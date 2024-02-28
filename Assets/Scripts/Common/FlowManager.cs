using SKCell;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.VisualScripting.Icons;

public class FlowManager : SKMonoSingleton<FlowManager>
{
    SceneTitle sceneTitle;
    public float crashCount;
    private int crashCountMax = 10;
    private void Start()
    {
        Application.targetFrameRate = 60;
        sceneTitle = (SceneTitle)PlayerPrefs.GetInt("StartScene");
        LoadScene(sceneTitle);

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

    public void LoadScene(SceneTitle scene)
    {
        SKSceneManager.instance.LoadSceneAsync("loading", scene.ToString());
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

    public void OnCollisionHappens(float increase, GameObject crash)
    {
        //RuntimeData.timeScale = 0;
        //UIManager.instance.SetState_FailPanel(true);
        crashCount += increase;
        print(crashCount);
        if(crash != null)
        {
            StartCoroutine(WaitToDestroy(crash));
        }
        if (crashCount >= crashCountMax)
        {
            LevelFail();
        }
    }
    public IEnumerator WaitToDestroy(GameObject obj)
    {
        yield return new WaitForFixedUpdate();
        Destroy(obj);
    }
    public void OnPlayerCollision()
    {
        RuntimeData.timeScale = 0;
        UIManager.instance.SetState_DeathPanel(true);
    }
    public void LevelFail()
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
