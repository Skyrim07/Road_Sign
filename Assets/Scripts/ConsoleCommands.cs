using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SKCell;

public class ConsoleCommands : MonoBehaviour
{
    void Start()
    {
        SKConsole.AddCommand("quit", "Quit the game.", ()=>Application.Quit());
    }
}
