using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    Waypoint[] waypoints;
    float[] waypointDist;

    Dictionary<Transform, NavigatorInfo> navigatorInfo = new Dictionary<Transform, NavigatorInfo>();

    class NavigatorInfo
    {
        public int curWaypoint;
        public int spawnWaypoint;
        public float distToFinish;
    }

    void Awake()
    {
        waypoints = GetComponentsInChildren<Waypoint>();
        waypointDist = new float[waypoints.Length];

        float dist = 0f;
        for (int i = waypoints.Length - 2; i >= 0; i--)
        {
            dist += ((Vector2)waypoints[i+1].transform.position - (Vector2)waypoints[i].transform.position).magnitude;
            waypointDist[i] = dist;
        }
    }

    public void AddNav(Transform nav)
    {
        NavigatorInfo info = new NavigatorInfo();
        info.distToFinish = waypointDist[0];
        navigatorInfo.Add(nav, info);
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

            float distToWaypoint = ((Vector2)waypoints[nav.Value.curWaypoint].transform.position - (Vector2)nav.Key.position).magnitude;
            nav.Value.distToFinish = distToWaypoint + waypointDist[nav.Value.curWaypoint];
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

    public Transform[] GetNavDistanceOrder()
    {
        return navigatorInfo.OrderBy(pair => pair.Value.distToFinish).Select(pair => pair.Key).ToArray();
    }
}
