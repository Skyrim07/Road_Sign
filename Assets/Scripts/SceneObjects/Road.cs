using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    [HideInInspector]
    public List<Transform> waypoints = new List<Transform>();
    public SignSlot mySignSlot;
    

    public List<RoadRestriction> restrictions = new List<RoadRestriction>();
    void Start()
    {
        Transform wp = transform.Find("Waypoints");
        for (int i = 0; i < wp.childCount; i++) 
        { 
            waypoints.Add(wp.GetChild(i));
        }
        //im not sure if we're planning on multiple sign slots or not but for now im just doing one
        mySignSlot = transform.Find("Signs_Slots").GetChild(0).GetComponent<SignSlot>();
        mySignSlot.myRoad = this;
    }


}


public enum RoadRestriction
{
    NoRight, NoLeft, NoStraight
}
