using System;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    Waypoint[] waypoints;

    Dictionary<Transform, NavigatorInfo> navigatorInfo = new Dictionary<Transform, NavigatorInfo>();

    class NavigatorInfo
    {
        public int curWaypoint;
        public int spawnWaypoint;
    }

    void Awake()
    {
        waypoints = GetComponentsInChildren<Waypoint>();   
    }

    public void AddNav(Transform nav)
    {
        navigatorInfo.Add(nav, new NavigatorInfo());
    }

    void Update()
    {
        UpdateNavs();
        foreach (var nav in navigatorInfo)
        {
            Debug.DrawLine(nav.Key.position, waypoints[nav.Value.curWaypoint].transform.position, Color.green);
            Debug.DrawLine(nav.Key.position, waypoints[nav.Value.spawnWaypoint].transform.position, Color.yellow);
        }
    }

    void UpdateNavs()
    {
        foreach (var nav in navigatorInfo)
        {
            int curW = nav.Value.curWaypoint;
            Vector2 pos = nav.Key.position;

            // Check current waypoint ... 
            if (!waypoints[curW].InZone(pos))
            {
                if (curW > 0)
                    nav.Value.curWaypoint--;
            }
            else if (curW < waypoints.Length - 1 && waypoints[curW + 1].InZone(pos))
            {
                nav.Value.curWaypoint++;
                if (waypoints[curW + 1].spawnable)
                    nav.Value.spawnWaypoint = Mathf.Max(curW + 1, nav.Value.spawnWaypoint);
            }
        }
    }

    public Waypoint GetNavCurWaypoint(Transform nav)
    {
        NavigatorInfo info = navigatorInfo[nav];
        if (info == null)
            throw new KeyNotFoundException("Navigator was never added");

        return waypoints[info.curWaypoint];
    }

    public Waypoint GetNavNextWaypoint(Transform nav)
    {
        NavigatorInfo info = navigatorInfo[nav];
        if (info == null)
            throw new KeyNotFoundException("Navigator was never added");

        return waypoints[Mathf.Min(info.curWaypoint + 1, waypoints.Length - 1)];
    }

    public Waypoint GetNavSpawnWaypoint(Transform nav)
    {
        NavigatorInfo info = navigatorInfo[nav];
        if (info == null)
            throw new KeyNotFoundException("Navigator was never added");

        return waypoints[info.spawnWaypoint];
    }
}
