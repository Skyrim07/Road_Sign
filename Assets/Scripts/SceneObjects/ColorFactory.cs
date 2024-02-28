using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SKCell;
public class ColorFactory : SKMonoSingleton<ColorFactory>
{
    public float rotateSpeed = 1.0f;
    [SerializeField] Transform pointer;

    private bool isMoving = false;
    public void Init()
    {
        isMoving = true;
        pointer.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void FixedUpdate()
    {
        if (!isMoving) return;
        pointer.transform.Rotate(0, 0, 1 * rotateSpeed);
    }

    private void Update()
    {
        if (!isMoving) return;
        if (Input.GetMouseButtonDown(0))
        {
            StopRotation();
        }
    }

    private void StopRotation()
    {
        isMoving = false;
        UIManager.instance.SetState_ColorFactoryPanel(false);

        float angle = pointer.eulerAngles.z;
        while (angle > 360)
            angle -= 360;
        while (angle <0)
            angle += 360;
        SignColor c = angle <180 ? SignColor.Red : SignColor.Blue;
        PlayerLogic.instance.OnGetColor(c);
    }
}
public enum SignType
{
    None,
    Stop
}

public enum SignColor
{
    None,
    Red,
    Blue
}
public enum SignShape
{
    None,
    Octogon,
    Diamond
}
