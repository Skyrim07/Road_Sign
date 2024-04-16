using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalLibrary 
{
    public static readonly Dictionary<SceneCategory, SceneTitle[]> G_SCENE_CATEGORY_DICT = new Dictionary<SceneCategory, SceneTitle[]>
    {
        {SceneCategory.All, new SceneTitle[]{SceneTitle.MainMenu, SceneTitle.Level0, SceneTitle.Level1, SceneTitle.Level2, SceneTitle.Level3, SceneTitle.Level4, SceneTitle.Level5, SceneTitle.Level6, SceneTitle.Level7, SceneTitle.Level8, SceneTitle.Level9, SceneTitle.Level10, SceneTitle.Level11} },
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
}