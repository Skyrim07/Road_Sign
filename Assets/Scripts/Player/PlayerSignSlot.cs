using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SKCell;
public class PlayerSignSlot : SKMonoSingleton<PlayerSignSlot>
{
    [SerializeField] SpriteRenderer signBG, signSprite, paint;

    private void Start()
    {
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        signBG.sprite = PlayerLogic.instance.sign == null? null : GetSpriteFromShape(PlayerLogic.instance.sign.shape);
        signBG.color = PlayerLogic.instance.sign == null ? Color.white: GetColorFromSignColor(PlayerLogic.instance.sign.color);
        paint.color = PlayerLogic.instance.sign == null ? Color.white: GetColorFromSignColor(PlayerLogic.instance.sign.color);
        signSprite.sprite = PlayerLogic.instance.sign == null ? null : GetSpriteFromSignType(PlayerLogic.instance.sign.type);

        //paint.gameObject.SetActive(PlayerLogic.instance.sign == null || PlayerLogic.instance.sign.type == SignType.None);
    }
    private Sprite GetSpriteFromSignType(SignType type)
    {
        switch (type)
        {
            case SignType.None:
                return null;
            case SignType.Stop:
                return CommonReference.instance.sprite_StopSign;
            case SignType.Rail:
                return CommonReference.instance.sprite_RailSign;
            default:
                return null;
        }
    }
    private Color GetColorFromSignColor(SignColor signColor)
    {
        switch (signColor)
        {
            case SignColor.None:
                return Color.white;
            case SignColor.Red:
                return CommonReference.instance.red;
            case SignColor.Blue:
                return CommonReference.instance.blue;
            default:
                return Color.white;
        }
    }
    private Sprite GetSpriteFromShape(SignShape shape)
    {
        switch (shape)
        {
            case SignShape.None:
                return null;
            case SignShape.Octogon:
                return CommonReference.instance.sprite_octogon;
            case SignShape.Diamond:
                return CommonReference.instance.sprite_diamond;
            default:
                return CommonReference.instance.sprite_octogon;
        }
    }
}
