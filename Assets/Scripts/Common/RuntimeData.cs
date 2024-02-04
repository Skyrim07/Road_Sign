using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeData : MonoBehaviour
{
    public static float timeScale = 1.0f;
    public static float deltaTime {  get { return timeScale * Time.deltaTime; } }
    public static float fixedDeltaTime {  get { return timeScale * Time.fixedDeltaTime; } }
    public static bool isPaused = false;
}
