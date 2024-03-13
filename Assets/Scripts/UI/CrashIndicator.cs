using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SKCell;

public class CrashIndicator : MonoBehaviour
{
    [SerializeField] SKText countText;
    [SerializeField] SKSlider slider;

    private float sliderInitialValue = 0.2f;

    public void SetValue(int current, int max)
    {
        float f  = current/(float)max;
        slider.SetValue(Mathf.Lerp(sliderInitialValue, 1, f));
        countText.UpdateTextDirectly($"{current}/{max}");
    }
}
