using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    public int iBlockType;
    public GameObject GoBlockObject;
    public int iXBlockPosition;
    public int iYBlockPosition;

    public Block(int iBlockType, GameObject GoBlockObject, int iXBlockPosition, int iYBlockPosition)
    {
        this.iBlockType = iBlockType;
        this.GoBlockObject = GoBlockObject;
        this.iXBlockPosition = iXBlockPosition;
        this.iYBlockPosition = iYBlockPosition;
    }
     
}

public class Room
{
    public GameObject GoRoomBlock;

    public Block[,] BRoomBlocks;
    public int iXRoomLength;
    public int iYRoomHeight;
    public int iXRoomPosition;
    public int iYRoomPosition;

    public Room(int iXRoomPosition, int iYRoomPosition, int iXRoomLength, int iYRoomHeight, GameObject GoRoomBlock)
    {
        this.iYRoomPosition = iYRoomPosition;
        this.iXRoomPosition = iXRoomPosition;
        this.iXRoomLength = iXRoomLength;
        this.iYRoomHeight = iYRoomHeight;
        this.GoRoomBlock = GoRoomBlock;

        for (int iX = 0; iX < iXRoomLength; iX++)
        {
            for (int iY = 0; iY < iYRoomHeight; iY++)
            {
                //Instantiate(GoRoomBlock, new Vector3(iX + iXRoomPosition, iY + iYRoomPosition, 0), Quaternion.identity);
                BRoomBlocks[iX + iXRoomPosition, iY + iYRoomPosition] = new Block(0, GoRoomBlock, iX + iXRoomPosition, iY + iYRoomPosition);
            }
        }

    }
}


// Implement a use for BlockType
// Maybe create another class for Tile in general