using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    public List<Road> connectTo = new List<Road>();
    [HideInInspector]
    public List<Transform> waypoints = new List<Transform>();
    void Start()
    {
        Transform wp = transform.Find("Waypoints");
        for (int i = 0; i < wp.childCount; i++) 
        { 
            waypoints.Add(wp.GetChild(i));
        }
    }


}
