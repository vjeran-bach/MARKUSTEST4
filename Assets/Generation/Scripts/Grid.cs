using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using CodeMonkey.Utils;

public class Grid
{
    /* public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    } */


    [Header("Dimensions of the Grid")]
    private int iXGridCols;
    private int iYGridRows;
    // size of individual tiles
    private float fTileSize;
    // starting-point of the grid
    private Vector3 v3GridOriginPosition;
    // multidimnesional array to store the tiles
    private int[,] iGridArray;

    // bool to see the debug
    public bool bShowDebug;

    public Grid(int iCols, int iRows, float fTileSize, Vector3 v3OriginPosition)
    {
        // assigning the variables
        this.iXGridCols = iCols;
        this.iYGridRows = iRows;
        this.fTileSize = fTileSize;
        this.v3GridOriginPosition = v3OriginPosition;

        // creating an array to keep track of the rows and cols
        iGridArray = new int[iXGridCols, iYGridRows];

        bShowDebug = true;
        if (bShowDebug)
        {
            //TextMesh[,] debugTextArray = new TextMesh[width, height];

            // for every row and column in the grid
            for (int iX = 0; iX < iGridArray.GetLength(0); iX++)
            {
                for (int iY = 0; iY < iGridArray.GetLength(1); iY++)
                {
                    // Debug: Draw the lines in the grid
                    //debugTextArray[x, y] = UtilsClass.CreateWorldText(gridArray[x, y].ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 30, Color.white, TextAnchor.MiddleCenter);
                    Debug.DrawLine(v3GetWorldPosition(iX, iY), v3GetWorldPosition(iX, iY + 1), Color.white, 100f);
                    Debug.DrawLine(v3GetWorldPosition(iX, iY), v3GetWorldPosition(iX + 1, iY), Color.white, 100f);
                }
            }

            // Debug: Draw the last lines at the end of the grid
            Debug.DrawLine(v3GetWorldPosition(0, iRows), v3GetWorldPosition(iCols, iRows), Color.white, 100f);
            Debug.DrawLine(v3GetWorldPosition(iCols, 0), v3GetWorldPosition(iCols, iRows), Color.white, 100f);  
        }
    }


    // Methods to get the number of columns, rows, the size of the tiles, & the origin-position 
    public int iGetCols()
    {
        return iXGridCols;
    }

    public int iGetRows()
    {
        return iYGridRows;
    }

    public float fGetTileSize()
    {
        return fTileSize;
    }

    public Vector3 v3GetWorldPosition(int iX, int iY)
    {
        return new Vector3(iX, iY) * fTileSize + v3GridOriginPosition;
    }

    // Method to change a Vector3 into its X and Y values
    private void iGetColsRows(Vector3 v3WorldPosition, out int iX, out int iY)
    {
        iX = Mathf.FloorToInt((v3WorldPosition - v3GridOriginPosition).x / fTileSize);
        iY = Mathf.FloorToInt((v3WorldPosition - v3GridOriginPosition).y / fTileSize);
    }

    // Method to set the value (aka. what block should appear) of a specific tile
    public void iSetValue(int iX, int iY, int iValue)
    {
        if (iX >= 0 && iY >= 0 && iX < iXGridCols && iY < iYGridRows)
        {
            iGridArray[iX, iY] = iValue;
            //if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y });
        }
    }

    // Method to change a Vector3 into its X and Y values
    public void iSetValue(Vector3 worldPosition, int value)
    {
        int iX, iY;
        iGetColsRows(worldPosition, out iX, out iY);
        iSetValue(iX, iY, value);
    }

    // Method to get the value (aka. what block should appear) of a specific tile
    public int iGetValue(int iX, int iY)
    {
        if (iX >= 0 && iY >= 0 && iX < iXGridCols && iY < iYGridRows)
        {
            return iGridArray[iX, iY];
        }
        else
        {
            return 0;
        }
    }

    // Method to change a Vector3 into its X and Y values
    public int iGetValue(Vector3 worldPosition)
    {
        int iX;
        int iY;
        iGetColsRows(worldPosition, out iX, out iY);
        return iGetValue(iX, iY);
    }

}
