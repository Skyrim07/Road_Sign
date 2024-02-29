using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SKCell;
public class LevelManager : SKMonoSingleton<LevelManager>
{
    private void Start()
    {
        EventDispatcher.AddListener(EventDispatcher.Common, EventRef.ON_LOAD_LEVEL, new SKEvent(() =>
        {
           SetProgressValue(.25f);
        }));
    }

    public void AddProgressValue(float delta01)
    {
        SetProgressValue(RuntimeData.currentProgress + delta01);
    }
    public void SetProgressValue(float val01)
    {
        RuntimeData.currentProgress = val01;
        RuntimeData.currentProgress = Mathf.Clamp01(RuntimeData.currentProgress);
        UIManager.instance.SetValue_ProgressBar(val01);

        if (RuntimeData.currentProgress >= 1.0f)
        {
            OnLevelComplete();
        }
    }

    public void OnLevelComplete()
    {
        print("level complete!");
        UIManager.instance.SetState_WinPanel(true);
    }
}
