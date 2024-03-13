using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeData : MonoBehaviour
{
    public static SceneTitle currentScene;
    public static float timeScale = 1.0f;
    public static float deltaTime {  get { return timeScale * Time.deltaTime; } }
    public static float fixedDeltaTime {  get { return timeScale * Time.fixedDeltaTime; } }
    public static bool isPaused = false;

    public static float currentProgress;

    public static float playerHealth;

    public static float crashCount;
    public static float crashCountMax = 10;

    public static int currentSignCount;
    public static int signCountMax;

}
