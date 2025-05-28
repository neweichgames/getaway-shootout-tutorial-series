using UnityEngine;

public class Waypoint : MonoBehaviour
{

    public HorizontalDirection horizontalZone = HorizontalDirection.RIGHT;
    public VerticalDirection verticalZone;
    public Vector2 zoneOffset;

    public bool spawnable;

    public enum HorizontalDirection
    {
        NONE, LEFT, RIGHT
    }

    public enum VerticalDirection
    {
        NONE, DOWN, UP
    }

    public bool InZone(Vector2 pos)
    {
        Vector2 diff = pos - ((Vector2)transform.position + zoneOffset);

        if (horizontalZone != HorizontalDirection.NONE)
            if (diff.x > 0 != (horizontalZone == HorizontalDirection.RIGHT))
                return false;

        if (verticalZone != VerticalDirection.NONE)
            if(diff.y > 0 != (verticalZone == VerticalDirection.UP))
                return false;

        return true;
    }
}
