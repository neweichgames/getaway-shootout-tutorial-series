using System;
using UnityEngine;
using UnityEngine.UI;

public class SplitScreenLinesUI : MonoBehaviour
{
    public float lineWidth = 5;
    public Color lineColor = Color.white;

    void Start()
    {
        CameraGrid grid = GetComponentInParent<CameraManager>().GetCameraGrid();
        int id = GetComponentInParent<CameraInfo>().GetCameraID();

        CreateLines(grid, id);
    }

    void CreateLines(CameraGrid grid, int id)
    {
        foreach (CameraGrid.GridDirection dir in Enum.GetValues(typeof(CameraGrid.GridDirection)))
            if (grid.HasNeighbor(id, dir))
                CreateUILine(dir);
    }

    void CreateUILine(CameraGrid.GridDirection dir)
    {
        RectTransform rt = new GameObject("GridLine_" + dir.ToString()).AddComponent<RectTransform>();
        rt.SetParent(transform);
        rt.localScale = Vector3.one;
        rt.localPosition = new Vector3(0, 0, 0);

        
        rt.anchorMin = new Vector2(dir == CameraGrid.GridDirection.RIGHT ? 1 : 0, dir == CameraGrid.GridDirection.UP ? 1 : 0);
        rt.anchorMax = new Vector2(dir == CameraGrid.GridDirection.LEFT ? 0 : 1, dir == CameraGrid.GridDirection.DOWN ? 0 : 1);
        if(dir == CameraGrid.GridDirection.LEFT || dir == CameraGrid.GridDirection.RIGHT)
            rt.sizeDelta = new Vector2(lineWidth, 0);
        else
            rt.sizeDelta = new Vector2(0, lineWidth);

        rt.gameObject.AddComponent<Image>().color = lineColor;
    }

}
