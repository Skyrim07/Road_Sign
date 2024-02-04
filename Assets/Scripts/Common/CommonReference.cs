using SKCell;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonReference : SKMonoSingleton<CommonReference>
{
    public SKUIPanel startPanel, pausePanel;
    public SKText pause_TitleText;
}
