using SKCell;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            SKConsole.Toggle();
        }
    }
}
