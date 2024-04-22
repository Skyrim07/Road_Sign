using SKCell;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    [Tooltip("Name of the music file you want to play")]
    [SerializeField] string track;
    [Tooltip("Whether previous music should be stopped")]
    [SerializeField] bool reset;
    void Start()
    {
        SKAudioManager.instance.StopMusic();
        SKAudioManager.instance.PlayMusic(track, reset, 0, 1);
    }
}
