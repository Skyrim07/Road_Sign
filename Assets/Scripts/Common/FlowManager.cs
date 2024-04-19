using SKCell;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class FlowManager : SKMonoSingleton<FlowManager>
{
    SceneTitle sceneTitle;

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
            if (!UIManager.instance.CanOpenPausePanel())
                return;
            if (RuntimeData.isPaused)
                UnPause();
            else
            {
                Pause();
                CommonReference.instance.pausePanel.SetState(true);
               // CommonReference.instance.pause_TitleText.textAnimator.PlayTypeWriter();
            }

        });
    }
    public void LoadTutorialPanel()
    {
        Pause();
        UIManager.instance.SetState_TutorialPanel(true);
    }
    public void LoadMainMenu()
    {
        LoadScene(SceneTitle.MainMenu);
        UnPause();
    }

    public void LoadNextLevel()
    {
        LoadScene((SceneTitle)((int)RuntimeData.currentScene + 1));
    }
    public void LoadScene(SceneTitle scene)
    {
        RuntimeData.currentScene = scene;
        SKSceneManager.instance.LoadSceneAsync("loading", scene.ToString());
        if(scene == SceneTitle.Level1)
        {
            //StartCoroutine(WaitToLoad(3));
        }
  
    }
    public void Pause()
    {
        SKAudioManager.instance.PlaySound("honkpause1");
        RuntimeData.isPaused = true;
        RuntimeData.timeScale = 0.0f;

        UIManager.instance.buttonPauseResume.Select();
        print("pause");
    }

    public void UnPause()
    {
        SKAudioManager.instance.PlaySound("honkunpause1");
        RuntimeData.isPaused = false;
        RuntimeData.timeScale = 1.0f;
        CommonReference.instance.pausePanel.SetState(false);

        print("unpause");
    }

    public void OnCollisionHappens(float increase, GameObject crash)
    {
        GameObject fx = Instantiate(CommonReference.instance.carExplosionFx, crash.transform.position, Quaternion.identity);
        Destroy(fx, 5);

        if (RuntimeData.isLevelComplete) return;

        RuntimeData.crashCount += increase;
        LevelManager.instance.AddProgressValue(-.15f);
        if (RuntimeData.crashCount >= RuntimeData.crashCountMax)
        {
            LevelFail();
        }

        UIManager.instance.UpdateCrashIndicator();
    }
    public void OnPlayerCollision()
    {
        SKAudioManager.instance.PlaySound("hit");

        if (RuntimeData.isLevelComplete) return;

        PlayerLogic.instance.AddHealth(-1);

        if (RuntimeData.playerHealth <= 0)
        {
            OnPlayerDeath();
        }
    }

    public void OnPlayerDeath()
    {
        if (RuntimeData.isLevelComplete) return;
        RuntimeData.timeScale = 0;
        UIManager.instance.SetState_DeathPanel(true);
    }
    public void LevelFail()
    {
        RuntimeData.timeScale = 0;
        UIManager.instance.SetState_CrashPanel(true);
    }
    public IEnumerator WaitToDestroy(GameObject crash)
    {
        yield return new WaitForEndOfFrame();
        Destroy(crash);
    }
    public IEnumerator WaitToLoad(int sec)
    {
        yield return new WaitForSeconds(sec);
        LoadTutorialPanel();
    }
    public void RestartLevel()
    {
        RuntimeData.timeScale = 1;
        PlayerLogic.instance.SetHealth(3);
        LoadScene(RuntimeData.currentScene);
    }
}


public class EventRef
{
    public static int ON_LOAD_LEVEL = 0;
}
