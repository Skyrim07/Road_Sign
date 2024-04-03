using SKCell;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SKMonoSingleton<UIManager> 
{
    [SerializeField] SKUIPanel colorFactoryPanel, shapeFactoryPanel;
    [SerializeField] SKUIPanel winPanel;
    [SerializeField] SKUIPanel deathPanel, ticketPanel, crashPanel;
    [SerializeField] SKUIPanel tutPanel;
    [SerializeField] SKSlider progressBar;

    [SerializeField] CrashIndicator crashIndicator;
    [SerializeField] Transform lifeIconContainer;

    private LifeIcon[] lifeIcons;

    private void Start()
    {
        lifeIcons = new LifeIcon[lifeIconContainer.childCount];
        for (int i = 0; i < lifeIconContainer.childCount; i++)
        {
            lifeIcons[i] = lifeIconContainer.GetChild(i).GetComponent<LifeIcon>();  
        }

    }
    public void SetValue_ProgressBar(float value01)
    {
       // progressBar.SetValue(value01);
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
