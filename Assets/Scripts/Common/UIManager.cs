using SKCell;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SKMonoSingleton<UIManager> 
{
    [SerializeField] SKUIPanel failPanel;

    public void SetState_FailPanel(bool active)
    {
        failPanel.SetState(active);
    }
}
