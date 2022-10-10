using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationManager : MonoBehaviour
{
    [Header("Grid Parameters")]

    [SerializeField] public bool bShowGrid;
    [SerializeField] public int iCols;
    [SerializeField] public int iRows;
    [SerializeField] public float fTileSize;
    [SerializeField] public Vector3 v3OriginPosition;

    private Room[] RRoomsArray;


    void Start()
    {
        CreateGrid(iCols, iRows, fTileSize, v3OriginPosition, bShowGrid);
        foreach (Room room in RRoomsArray)
        {

        }
    }

   
    void Update()
    {
        
    }

    public void CreateGrid(int iCols, int iRows, float fTileSize, Vector3 v3OriginPosition, bool bShowGrid)
    {
        Grid GGameGrid = new Grid(iCols, iRows, fTileSize, v3OriginPosition);
        GGameGrid.bShowDebug = bShowGrid;
    }

    public void CreateRoom(int iXRoomPosition, int iYRoomPosition, int iRoomLength, int iRoomHeight, GameObject GoRoomOrigin)
    {
       // RRoomsArray[]
    }

    public void Generate()
    {
        foreach (Room RRoom in RRoomsArray)
        {
            GenerateRoom(RRoom); 
        }
    }

    public void GenerateRoom(Room RGeneratingRoom)
    {
        Instantiate(RGeneratingRoom.GoRoomBlock, new Vector3(RGeneratingRoomiX + iXRoomPosition, iY + iYRoomPosition, 0), Quaternion.identity);
    }
}
