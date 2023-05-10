using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateRooms : MonoBehaviour
{
    float ScreenWidth;
    ///////
    ///////
    [SerializeField] GameObject[] availableRooms;
    [SerializeField] GameObject firstRoom;
    Queue<GameObject> CurrentRoomsQueue;
    float lastRoomEndX;
    float firstRoomEndX;
    /////////
    /////////
    [SerializeField] GameObject[] availableObjects;
    Queue<GameObject> CurrentObjectsQueue;
    float lastObjectX;
    float firstObjectX;
    [SerializeField] float minObjectRotation, maxObjectRotation;
    [SerializeField] float minDiffBetweenObjectsX, MaxDiffBetweenObjectsX, minObjectsY, MaxObjectsY;
    void Start()
    {
        CurrentRoomsQueue = new Queue<GameObject>();
        CurrentRoomsQueue.Enqueue(firstRoom);
        float ScreenHeight = Camera.main.orthographicSize * 2;
        ScreenWidth = ScreenHeight * Camera.main.aspect;
        float StartRoomWidth = firstRoom.transform.Find("floor").transform.localScale.x;
        firstRoomEndX = StartRoomWidth / 2 + firstRoom.transform.position.x;
        lastRoomEndX = firstRoomEndX;
        ////////
        ////////
        CurrentObjectsQueue = new Queue<GameObject>();
        AddObjects();
        firstObjectX = lastObjectX;
        InvokeRepeating("AddDel", 0f, 0.5f);
    }
    void AddDel()
    {
        if (firstRoomEndX <= gameObject.transform.position.x - ScreenWidth)
        {
            RemoveRooms();
        }
        if (lastRoomEndX <= gameObject.transform.position.x + ScreenWidth)
        {
            addRooms();
        }
        if (lastObjectX <= gameObject.transform.position.x + ScreenWidth)
        {
            AddObjects();
        }
        if (firstObjectX <= gameObject.transform.position.x - ScreenWidth)
        {
            RemoveObjects();
        }
    }
    void addRooms()
    {
        int IndexNumber = Random.Range(0, availableRooms.Length);
        GameObject newRoom = Instantiate(availableRooms[IndexNumber]);
        float length = newRoom.transform.Find("floor").transform.localScale.x;
        newRoom.transform.position = new Vector3(lastRoomEndX + (length / 2), 0, 0);
        lastRoomEndX += length;
        CurrentRoomsQueue.Enqueue(newRoom);
    }
    void RemoveRooms()
    {
        GameObject room = CurrentRoomsQueue.Dequeue();
        Destroy(room);
        firstRoomEndX = CurrentRoomsQueue.Peek().transform.position.x;
    }
    void AddObjects()
    {

        int IndexNumber = Random.Range(0, availableObjects.Length);
        GameObject newObject = Instantiate(availableObjects[IndexNumber]);
        float positionX = lastObjectX + Random.Range(minDiffBetweenObjectsX, MaxDiffBetweenObjectsX);
        float positionY = Random.Range(minObjectsY, MaxObjectsY);
        newObject.transform.position = new Vector3(positionX, positionY, 0);
        CurrentObjectsQueue.Enqueue(newObject);
        lastObjectX = positionX;
        float rotation = Random.Range(minObjectRotation, maxObjectRotation);
        newObject.transform.rotation = Quaternion.Euler(0, 0, rotation);
    }
    void RemoveObjects()
    {
        GameObject firstObject = CurrentObjectsQueue.Dequeue();
        Destroy(firstObject);
        firstObjectX = CurrentObjectsQueue.Peek().transform.position.x;
    }
}