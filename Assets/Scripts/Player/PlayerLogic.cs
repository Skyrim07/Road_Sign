using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SKCell;

public class PlayerLogic : SKMonoSingleton<PlayerLogic>
{

    public RoadSign sign;
    private KeyCode discardSign = KeyCode.X;

   
    public void OnGetColor(SignColor color)
    {
        if (sign == null) 
        {
            sign = new RoadSign();
        }
        sign.color = color;
    
        CheckSignCreation();
        PlayerSignSlot.instance.UpdateVisual();
    }
    private void Update()
    {
        if (Input.GetKeyDown(discardSign) && sign != null)
        {
            DiscardSign(sign);
        }

    }
    public void DiscardSign(RoadSign discardedSign)
    {
        sign.shape = SignShape.None;
        sign.color = SignColor.None;
        sign.type = SignType.None;
        PlayerSignSlot.instance.UpdateVisual();
    }
    public void OnGetShape(SignShape shape)
    {
        if (sign == null)
        {
            sign = new RoadSign();
        }
        sign.shape = shape;

        CheckSignCreation();
        PlayerSignSlot.instance.UpdateVisual();
    }

    /// <summary>
    /// Whenever the player gets a color or a shape, this is called to perform sign creation
    /// </summary>
    public void CheckSignCreation()
    {
        //hardcoded placeholder
        if(sign.color == SignColor.Red && sign.shape == SignShape.Octogon)
        {
            OnSignCreated(SignType.Stop);
        }
    }

    public void OnSignCreated(SignType type)
    {
        sign.type = SignType.Stop;
        NewSignPanel.instance.SetState(true);
        FlowManager.instance.Pause();
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
    public SignType type;
}
