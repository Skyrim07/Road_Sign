using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Intersection : MonoBehaviour
{
    /// <summary>
    /// 0: down, 1: left, 2: up, 3: right
    /// </summary>
    public List<Road> roads = new List<Road>();
    public List<Sign> signs = new List<Sign>();


    private Vector2[] directions = new Vector2[] {Vector2.down, Vector2.left, Vector2.up, Vector2.right};
    private void Awake()
    {
        Road[] r = new Road[roads.Count];
        //Initialize connecting roads
        for (int i = 0; i < directions.Length; i++)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, directions[i], 2f);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.transform.GetComponent<Road>() != null)
                {
                    r[i]=hit.transform.GetComponent<Road>();
                    break;
                }
            }
        }

        roads = new List<Road>(r);

    }

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
        if (from != null)
        {
            if (candidate.Contains(from))
                candidate.Remove(from);
            foreach (RoadRestriction r in from.restrictions)
            {
                if (r == RoadRestriction.NoRight)
                    candidate.Remove(GetRoadRightOf(from));
                if (r == RoadRestriction.NoLeft)
                    candidate.Remove(GetRoadLeftOf(from));
                if (r == RoadRestriction.NoStraight)
                    candidate.Remove(GetRoadStraightOf(from));
            }
        }


        Road res = candidate[Random.Range(0, candidate.Count)];
        while (res == null)
        {
            res = candidate[Random.Range(0, candidate.Count)];
        }
        return res;
    }
}
