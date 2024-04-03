using SKCell;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEffector : MonoBehaviour
{
    public void PlaySound(string name)
    {
        SKAudioManager.instance.PlaySound(name, null, false, 1, Random.Range(.8f, 1.2f));
    }
}
