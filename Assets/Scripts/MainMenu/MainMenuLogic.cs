using SKCell;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuLogic : SKMonoSingleton<MainMenuLogic>
{
    public Animator levelSelectAnim;
    public Button firstButton, levelSelectFirstButton;
    private void Start()
    {
        firstButton.Select();
    }

    public void OnPressStartButton()
    {
        levelSelectAnim.Appear();
        levelSelectFirstButton.Select();
    }
    public void OnPressBackMenuButton()
    {
        levelSelectAnim.Disappear();
        firstButton.Select();
    }
    public void OnPressSettingsButton()
    {
        UIManager.instance.SetState_CreditsPanel(true);
    }
    public void OnPressHatButton()
    {
        UIManager.instance.SetState_HatPanel(true);
    }
    public void OnPressQuitButton()
    {
        UIManager.instance.SetState_QuitConfirmPanel(true);
    }
}
