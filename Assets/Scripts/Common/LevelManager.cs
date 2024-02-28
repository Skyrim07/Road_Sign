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
        UIManager.instance.SetValue_ProgressBar(val01);
    }
}
