using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SKCell;

public class PlayerLogic : SKMonoSingleton<PlayerLogic>
{
    public RoadSign sign;
    public void OnGetColor(SignColor color)
    {
        if (sign == null) 
        {
            sign = new RoadSign();
        }
        sign.color = color;
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
