using SKCell;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level0Logic : MonoBehaviour
{
    [SerializeField] SKUIPanel[] panels;

    private void Start()
    {
        SetTimeScale(0);
    }
    public void SetTimeScale(float t)
    {
        RuntimeData.timeScale = t;
    }

    public void ClosePanel(int id)
    {
        panels[id].SetState(false);
        //panels[id].gameObject.SetActive(false);
    }

    public void OpenPanel(int id)
    {
        panels[id].SetState(true);
        //panels[id].gameObject.SetActive(true);
    }
}
