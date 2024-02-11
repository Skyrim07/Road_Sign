using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intersection : MonoBehaviour
{
    /// <summary>
    /// 0: down, 1: left, 2: up, 3: right
    /// </summary>
    public List<Road> roads = new List<Road>();


    public Road GetRoadRightOf(Road road)
    {
        int index = roads.IndexOf(road) - 1;
        return roads[index<0?roads.Count-1:index];
    }
    public Road GetRoadLeftOf(Road road)
    {
        int index = roads.IndexOf(road) + 1;
        return roads[index >=roads.Count ? 0 : index];
    }
    public Road GetRoadStraightOf(Road road)
    {
        int index = roads.IndexOf(road) + 2;
        return roads[index%4];
    }

    public Road GetRandomTargetRoadWithRestrictions(Road from)
    {
        List<Road> candidate = new List<Road>(roads);
        if(candidate.Contains(from))
            candidate.Remove(from);
        foreach(RoadRestriction r in from.restrictions)
        {
            if (r == RoadRestriction.NoRight)
                candidate.Remove(GetRoadRightOf(from));
            if (r == RoadRestriction.NoLeft)
                candidate.Remove(GetRoadLeftOf(from));
            if (r == RoadRestriction.NoStraight)
                candidate.Remove(GetRoadStraightOf(from));
        }

        return candidate[Random.Range(0, candidate.Count)]; 
    }
}

