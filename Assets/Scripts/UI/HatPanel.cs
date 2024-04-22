using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SKCell;
public class HatPanel : SKMonoSingleton<HatPanel>
{
    [SerializeField] Transform hatSlotContainer;
    [SerializeField] Transform hatSpriteContainer;
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
        for (int i = 0; i < hatSpriteContainer.childCount; i++)
        {
            hatSpriteContainer.GetChild(i).gameObject.SetActive(PersistentData.equippedHat == i);
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

        for (int i = 0; i < hatSpriteContainer.childCount; i++)
        {
          
            hatSpriteContainer.GetChild(i).gameObject.SetActive(PersistentData.equippedHat == i);
        }
    }
}
