using UnityEngine;
using UnityEditor;
using static Waypoint;

[CustomEditor(typeof(Waypoint))]
public class WaypointEditor : Editor
{
    const float cubeSize = 1000f;

    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
    static void DrawGizmosSelected(Waypoint waypoint, GizmoType gizmoType)
    {
        if (waypoint.horizontalZone == HorizontalDirection.NONE && waypoint.verticalZone == VerticalDirection.NONE)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawCube(waypoint.transform.position, Vector3.one / 2f);
            return;
        }

        if (((int)gizmoType & (int)GizmoType.Active) != 0)
            DrawCube(waypoint);

        if (waypoint.spawnable)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(waypoint.transform.position, 0.25f);
        }
        else
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawSphere(waypoint.transform.position, 0.125f);
        }

        Waypoint nextWaypoint = NextWaypoint(waypoint);
        if(nextWaypoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(waypoint.transform.position, nextWaypoint.transform.position);
        }
    }

    static void DrawCube(Waypoint waypoint)
    {
        Gizmos.color = new Color(1f, 1f, 0, 0.5f);
        Vector2 offset = Vector2.zero;

        switch (waypoint.horizontalZone)
        {
            case HorizontalDirection.LEFT:
                offset += Vector2.right * (-cubeSize / 2f);
                break;
            case HorizontalDirection.RIGHT:
                offset += Vector2.right * cubeSize / 2f;
                break;
        }

        switch (waypoint.verticalZone)
        {
            case VerticalDirection.DOWN:
                offset += Vector2.up * (-cubeSize / 2f);
                break;
            case VerticalDirection.UP:
                offset += Vector2.up * cubeSize / 2f;
                break;
        }

        Gizmos.DrawCube(waypoint.transform.position + (Vector3)offset + (Vector3)waypoint.zoneOffset, new Vector3(cubeSize, cubeSize, 1f));
    }

    static Waypoint NextWaypoint(Waypoint waypoint)
    {
        Transform parent = waypoint.transform.parent;
        for (int i = waypoint.transform.GetSiblingIndex() + 1; i < parent.childCount; i++)
        {
            Waypoint w = parent.GetChild(i).GetComponent<Waypoint>();
            if (w != null)
                return w;
        }

        return null;
    }
}
