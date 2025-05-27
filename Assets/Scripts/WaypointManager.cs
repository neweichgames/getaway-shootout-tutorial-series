using System;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    Waypoint[] waypoints;

    List<Transform> navigators = new List<Transform>();
    List<int> curWaypoints = new List<int>();

    void Awake()
    {
        waypoints = GetComponentsInChildren<Waypoint>();   
    }

    public void AddNav(Transform nav)
    {
        navigators.Add(nav);
        curWaypoints.Add(0);
    }

    void Update()
    {
        UpdateNavs();
        for (int i = 0; i < navigators.Count; i++)
        {
            Debug.DrawLine(navigators[i].position, waypoints[curWaypoints[i]].transform.position, Color.green);
        }
    }

    void UpdateNavs()
    {
        for (int i = 0; i < navigators.Count; i++)
        {
            int curW = curWaypoints[i];

            // Check current waypoint ... 
            if (!waypoints[curW].InZone(navigators[i].position))
            {
                if (curW > 0)
                    curWaypoints[i]--;
            }
            else if (curW < waypoints.Length - 1 && waypoints[curW + 1].InZone(navigators[i].position))
                curWaypoints[i]++;
        }
    }

    public Waypoint GetNavCurWaypoint(Transform nav)
    {
        int i = navigators.IndexOf(nav);
        if (i < 0)
        {
            Debug.LogError("player not found");
            return null;
        }
        
        return waypoints[curWaypoints[i]];
    }
}
