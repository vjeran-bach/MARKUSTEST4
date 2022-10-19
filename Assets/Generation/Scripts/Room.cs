using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Room : MonoBehaviour
{
    [Header("Variables")]
    // chance of the room being accepted if picked at random (100 = if picked at random, gets chosen)
    [Range(0.0f, 100.0f)] public float fRoomSpawnChance;
    // type of room
    public int iTypeOfRoom;
    
    [Header("Doors")]
    // doors in the room
    public List<GameObject> GoLeftDoors = new List<GameObject>();
    public List<GameObject> GoRightDoors = new List<GameObject>();
    public List<GameObject> GoTopDoors = new List<GameObject>();
    public List<GameObject> GoBottomDoors = new List<GameObject>();

    public virtual void FindDoors()
    {
        // find all children of these children and add them to the list
        Transform TRoomTransform = this.transform;
        Debug.Log("[ROOM:" + this.name + "] Children count: " + TRoomTransform.childCount);
        for (int i = 0; i < TRoomTransform.childCount; i++)
        {
            if (TRoomTransform.GetChild(i).gameObject.name == "Left Doors" && TRoomTransform.GetChild(i).childCount != 0)
            {
                Debug.Log("[ROOM:" + this.name + "] Left doors count: " + TRoomTransform.GetChild(i).childCount);
                for (int j = 0; j < TRoomTransform.GetChild(i).childCount; j++)
                {
                    Debug.Log("[ROOM:" + this.name + "] generated left door: " + TRoomTransform.GetChild(i).GetChild(j).gameObject.name);
                    GoLeftDoors.Add(TRoomTransform.GetChild(i).GetChild(j).gameObject);
                }
            }
            if (TRoomTransform.GetChild(i).gameObject.name == "Right Doors" && TRoomTransform.GetChild(i).childCount != 0)
            {
                Debug.Log("[ROOM:" + this.name + "] Right doors count: " + TRoomTransform.GetChild(i).childCount);
                for (int j = 0; j < TRoomTransform.GetChild(i).childCount; j++)
                {
                    Debug.Log("[ROOM:" + this.name + "] generated right door: " + TRoomTransform.GetChild(i).GetChild(j).gameObject.name);
                    GoRightDoors.Add(TRoomTransform.GetChild(i).GetChild(j).gameObject);
                }
            }
            if (TRoomTransform.GetChild(i).gameObject.name == "Top Doors" && TRoomTransform.GetChild(i).childCount != 0)
            {
                Debug.Log("[ROOM:" + this.name + "] Top doors count: " + TRoomTransform.GetChild(i).childCount);
                for (int j = 0; j < TRoomTransform.GetChild(i).childCount; j++)
                {
                    Debug.Log("[ROOM:" + this.name + "] generated top door: " + TRoomTransform.GetChild(i).GetChild(j).gameObject.name);
                    GoTopDoors.Add(TRoomTransform.GetChild(i).GetChild(j).gameObject);
                }
            }
            if (TRoomTransform.GetChild(i).gameObject.name == "Bottom Doors" && TRoomTransform.GetChild(i).childCount != 0)
            {
                Debug.Log("[ROOM:" + this.name + "] Bottom doors count: " + TRoomTransform.GetChild(i).childCount);
                for (int j = 0; j < TRoomTransform.GetChild(i).childCount; j++)
                {
                    Debug.Log("[ROOM:" + this.name + "] generated bottom door: " + TRoomTransform.GetChild(i).GetChild(j).gameObject.name);
                    GoBottomDoors.Add(TRoomTransform.GetChild(i).GetChild(j).gameObject);
                }
            }
        }
    }

    public virtual void RemoveDoors()
    {
        GoLeftDoors.Clear();
        GoRightDoors.Clear();
        GoTopDoors.Clear();
        GoBottomDoors.Clear();
    }


}

