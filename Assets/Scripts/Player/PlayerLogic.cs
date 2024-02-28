using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SKCell;

public class PlayerLogic : SKMonoSingleton<PlayerLogic>
{
    public RoadSign sign;
    private KeyCode discardSign = KeyCode.X;

    private void Start()
    {
        EventDispatcher.AddListener(EventDispatcher.Common, EventRef.ON_LOAD_LEVEL, new SKEvent(() =>
        {
            PlayerLogic.instance.SetProgressValue(0);
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
    public void OnGetColor(SignColor color)
    {
        if (sign == null) 
        {
            sign = new RoadSign();
        }
        sign.color = color;
        PlayerSignSlot.instance.UpdateVisual();
    }
    private void Update()
    {
        if (Input.GetKeyDown(discardSign) && sign != null)
        {
            DiscardSign(sign);
        }
    }
    public void DiscardSign(RoadSign sign)
    {
        sign.shape = SignShape.None;
        sign.color = SignColor.None;
        PlayerSignSlot.instance.UpdateVisual();
    }
    public void OnGetShape(SignShape shape)
    {
        if (sign == null)
        {
            sign = new RoadSign();
        }
        sign.shape = shape;
        PlayerSignSlot.instance.UpdateVisual();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ColorFactory"))
        {
            UIManager.instance.SetState_ColorFactoryPanel(true);
        }
        if (collision.CompareTag("ShapeFactory"))
        {
            UIManager.instance.SetState_ShapeFactoryPanel(true);
        }
    }
}

public class RoadSign
{
    public SignColor color;
    public SignShape shape;
}
