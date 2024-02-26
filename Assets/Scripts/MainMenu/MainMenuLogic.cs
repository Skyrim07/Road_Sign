using SKCell;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuLogic : MonoBehaviour
{
    public Animator levelSelectAnim;
    public void OnPressStartButton()
    {
        levelSelectAnim.Appear();
    }
    public void OnPressBackMenuButton()
    {
        levelSelectAnim.Disappear();
    }
    public void OnPressSettingsButton()
    {

    }
    public void OnPressQuitButton()
    {
        Application.Quit();
    }
}
