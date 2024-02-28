using SKCell;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SKMonoSingleton<UIManager> 
{
    [SerializeField] SKUIPanel colorFactoryPanel, shapeFactoryPanel;
    [SerializeField] SKUIPanel failPanel;
    [SerializeField] SKSlider progressBar;

    

    public void SetState_FailPanel(bool active)
    {
        failPanel.SetState(active);
    }
    public void SetState_ColorFactoryPanel(bool active)
    {
        colorFactoryPanel.SetState(active);
        if(active)
        ColorFactory.instance.Init();
    }
    public void SetState_ShapeFactoryPanel(bool active)
    {
        shapeFactoryPanel.SetState(active);
        if (active)
            ShapeFactory.instance.Init();
    }
}
