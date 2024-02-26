using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

using SKCell;
[ExecuteInEditMode]
public sealed class LevelEditor : EditorWindow
{
    static SceneTitle sceneTitle;
    static string lastActiveScene;
    static int spawnPoint = 0;
    static LanguageSupport language;
   // static StartTeleportType startTeleportType;
    static bool waitForReset = false;

    static GameObject cam_Preview;

    private SceneTitle selectedSceneTitle;

    [MenuItem("RoadSign/Scene Controller")]
    public static void Initialize()
    {
        LevelEditor window = GetWindow<LevelEditor>("Scene Controller");
        Texture HierarchyIcon = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Editor/Textures/SceneController.png");
        GUIContent content = new GUIContent("Scene Controller", HierarchyIcon);
        window.titleContent = content;
    }

    private void OnGUI()
    {
 
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Scene:", GUILayout.Width(50));
            sceneTitle = (SceneTitle)PlayerPrefs.GetInt("StartScene");
            GUI.skin.button.fontStyle = FontStyle.Bold;
            GUI.contentColor = new Color(0.8f, 0.9f, 0.2f);
            if (GUILayout.Button(selectedSceneTitle.ToString(), GUILayout.Width(200)))
            {
                GenericMenu menu = new GenericMenu();
                SceneCategory[] sceneCategories = (SceneCategory[])System.Enum.GetValues(typeof(SceneCategory));

                foreach (SceneCategory category in sceneCategories)
                {
                    SceneTitle[] sceneTitlesInCategory = GetSceneTitlesForCategory(category);

                    foreach (SceneTitle sceneTitle in sceneTitlesInCategory)
                    {
                        SceneTitle localSceneTitle = sceneTitle;
                        SceneCategory localCategory = category;
                        menu.AddItem(new GUIContent(localCategory + "/" + localSceneTitle), false, () => { SelectScene(localSceneTitle); });
                    }
                }

                menu.ShowAsContext();
            }
            GUI.skin.button.fontStyle = FontStyle.Normal;
            GUI.contentColor = Color.white;
            PlayerPrefs.SetInt("StartScene", (int)selectedSceneTitle);

            EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Language:", GUILayout.Width(70));
        language = (LanguageSupport)PlayerPrefs.GetInt("StartLanguage");
        language = (LanguageSupport)EditorGUILayout.EnumPopup(language, GUILayout.Width(150));
        PlayerPrefs.SetInt("StartLanguage", (int)language);

        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);
        GUI.contentColor = new Color(0.9f, 0.8f, 0.2f);
        if (GUILayout.Button("Start!"))
        {
            EndCameraPreview();
            if (Application.isPlaying)
            {
               // Scenecontroller.instance.LoadSceneAsset(sceneTitle);
            }
            else
            {
                EditorApplication.ExitPlaymode();
                lastActiveScene = "Assets/Scenes/" + EditorSceneManager.GetActiveScene().name + ".unity";
                PlayerPrefs.SetString("LastActiveScene", lastActiveScene);

                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                EditorSceneManager.OpenScene("Assets/Scenes/Prepare.unity");
                EditorApplication.EnterPlaymode();
            }
        }
        GUI.contentColor = Color.white;
        if (Application.isPlaying)
        {
            if (GUILayout.Button("End!"))
            {
                EditorApplication.ExitPlaymode();
                waitForReset = true;
            }
        }
        else
        {
            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Open scene in editor"))
            {
                EndCameraPreview();
                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                EditorSceneManager.OpenScene("Assets/Scenes/" + sceneTitle.ToString() + ".unity");
            }
                
            if (GUILayout.Button("Open prepare scene"))
            {
                EndCameraPreview();
                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                EditorSceneManager.OpenScene("Assets/Scenes/Prepare.unity");
            }
            EditorGUILayout.EndHorizontal();
        }

        if (waitForReset)
        {
            if (!Application.isPlaying)
            {
                lastActiveScene = PlayerPrefs.GetString("LastActiveScene");
                EditorSceneManager.OpenScene(lastActiveScene);
                waitForReset = false;
            }
        }
    }
    void SelectScene(SceneTitle sceneTitle)
    {
        selectedSceneTitle = sceneTitle;
    }

    SceneTitle[] GetSceneTitlesForCategory(SceneCategory category)
    {
        return GlobalLibrary.G_SCENE_CATEGORY_DICT[category];
    }
    private void EndCameraPreview()
    {
        if (cam_Preview != null)
            DestroyImmediate(cam_Preview);
    }
}
