using SKCell;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SKMonoSingleton<UIManager> 
{
    public SKUIPanel colorFactoryPanel, shapeFactoryPanel;
    public SKUIPanel winPanel;
    public SKUIPanel deathPanel, ticketPanel, crashPanel, hatPanel;
    [SerializeField] SKUIPanel tutPanel;
    [SerializeField] SKSlider progressBar;

    [SerializeField] CrashIndicator crashIndicator;
    [SerializeField] Transform lifeIconContainer;

    public Button buttonPauseResume;

    private LifeIcon[] lifeIcons;

    private void Start()
    {
        lifeIcons = new LifeIcon[lifeIconContainer.childCount];
        for (int i = 0; i < lifeIconContainer.childCount; i++)
        {
            lifeIcons[i] = lifeIconContainer.GetChild(i).GetComponent<LifeIcon>();  
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (winPanel.active)
            {
                FlowManager.instance.LoadNextLevel();
                SetState_WinPanel(false);
                SKAudioManager.instance.PlaySound("UI3");
            }
            if (deathPanel.active)
            {
                FlowManager.instance.RestartLevel();
                SetState_DeathPanel(false);
                SKAudioManager.instance.PlaySound("UI3");
            }
            if (crashPanel.active)
            {
                FlowManager.instance.RestartLevel();
                SetState_CrashPanel(false);
                SKAudioManager.instance.PlaySound("UI3");
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (winPanel.active)
            {
                FlowManager.instance.LoadMainMenu();
                SetState_WinPanel(false);
                SKAudioManager.instance.PlaySound("UI3");
            }
            if (deathPanel.active)
            {
                FlowManager.instance.LoadMainMenu();
                SetState_DeathPanel(false);
                SKAudioManager.instance.PlaySound("UI3");
            }
        }
    }

    public bool CanOpenPausePanel()
    {
        bool can = true;
        can &= !deathPanel.active;
        can &= !winPanel.active;
        can &= !NewSignPanel.instance.panel.active;
        can &= !crashPanel.active;
        can &= !colorFactoryPanel.active;
        can &= !shapeFactoryPanel.active;
        print(can);
        return can;
    }
    public void SetValue_ProgressBar(float value01)
    {
       // progressBar.SetValue(value01);
    }
    public void SetState_HatPanel(bool active)
    {
        if(active)
            HatPanel.instance.UpdateInfo();
        hatPanel.SetState(active);
    }
    public void SetState_FailPanel(bool active)
    {
        ticketPanel.SetState(active);
    }
    public void SetState_WinPanel(bool active)
    {
        winPanel.SetState(active);
    }
    public void SetState_DeathPanel(bool active)
    {
        deathPanel.SetState(active);
    }
    public void SetState_CrashPanel(bool active)
    {
        crashPanel.SetState(active);
    }
    public void SetState_TutorialPanel(bool active)
    {
        tutPanel.SetState(active);
    }
    public void SetState_ColorFactoryPanel(bool active)
    {
        RuntimeData.timeScale = active?0:1;
        colorFactoryPanel.SetState(active);
        if(active)
        ColorFactory.instance.Init();
    }
    public void SetState_ShapeFactoryPanel(bool active)
    {
        RuntimeData.timeScale = active ? 0 : 1;
        shapeFactoryPanel.SetState(active);
        if (active)
            ShapeFactory.instance.Init();
    }

    public void UpdateLifeIcon()
    {
        for(int i=0;i<lifeIconContainer.childCount;i++)
        {
            lifeIcons[i].SetState(i < RuntimeData.playerHealth);
        }
    }

    public void UpdateCrashIndicator()
    {
        crashIndicator.SetValue((int)RuntimeData.crashCount, (int)RuntimeData.crashCountMax);
    }
}
