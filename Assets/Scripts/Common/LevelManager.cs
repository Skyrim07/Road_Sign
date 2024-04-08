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
            PlayerLogic.instance.SetHealth(3);
            RuntimeData.isLevelComplete = false;
            RuntimeData.crashCount = 0;
            RuntimeData.currentSignCount = 0;
            UIManager.instance.UpdateCrashIndicator();
            SKUtils.InvokeAction(1f, () =>
            {
                LoadSignSlots();
            });
     
        }));
    }

    private void LoadSignSlots()
    {
        RuntimeData.signCountMax = GameObject.FindObjectsOfType<SignSlot>().Length;
    }
    public void OnPlaceSlot()
    {
        RuntimeData.currentSignCount++;
        print(RuntimeData.currentSignCount + "   " + RuntimeData.signCountMax);
        if(RuntimeData.currentSignCount >= RuntimeData.signCountMax)
        {
            OnLevelComplete();
        }

    }
    public void OnDeleteSlot()
    {
        RuntimeData.currentSignCount--;
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
            //OnLevelComplete();
        }
    }

    public void OnLevelComplete()
    {
        SKAudioManager.instance.StopMusic();
        SKAudioManager.instance.PlayMusic("Real Success", false, 0, 1);
        RuntimeData.isLevelComplete = true;
        UIManager.instance.SetState_WinPanel(true);
    }
}
