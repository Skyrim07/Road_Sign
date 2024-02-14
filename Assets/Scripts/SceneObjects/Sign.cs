using System.Collections.Generic;
using UnityEngine;

public class Sign : MonoBehaviour
{
    private SpriteRenderer spriteRend;
    public enum SignType
    {
        Stop
    }
    public List<Sprite> sprites = new List<Sprite>();
    public SignType type;

    private void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();
    }

    public void ChangeSprite(SignType myType)
    {
        switch (myType)
        {
            case SignType.Stop:
                spriteRend.sprite = sprites[0];
                break;
        }
    }
    //will have index of different sign sprites that it assigns itself based on its type
}