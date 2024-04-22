using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PersistentData
{
    public static List<int> levelUnlocked = new List<int>() { 1 };
    public static List<int> signsDiscovered = new List<int>();

    public static List<int> hatUnlocked = new List<int>() { 0,1,2,3,4,5,6,7 };
    public static int equippedHat = 0;
}
