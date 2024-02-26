using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonActions : MonoBehaviour
{
   public void Pause()
    {
        FlowManager.instance.Pause();
    }
    public void UnPause()
    {
        FlowManager.instance.UnPause();
    }
}
