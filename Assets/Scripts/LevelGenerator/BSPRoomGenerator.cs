// 
// Copyright (c) 2023 Kim Hyun Deok
//
// BSPRoomGenerator.cs
//

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Generate the dungeon using Binary Space Partitioning Algorithm 
/// </summary>
public class BSPRoomGenerator : MonoBehaviour
{
    public float horizontalSplitWeight = 0.5f;
    public int minRoomWidth = 10, minRoomHeight = 10;
    public int dungeonWidth = 100, dungeonHeight = 100;
    private List<BoundsInt> roomList;

    private void Start()
    {
        roomList = GenerateRooms(
            new BoundsInt(new Vector3Int(0, 0), new Vector3Int(dungeonWidth, dungeonHeight, 0)),
            minRoomWidth, minRoomHeight);
    }

    private void OnDrawGizmos()
    {
        if (roomList != null)
        {
            foreach (var room in roomList)
            {
                for (int y = room.yMin; y <= room.yMax; y++)
                {
                    for (int x = room.xMin; x <= room.xMax; x++)
                    {
                        Vector3 pos = new Vector3(x, y, 0);
                        Gizmos.DrawCube(pos, Vector3.one);
                    }
                }
            }
        }
    }

    public List<BoundsInt> GenerateRooms(BoundsInt spaceToSplit, int minWidth, int minHeight)
    {
        Queue<BoundsInt> queue = new Queue<BoundsInt>();
        List<BoundsInt> rooms = new List<BoundsInt>();
        queue.Enqueue(spaceToSplit);

        while (queue.Count > 0)
        {
            var room = queue.Dequeue();
            if (room.size.y >= minHeight && room.size.x >= minWidth)
            {
                if (Random.value < horizontalSplitWeight)
                {
                    if (room.size.y >= minHeight * 2)
                    {
                        SplitHorizontally(minWidth, queue, room);
                    }
                    else if (room.size.x >= minWidth * 2)
                    {
                        SplitVertically(minWidth, queue, room);
                    }
                    else
                    {
                        rooms.Add(room);
                    }
                }
                else
                {
                    if (room.size.x >= minWidth * 2)
                    {
                        SplitVertically(minWidth, queue, room);
                    }
                    else if (room.size.y >= minHeight * 2)
                    {
                        SplitHorizontally(minWidth, queue, room);
                    }
                    else
                    {
                        rooms.Add(room);
                    }
                }
            }
        }
        return rooms;
    }

    private void SplitVertically(int minWidth, Queue<BoundsInt> queue, BoundsInt room)
    {
        var xRandomSplit = Random.Range(1, room.size.x);
        BoundsInt leftRoom = new BoundsInt(room.min, new Vector3Int(xRandomSplit, room.size.y, room.size.z));
        BoundsInt rightRoom = new BoundsInt(new Vector3Int(room.min.x + xRandomSplit, room.min.y, room.min.z), 
            new Vector3Int(room.size.x - xRandomSplit, room.size.y, room.size.z));

        queue.Enqueue(leftRoom);
        queue.Enqueue(rightRoom);
    }

    private void SplitHorizontally(int minWidth, Queue<BoundsInt> queue, BoundsInt room)
    {
        var yRandomSplit = Random.Range(1, room.size.y);
        BoundsInt upperRoom = new BoundsInt(room.min, new Vector3Int(room.size.x, yRandomSplit, room.size.z));
        BoundsInt downRoom = new BoundsInt(new Vector3Int(room.min.x, room.min.y + yRandomSplit, room.min.z),
            new Vector3Int(room.size.x, room.size.y - yRandomSplit, room.size.z));

        queue.Enqueue(upperRoom);
        queue.Enqueue(downRoom);
    }
}
