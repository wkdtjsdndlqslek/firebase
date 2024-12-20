using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomState
{
    Waiting,
    Playing,
    Finished
}

[Serializable]
public class Room
{
    public string host;
    public string guest;
    public RoomState state;
    public Dictionary<int,Turn> turn = new Dictionary<int, Turn>();
}

[Serializable]
public class Turn
{
    public bool isHostTurn;
    public string coodinate;
}