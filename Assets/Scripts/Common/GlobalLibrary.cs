using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalLibrary 
{
    public static readonly Dictionary<SceneCategory, SceneTitle[]> G_SCENE_CATEGORY_DICT = new Dictionary<SceneCategory, SceneTitle[]>
    {
        {SceneCategory.All, new SceneTitle[]{SceneTitle.MainMenu, SceneTitle.Level0, SceneTitle.Level1, SceneTitle.Level2, SceneTitle.Level3, SceneTitle.Level4, SceneTitle.Level5, SceneTitle.Level6, SceneTitle.Level7, SceneTitle.Level8, SceneTitle.Level9, SceneTitle.Level10, SceneTitle.Level11, SceneTitle.Level12, SceneTitle.Level13, SceneTitle.Level14, SceneTitle.Level15} },
    };
    public static readonly Dictionary<SceneTitle, float> MaxCrashCounts = new Dictionary<SceneTitle, float>
    {
        { SceneTitle.Level0, 10f },
        { SceneTitle.Level1, 10f },
        { SceneTitle.Level2, 10f },
        { SceneTitle.Level3, 10f },
        { SceneTitle.Level4, 10f },
        { SceneTitle.Level5, 10f },
        { SceneTitle.Level6, 10f },
        { SceneTitle.Level7, 15f },
        { SceneTitle.Level8, 15f },
        { SceneTitle.Level9, 10f },
        { SceneTitle.Level10, 15f },
        { SceneTitle.Level11, 20f },
        { SceneTitle.Level12, 10f },
        { SceneTitle.Level13, 10f },
        { SceneTitle.Level14, 10f },
        { SceneTitle.Level15, 10f }
    };

}
public enum SceneCategory
{
    All,
}
public enum SceneTitle
{
    MainMenu,
    Level0,
    Level1,
    Level2,
    Level3,
    Level4,
    Level5,
    Level6,
    Level7,
    Level8,
    Level9,
    Level10,
    Level11,
    Level12,
    Level13,
    Level14,
    Level15,

}