using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalLibrary 
{
    public static readonly Dictionary<SceneCategory, SceneTitle[]> G_SCENE_CATEGORY_DICT = new Dictionary<SceneCategory, SceneTitle[]>
    {
        {SceneCategory.All, new SceneTitle[]{SceneTitle.MainMenu, SceneTitle.Level1, SceneTitle.Level2, SceneTitle.Level3, SceneTitle.Level4} },
    };

}
public enum SceneCategory
{
    All,
}
public enum SceneTitle
{
    MainMenu,
    Level1,
    Level2,
    Level3,
    Level4,
}