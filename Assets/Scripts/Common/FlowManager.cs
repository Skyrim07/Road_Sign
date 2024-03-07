using SKCell;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class FlowManager : SKMonoSingleton<FlowManager>
{
    SceneTitle sceneTitle;
    [SerializeField] private float crashCount;
    [SerializeField] private float crashCountMax = 10f;

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
    public void LoadScene(SceneTitle scene)
    {
        SKSceneManager.instance.LoadSceneAsync("loading", scene.ToString());
        if(scene == SceneTitle.Level1)
        {
            StartCoroutine(WaitToLoad(3));
        }
  
    }
    public void Pause()
    {
        RuntimeData.isPaused = true;
        RuntimeData.timeScale = 0.0f;
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
        LevelManager.instance.AddProgressValue(-.15f);

        if(crashCount >= crashCountMax)
        {
            LevelFail();
        }
    }
    public void OnPlayerCollision()
    {
        PlayerLogic.instance.DestroySign();
        RuntimeData.timeScale = 0;
        UIManager.instance.SetState_DeathPanel(true);
    }
    public void LevelFail()
    {
        RuntimeData.timeScale = 0;
        UIManager.instance.SetState_FailPanel(true);
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}


public class EventRef
{
    public static int ON_LOAD_LEVEL = 0;
}
