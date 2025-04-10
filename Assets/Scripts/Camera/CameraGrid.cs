using System;
using UnityEngine;

public class CameraGrid
{
    private int numElements, numCols, numRows, gridSize;
    private bool forceEqual;

    public enum GridDirection { LEFT, RIGHT, UP, DOWN };

    public CameraGrid(int numElements, bool forceEqual = false)
    {
        this.numElements = numElements;
        this.forceEqual = forceEqual;

        numCols = (int)Mathf.Ceil(Mathf.Sqrt(numElements));
        numRows = (int)Mathf.Ceil((float)numElements / numCols);

        gridSize = numCols * numRows;
    }

    public Rect GetGridRect(int elementIndex)
    {
        if (elementIndex >= numElements)
            throw new ArgumentOutOfRangeException("Element out of grid max size!");

        int c = elementIndex % numCols;
        int r = elementIndex / numCols;

        int numRowsInCol = numRows;
        if (!forceEqual && c >= (numCols - (gridSize - numElements)))
            numRowsInCol--;

        float colSize = 1f / numCols;
        float rowSize = 1f / numRowsInCol;

        return new Rect(c * colSize, 1 - (r + 1) * rowSize, colSize, rowSize);
    }

    public bool HasNeighbor(int elementIndex, GridDirection direction)
    {
        if (elementIndex >= numElements)
            throw new ArgumentOutOfRangeException("Element out of grid max size!");

        switch (direction)
        {
            case GridDirection.LEFT:
                return elementIndex % numCols > 0;
            case GridDirection.RIGHT:
                return elementIndex % numCols < numCols - 1;
            case GridDirection.UP:
                return elementIndex / numCols > 0;
            case GridDirection.DOWN:
                int numRowsInCol = numRows;
                if (!forceEqual && (elementIndex % numCols) >= (numCols - (gridSize - numElements)))
                    numRowsInCol--;

                return elementIndex / numCols < numRowsInCol - 1;
            default: return false;
        }
    }
}