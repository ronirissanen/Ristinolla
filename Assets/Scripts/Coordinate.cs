using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Coordinate
{
    public int x;
    public int y;

    public Coordinate(int _x, int _y)
    {
        this.x = _x;
        this.y = _y;
    }
}

public enum TILEVALUE { NONE, X, O }