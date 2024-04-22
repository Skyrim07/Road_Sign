using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SKCell;

public class PlayerLogic : SKMonoSingleton<PlayerLogic>
{

    public RoadSign sign;
    private KeyCode discardSign = KeyCode.X;
    private bool inFactory;

    public Transform hatsContainer;

    public void UpdateHatVisual()
    {
        for (int i = 0; i < hatsContainer.childCount; i++)
        {
            hatsContainer.GetChild(i).gameObject.SetActive(PersistentData.equippedHat == i);
        }
    }

   public static bool HasValidSign()
    {
        bool has = true;
        has &= PlayerLogic.instance.sign != null && PlayerLogic.instance.sign.type != SignType.None;
        return has;
    }
    public void AddHealth(int delta)
    {
        RuntimeData.playerHealth+= delta;
        UIManager.instance.UpdateLifeIcon();
    }
    public void SetHealth(int hp)
    {
        RuntimeData.playerHealth = hp;
        UIManager.instance.UpdateLifeIcon();
    }
    public void DestroySign()
    {
        sign = null;
        PlayerSignSlot.instance.UpdateVisual();
    }
    public void OnGetColor(SignColor color)
    {
        SKAudioManager.instance.PlaySound("paint3");
        if (sign == null) 
        {
            sign = new RoadSign();
        }
        sign.color = color;
        inFactory = false;

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
    public void DiscardSign(RoadSign sign)
    {
        SKAudioManager.instance.PlaySound("signdelete");

        sign.shape = SignShape.None;
        sign.color = SignColor.None;
        sign.type = SignType.None;
        PlayerSignSlot.instance.UpdateVisual();
    }
    public bool InFactory()
    {
        return inFactory;
    }
    public void OnGetShape(SignShape shape)
    {
        if (sign == null)
        {
            sign = new RoadSign();
        }
        sign.shape = shape;
        inFactory = false;

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
        if (sign.color == SignColor.Blue && sign.shape == SignShape.Diamond)
        {
            OnSignCreated(SignType.Rail);
        }
    }

    public void OnSignCreated(SignType type)
    {
        sign.type = type;
        sign.shape = SignShape.None;
        sign.color = SignColor.None;
        if (!RuntimeData.signsDiscovered.Contains(type))
        {
            if(type == SignType.Stop)
                NewSignPanel.instance.SetState(true);
            if (type == SignType.Rail)
                NewSignPanel_2.instance.SetState(true);

            FlowManager.instance.Pause();
            SKUtils.InsertToList(RuntimeData.signsDiscovered, type, false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ColorFactory"))
        {

            
        }
        if (collision.CompareTag("ShapeFactory"))
        {

        }
    }

    public void EnterShapeFactory()
    {
        if (sign == null || sign.shape == SignShape.None)
        {
            SKAudioManager.instance.PlaySound("door flapping alt");

            UIManager.instance.SetState_ShapeFactoryPanel(true);
            inFactory = true;
        }

    }
    public void EnterColorFactory()
    {
        if (sign == null || sign.color == SignColor.None)
        {
            SKAudioManager.instance.PlaySound("door flapping alt");

            UIManager.instance.SetState_ColorFactoryPanel(true);
            inFactory = true;
        }
    }
}

public class RoadSign
{
    public SignColor color;
    public SignShape shape;
    public SignType type;
}
