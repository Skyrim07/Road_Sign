using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    [HideInInspector]
    public List<Transform> waypoints = new List<Transform>();

    public List<RoadRestriction> restrictions = new List<RoadRestriction>();
    void Start()
    {
        Transform wp = transform.Find("Waypoints");
        for (int i = 0; i < wp.childCount; i++) 
        { 
            waypoints.Add(wp.GetChild(i));
        }
    }


}


public enum RoadRestriction
{
    NoRight, NoLeft, NoStraight
}
