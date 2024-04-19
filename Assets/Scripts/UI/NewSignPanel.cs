using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SKCell;
public class NewSignPanel : SKMonoSingleton<NewSignPanel>
{
    SKUIPanel panel;
    private void Start()
    {
        panel = GetComponent<SKUIPanel>();
    }

    private void Update()
    {
        if (panel.active)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SetState(false);
            }
        }
    }
    public void SetState(bool active)
    {
        panel.SetState(active);
        if(!active)
        FlowManager.instance.UnPause();
    }

}
