using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public struct Coordinate : INetworkSerializable
{
    public int x;
    public int y;
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref x);
        serializer.SerializeValue(ref y);
    }
    public Coordinate(int _x, int _y)
    {
        this.x = _x;
        this.y = _y;
    }
}

public enum TILEVALUE { NONE, X, O }