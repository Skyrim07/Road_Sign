using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SKCell;
public class HatPanel : SKMonoSingleton<HatPanel>
{
    [SerializeField] Transform hatSlotContainer;

    HatSlot currentHatSlot;
    SKUIPanel panel;
    void Start()
    {
        panel = GetComponent<SKUIPanel>();
    }

    public void UpdateInfo()
    {
        for (int i = 0; i < hatSlotContainer.childCount; i++)
        {
            hatSlotContainer.GetChild(i).GetComponent<HatSlot>().UpdateInfo();  
        }
    }
    public void OnPressExit()
    {
        panel.SetState(false);
    }
    public void OnSelectHatSlot(HatSlot hatSlot)
    {
        if (currentHatSlot != null && currentHatSlot != hatSlot)
            currentHatSlot.OnDeselect();
        currentHatSlot = hatSlot;
    }
}
