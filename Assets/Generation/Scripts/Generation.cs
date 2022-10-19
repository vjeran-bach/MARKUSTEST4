using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Generation : MonoBehaviour
{
    [Header("Generation Variables")]
    // delay time for loading the first hallways
    public float fWaitingTime;
    // delay time for loading additional rooms
    public float fAdditionalWaitingTime;

    [Header("Rooms")]
    // lists containing all the rooms
    public List<GameObject> GoSpawnRooms = new List<GameObject>();
    public List<GameObject> GoLargeRooms = new List<GameObject>();
    public List<GameObject> GoMediumRooms = new List<GameObject>();
    public List<GameObject> GoSmallRooms = new List<GameObject>();
    [SerializeField]
    private List<GameObject> GoAllRooms = new List<GameObject>();
    // chosen spawnroom & room next to generate
    public GameObject GoSpawnRoom;
    public GameObject GoChosenRoom;

    // lists containing all the hallways & the hallways next to generate
    public List<GameObject> GoLeftHallways = new List<GameObject>();
    public List<GameObject> GoRightHallways = new List<GameObject>();
    public GameObject GoLeftHallway;
    public GameObject GoRightHallway;

    [Header("Generated")]
    // currently generated room, its script and its hallways
    public GameObject GoGeneratedRoom;
    public Room RGeneratedRoom;
    public List<GameObject> GoGeneratedLeftHallways = new List<GameObject>();
    public List<GameObject> GoGeneratedRightHallways = new List<GameObject>();
    public List<GameObject> GoGeneratedPlaces = new List<GameObject>();
    public Room RGeneratedHallway;
    // Vector3 where the next room/hallway should generate
    [SerializeField] private Vector3 V3GeneratedRoomSpawnPoint;

    void Update()
    {
        
    }


    // main function
    public void Generate()
    {
        // add all rooms ro the full list  
        GoAllRooms.AddRange(GoLargeRooms);
        GoAllRooms.AddRange(GoMediumRooms);
        GoAllRooms.AddRange(GoSmallRooms);
        Debug.Log("[GEN] starting to generate");
        // generate a spawn room
        GenerateSpawnRoom();
        // generate hallways for the spawn room
        StartCoroutine(GenerateHallways(fWaitingTime, 0, GoSpawnRoom));
        // generate rooms at the end of the hallways
        StartCoroutine(LookForHallways(fWaitingTime, fAdditionalWaitingTime));
    }

    public void GenerateSpawnRoom()
    {
        // choose a spawn room at random
        GoSpawnRoom = GoRandomizedRoom(GoSpawnRooms);
        // create the spawn room
        Instantiate(GoSpawnRoom, new Vector3(0, 0, 0), Quaternion.identity);
        GoGeneratedPlaces.Add(GoSpawnRoom);
        Debug.Log("[GEN] SpawnRoom (" + GoSpawnRoom.name + ") at " + GoSpawnRoom.transform.position.x + ", " + GoSpawnRoom.transform.position.y);
    }

    public void GenerateLeftHallway(GameObject GoLeftDoor, Vector3 V3SpawnPosition)
    {
        // choose a random hallway
        //  GoCalculateRoom(GoLeftHallways, GoLeftHallway);
        GoLeftHallway = GoRandomizedRoom(GoLeftHallways);
        // add the position of the door to the position of 
        V3GeneratedRoomSpawnPoint = GoLeftDoor.transform.position + V3SpawnPosition;
        // create the hallway
        Instantiate(GoLeftHallway, V3GeneratedRoomSpawnPoint, Quaternion.identity);
        // add the hallway to all previously generated ones
        GoGeneratedLeftHallways.Add(GoLeftHallway);
        GoGeneratedPlaces.Add(GoLeftHallway);

        Debug.Log("[GEN] left hallway (" + GoLeftHallway.name + ") at " + GoLeftDoor.transform.position.x + ", " + GoLeftDoor.transform.position.y);
    }

    public void GenerateRightHallway(GameObject GoRightDoor, Vector3 V3SpawnPosition)
    {
        // -''-
        GoRightHallway = GoRandomizedRoom(GoRightHallways);
        V3GeneratedRoomSpawnPoint = GoRightDoor.transform.position + V3SpawnPosition;
        Instantiate(GoRightHallway, V3GeneratedRoomSpawnPoint, Quaternion.identity);
        GoGeneratedPlaces.Add(GoRightHallway);
        GoGeneratedRightHallways.Add(GoRightHallway);
        Debug.Log("[GEN] right hallway (" + GoRightHallway.name + ") at " + GoRightDoor.transform.position.x + ", " + GoRightDoor.transform.position.y);
    }

    public void GenerateRightRoom(GameObject GoRightDoor, Vector3 V3SpawnPosition)
    {
        // choose a random right room
        GoChosenRoom = GoRandomizedRoom(GoAllRooms);
        V3GeneratedRoomSpawnPoint += GoRightDoor.transform.position;
        Instantiate(GoChosenRoom, V3GeneratedRoomSpawnPoint, Quaternion.identity);
        GoGeneratedPlaces.Add(GoChosenRoom);
        Debug.Log("[GEN] right room (" + GoChosenRoom.name + ") at " + GoRightDoor.transform.position.x + ", " + GoRightDoor.transform.position.y);
    }


    private IEnumerator GenerateHallways(float fWaitingTime, float fAdditionalWaitingTime, GameObject GoGeneratedRoom)
    {
        // wait so everything has properly spawned in
        yield return new WaitForSeconds(fWaitingTime + fAdditionalWaitingTime);

        // set given room as the variable
        this.GoGeneratedRoom = GoGeneratedRoom;
        RGeneratedRoom = this.GoGeneratedRoom.GetComponent<Room>();
        Debug.Log("[GEN] generating Hallways for " + this.GoGeneratedRoom.name);
        // search for doors in the room
        RGeneratedRoom.FindDoors();

        // if there is a door, generate a hallway
        if (this.GoGeneratedRoom.GetComponent<Room>().GoLeftDoors != null)
        {
            Debug.Log("[GEN] possible left doors: " + this.GoGeneratedRoom.GetComponent<Room>().GoLeftDoors.Count);
            foreach (GameObject GoLeftDoor in this.GoGeneratedRoom.GetComponent<Room>().GoLeftDoors)
            {
                GenerateLeftHallway(GoLeftDoor, GoGeneratedRoom.transform.position);
            }
        }
        // -''-
        if (this.GoGeneratedRoom.GetComponent<Room>().GoRightDoors != null)
        {
            Debug.Log("[GEN] possible right doors: " + this.GoGeneratedRoom.GetComponent<Room>().GoRightDoors.Count);
            foreach (GameObject GoRightDoor in this.GoGeneratedRoom.GetComponent<Room>().GoRightDoors)
            {
                GenerateRightHallway(GoRightDoor, GoGeneratedRoom.transform.position);
            }
        }
        RGeneratedRoom.RemoveDoors();

    }

    private IEnumerator LookForHallways(float fWaitingTime, float fAdditionalWaitingTime)
    {
        yield return new WaitForSeconds(fWaitingTime + fAdditionalWaitingTime);
        {
            // generate a room for each other door
            foreach (GameObject GoGeneratedRightHallway in GoGeneratedRightHallways)
            {
                Debug.Log("[GEN] generating rooms for right hallway (" + GoGeneratedRightHallway.name + ") at " + GoGeneratedRightHallway.transform.position.x + ", " + GoGeneratedRightHallway.transform.position.y);
                RGeneratedHallway = GoGeneratedRightHallway.GetComponent<Room>();
                RGeneratedHallway.FindDoors();
                foreach (GameObject GoRightDoor in GoGeneratedRightHallway.GetComponent<Room>().GoRightDoors)
                {
                    GenerateRightRoom(GoRightDoor, GoGeneratedRightHallway.transform.position);
                }
            }
        }
    }

    public GameObject GoRandomizedRoom(List<GameObject> GoRooms)
    {
        bool bFound = false;
        int iIndex = 0;

        // find a random room, while taking its probability into account
        for (int i = 0; i < GoRooms.Count; i++)
        {
            while (!bFound)
            {

                int iRandomIndex = Random.Range(0, GoRooms.Count);
                Debug.Log("[GEN] calculating room with index: " + iRandomIndex + " which is " + GoRooms[iRandomIndex].name);
                if (GoRooms[iRandomIndex].GetComponent<Room>().fRoomSpawnChance >= Random.Range(0.0f, 99.0f))
                {
                    if (GoRooms != GoSpawnRooms)
                    {
                        // checking if there is already the same room somewhere
                        foreach (GameObject GoGeneratedPlace in GoGeneratedPlaces)
                        {
                            if (GoRooms[iRandomIndex].name != GoGeneratedPlace.name)
                            {
                                iIndex = iRandomIndex;
                                bFound = true;
                            }
                        }
                    } else
                    { 
                    iIndex = iRandomIndex;
                    bFound = true;
                    }

                }
            }
        }
        return GoRooms[iIndex];
    }

}

// listen to breakcore (:

// fix position of second room spawing
// add more rooms and hallways to test that everything is working


// implement up/down doors