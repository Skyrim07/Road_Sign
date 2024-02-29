using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SKCell;

public class ShapeFactory : SKMonoSingleton<ShapeFactory>
{
    bool canSelect = false;
    public void Init()
    {
        canSelect = true;
    }
    public void OnSelectOctogon()
    {
        OnSelectSomething();
        PlayerLogic.instance.OnGetShape(SignShape.Octogon);
        UIManager.instance.SetState_ShapeFactoryPanel(false);
    }
    public void OnSelectDiamond()
    {
        OnSelectSomething();
        PlayerLogic.instance.OnGetShape(SignShape.Diamond);
        UIManager.instance.SetState_ShapeFactoryPanel(false);

    }

    private void OnSelectSomething()
    {
        SKAudioManager.instance.PlaySound("shape_factory");
    }


}
